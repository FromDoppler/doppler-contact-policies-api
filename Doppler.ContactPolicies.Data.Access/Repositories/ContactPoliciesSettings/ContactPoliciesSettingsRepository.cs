using Dapper;
using Doppler.ContactPolicies.Data.Access.Core;
using Doppler.ContactPolicies.Data.Access.Entities;
using System;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings
{
    public class ContactPoliciesSettingsRepository : IContactPoliciesSettingsRepository
    {
        private readonly IDatabaseConnectionFactory _databaseConnectionFactory;

        public ContactPoliciesSettingsRepository(IDatabaseConnectionFactory databaseConnectionFactory)
        {
            _databaseConnectionFactory = databaseConnectionFactory;
        }

        #region public methods

        public async Task<Entities.ContactPoliciesSettings> GetContactPoliciesSettingsByIdUserAsync(int idUser)
        {
            using var connection = _databaseConnectionFactory.GetConnection();
            const string query =
                @"select convert(bit, (case when usl.IdUser is null then 0 else 1 end)) as UserHasContactPolicies, usl.Active, usl.Interval [IntervalInDays], usl.Amount [EmailsAmountByInterval], u.Email [AccountName]
                from [User] u
                left join [UserShippingLimit] usl on u.IdUser = usl.IdUser
                where u.IdUser = @IdUser;
                select sl.IdSubscribersList [Id], sl.Name
                from [SubscribersListXShippingLimit] sls
                inner join [SubscribersList] sl on sl.IdSubscribersList = sls.IdSubscribersList
                inner join [User] u on u.IdUser = sl.IdUser where u.IdUser = @IdUser;";

            var queryParams = new { IdUser = idUser };
            var multipleAsync = await connection.QueryMultipleAsync(query, queryParams);
            var contactPoliciesSettings =
                (await multipleAsync.ReadAsync<Entities.ContactPoliciesSettings>()).First();

            if (!contactPoliciesSettings.UserHasContactPolicies)
                return contactPoliciesSettings;

            contactPoliciesSettings.ExcludedSubscribersLists =
                (await multipleAsync.ReadAsync<ExcludedSubscribersLists>()).ToList();

            return contactPoliciesSettings;
        }

        public async Task<ContactPoliciesTimeRestriction> GetContactPoliciesTimeRestrictionByIdUserAsync(int idUser)
        {
            using var connection = _databaseConnectionFactory.GetConnection();
            const string query =
                @"select
                    ucptr.TimeSlotEnabled [TimeSlotEnabled],
                    ucptr.HourFrom [HourFrom],
                    ucptr.HourTo [HourTo],
                    ucptr.WeekdaysEnabled [WeekdaysEnabled],
                    u.Email [AccountName]
                from [User] u
                left join [UserContactPolicyTimeRestriction] ucptr on u.IdUser = ucptr.IdUser
                where u.IdUser = @IdUser;";

            var queryParams = new { IdUser = idUser };
            var contactPoliciesTimeRestriction = await connection.QueryFirstOrDefaultAsync<ContactPoliciesTimeRestriction>(query, queryParams);

            return contactPoliciesTimeRestriction;
        }

        public async Task UpdateContactPoliciesSettingsAsync(
            int idUser,
            Entities.ContactPoliciesSettings contactPoliciesToInsert,
            ContactPoliciesTimeRestriction contactPoliciesTimeRestrictionToInsert
        )
        {
            using var connection = _databaseConnectionFactory.GetConnection();
            connection.Open();

            using var transaction = connection.BeginTransaction();
            const string upsertQuery =
                @"update [UserShippingLimit] set Active = @Active, Interval = @IntervalInDays, Amount = @EmailsAmountByInterval
                from [UserShippingLimit] usl
                where usl.IdUser = @IdUser;

                if @@ROWCOUNT = 0
                    insert into [UserShippingLimit] ([IdUser] ,[Active] ,[Amount] ,[Interval]) VALUES (@IdUser, @Active, @EmailsAmountByInterval, @IntervalInDays);";

            var affectedRows = await connection.ExecuteAsync(upsertQuery, new
            {
                IdUser = idUser,
                contactPoliciesToInsert.IntervalInDays,
                contactPoliciesToInsert.EmailsAmountByInterval,
                contactPoliciesToInsert.Active
            }, transaction);

            if (affectedRows == 0)
                throw new Exception($"User shipping limit could not be updated");

            await connection.ExecuteAsync(
                @"delete [SubscribersListXShippingLimit] from [SubscribersListXShippingLimit] slxsl where slxsl.IdUser = @IdUser and  IdSubscribersList not in @Ids;",
                new { IdUser = idUser, Ids = contactPoliciesToInsert.ExcludedSubscribersLists.Select(x => x.Id) }, transaction);

            const string updateExclusionList = "insert into SubscribersListXShippingLimit(IdUser, IdSubscribersList, Active) " +
                                                "select sl.IdUser, sl.IdSubscribersList, 1 as Active from SubscribersList sl " +
                                                "inner join UserShippingLimit usl on sl.IdUser = usl.IdUser " +
                                                "left join SubscribersListXShippingLimit slxsl on slxsl.IdUser = sl.IdUser and slxsl.IdSubscribersList = sl.IdSubscribersList " +
                                                "where slxsl.IdSubscribersList IS NULL " +
                                                "and sl.IdUser = @IdUser " +
                                                "and sl.IdSubscribersList in @IdsExcludedSubscriberList";

            await connection.ExecuteAsync(updateExclusionList, new
            {
                IdUser = idUser,
                IdsExcludedSubscriberList = contactPoliciesToInsert.ExcludedSubscribersLists.Select(s => s.Id)
            }, transaction);

            // Time Restriction Update
            await UpdateContactPoliciesTimeRestrictionAsync(connection, transaction, idUser, contactPoliciesTimeRestrictionToInsert);

            transaction.Commit();
        }

        private async Task UpdateContactPoliciesTimeRestrictionAsync(IDbConnection connection, IDbTransaction transaction, int idUser, ContactPoliciesTimeRestriction contactPoliciesTimeRestrictionToInsert)
        {
            if (contactPoliciesTimeRestrictionToInsert == null)
            {
                return;
            }

            const string upsertQuery =
                @"update [UserContactPolicyTimeRestriction] set TimeSlotEnabled = @TimeSlotEnabled, HourFrom = @HourFrom, HourTo = @HourTo, WeekdaysEnabled = @WeekdaysEnabled
                from [UserContactPolicyTimeRestriction] ucptr
                where ucptr.IdUser = @IdUser;

                if @@ROWCOUNT = 0
                    insert into [UserContactPolicyTimeRestriction] ([IdUser] ,[TimeSlotEnabled] ,[HourFrom] ,[HourTo], [WeekdaysEnabled]) VALUES (@IdUser, @TimeSlotEnabled, @HourFrom, @HourTo, @WeekdaysEnabled);";

            var affectedRows = await connection.ExecuteAsync(upsertQuery, new
            {
                IdUser = idUser,
                contactPoliciesTimeRestrictionToInsert.TimeSlotEnabled,
                contactPoliciesTimeRestrictionToInsert.HourFrom,
                contactPoliciesTimeRestrictionToInsert.HourTo,
                contactPoliciesTimeRestrictionToInsert.WeekdaysEnabled
            }, transaction);

            if (affectedRows == 0)
            {
                throw new Exception("User Contact Policy Time Restriction could not be updated");
            }
        }

        public async Task<int?> GetIdUserByAccountName(string accountName)
        {
            using var connection = _databaseConnectionFactory.GetConnection();
            const string query =
                @"select IdUser from [User] u where u.Email = @Email";
            var queryParams = new { Email = accountName };
            return await connection.QueryFirstOrDefaultAsync<int?>(query, queryParams);
        }

        #endregion
    }
}
