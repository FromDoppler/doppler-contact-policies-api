using Dapper;
using Doppler.ContactPolicies.Data.Access.Core;
using Doppler.ContactPolicies.Data.Access.Entities;
using System;
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

        public async Task<Entities.ContactPoliciesSettings> GetContactPoliciesSettingsAsync(int idUser)
        {
            using var connection = await _databaseConnectionFactory.GetConnection();
            const string query =
                @"select uslIdUser, usl.Active, usl.Interval [IntervalInDays], usl.Amount [EmailsAmountByInterval], u.Email [AccountName]
                from [User] u
                left join [UserShippingLimit] usl on u.IdUser = usl.IdUser and usl.Enabled = 1
                where u.IdUser = @IdUser;
                select sl.IdSubscribersList [Id], sl.Name
                from [SubscribersListXShippingLimit] sls
                inner join [SubscribersList] sl on sl.IdSubscribersList = sls.IdSubscribersList
                inner join [User] u on u.IdUser = sl.IdUser where u.IdUser = @IdUser;";

            var queryParams = new { IdUser = idUser };
            var multipleAsync = await connection.QueryMultipleAsync(query, queryParams);
            var contactPoliciesSettings =
                (await multipleAsync.ReadAsync<Entities.ContactPoliciesSettings>()).FirstOrDefault();

            if (contactPoliciesSettings.IdUser == null)
                return contactPoliciesSettings;

            contactPoliciesSettings.ExcludedSubscribersLists =
                (await multipleAsync.ReadAsync<ExcludedSubscribersLists>()).ToList();

            return contactPoliciesSettings;
        }

        public async Task UpdateContactPoliciesSettingsAsync(int idUser, Entities.ContactPoliciesSettings contactPoliciesToInsert)
        {

            using var connection = await _databaseConnectionFactory.GetConnection();

            using var transaction = connection.BeginTransaction();
            const string updateQuery =
                @"update [UserShippingLimit] set Active = @Active, Interval = @IntervalInDays, Amount = @EmailsAmountByInterval
                from [UserShippingLimit] usl
                inner join [User] u on u.IdUser = usl.IdUser where u.IdUser = @IdUser and usl.Enabled = 1;";

            var affectedRows = await connection.ExecuteAsync(updateQuery, new
            {
                IdUser = idUser,
                contactPoliciesToInsert.IntervalInDays,
                contactPoliciesToInsert.EmailsAmountByInterval,
                contactPoliciesToInsert.Active
            }, transaction);

            if (affectedRows == 0)
                throw new Exception($"This action is not allowed for this user.");

            await connection.ExecuteAsync(
                @"delete [SubscribersListXShippingLimit] from [SubscribersListXShippingLimit] slxsl inner join [User] u on u.IdUser = slxsl.IdUser where u.IdUser = @IdUser and  IdSubscribersList not in @Ids;",
                new { IdUser = idUser, Ids = contactPoliciesToInsert.ExcludedSubscribersLists.Select(x => x.Id) }, transaction);

            const string updateExclusionList = "insert into SubscribersListXShippingLimit(IdUser, IdSubscribersList, Active) " +
                                                "select sl.IdUser, sl.IdSubscribersList, 1 as Active from SubscribersList sl inner join [User] u on u.IdUser = sl.IdUser " +
                                                "inner join UserShippingLimit usl on u.IdUser = usl.IdUser " +
                                                "left join SubscribersListXShippingLimit slxsl on slxsl.IdUser = sl.IdUser and slxsl.IdSubscribersList = sl.IdSubscribersList " +
                                                "where slxsl.IdSubscribersList IS NULL " +
                                                "and u.IdUser = @IdUser " +
                                                "and sl.IdSubscribersList in @IdsExcludedSubscriberList";

            await connection.ExecuteAsync(updateExclusionList, new
            {
                IdUser = idUser,
                IdsExcludedSubscriberList = contactPoliciesToInsert.ExcludedSubscribersLists.Select(s => s.Id)
            }, transaction);

            transaction.Commit();
        }

        public async Task<int?> GetIdUserByAccountName(string accountName)
        {
            using var connection = await _databaseConnectionFactory.GetConnection();
            const string query =
                @"select IdUser from [User] u where u.Email = @Email";
            var queryParams = new { Email = accountName };
            return await connection.QueryFirstOrDefaultAsync<int?>(query, queryParams);
        }

        #endregion
    }
}
