using Dapper;
using Doppler.ContactPolicies.Data.Access.Core;
using Doppler.ContactPolicies.Data.Access.Entities;
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

        public async Task<Entities.ContactPoliciesSettings> GetContactPoliciesSettingsAsync(string accountName)
        {
            using var connection = await _databaseConnectionFactory.GetConnection();
            const string query =
                @"select cp.IdUser, cp.Active, cp.Interval [IntervalInDays], cp.Amount [EmailsAmountByInterval], u.Email [AccountName]
                from [User] u
                left join [UserShippingLimit] cp on u.IdUser = cp.IdUser and cp.Enabled = 1
                where u.Email = @Email;
                select sl.IdSubscribersList [Id], sl.Name
                from [SubscribersListXShippingLimit] sls
                inner join [SubscribersList] sl on sl.IdSubscribersList = sls.IdSubscribersList
                inner join [User] u on u.IdUser = sl.IdUser where u.Email = @Email;";

            var queryParams = new { Email = accountName };
            var multipleAsync = await connection.QueryMultipleAsync(query, queryParams);
            var contactPoliciesSettings =
                (await multipleAsync.ReadAsync<Entities.ContactPoliciesSettings>()).FirstOrDefault();

            if (contactPoliciesSettings == null)
                return null;

            if (contactPoliciesSettings.IdUser == null)
                return contactPoliciesSettings;

            contactPoliciesSettings.ExcludedSubscribersLists =
                (await multipleAsync.ReadAsync<ExcludedSubscribersLists>()).ToList();

            return contactPoliciesSettings;
        }

        public async Task<Entities.ContactPoliciesSettings> GetBasicContactPoliciesSettingsAsync(string accountName)
        {
            using var connection = await _databaseConnectionFactory.GetConnection();
            const string query =
                @"select cp.IdUser, cp.Active, cp.Interval [IntervalInDays], cp.Amount [EmailsAmountByInterval], u.Email [AccountName]
                from [User] u
                left join [UserShippingLimit] cp on u.IdUser = cp.IdUser and cp.Enabled = 1
                where u.Email = @Email;";

            var queryParams = new { Email = accountName };
            return await connection.QueryFirstOrDefaultAsync<Entities.ContactPoliciesSettings>(query, queryParams);
        }

        public async Task<bool> UpdateContactPoliciesSettingsAsync(Entities.ContactPoliciesSettings contactPoliciesToInsert)
        {

            using var connection = await _databaseConnectionFactory.GetConnection();

            using (var transaction = connection.BeginTransaction())
            {
                const string updateQuery =
                    @"update [UserShippingLimit] set Active = @Active, Interval = @IntervalInDays, Amount = @EmailsAmountByInterval where IdUser = @IdUser;";

                var affectedRows = await connection.ExecuteAsync(updateQuery, contactPoliciesToInsert, transaction: transaction);

                var deletedRows = await connection.ExecuteAsync(
                @"delete from [SubscribersListXShippingLimit] where IdUser = @IdUser and  IdSubscribersList not in @Ids;",
                new { IdUser = contactPoliciesToInsert.IdUser, Ids = contactPoliciesToInsert.ExcludedSubscribersLists.Select(x => x.Id) }, transaction: transaction);


                var paramIds = contactPoliciesToInsert.ExcludedSubscribersLists.Select(s => s.Id);

                string updateExclusionList = "insert into SubscribersListXShippingLimit(IdUser, IdSubscribersList, Active) " +
                                            "select sl.IdUser, sl.IdSubscribersList, 1 as Active from SubscribersList sl join [User] u ON u.IdUser = sl.IdUser " +
                                            "join UserShippingLimit usl on u.IdUser = usl.IdUser " +
                                            "left join SubscribersListXShippingLimit slxsl on slxsl.IdUser = sl.IdUser and slxsl.IdSubscribersList = sl.IdSubscribersList " +
                                            "where slxsl.IdSubscribersList IS NULL " +
                                            "and usl.Enabled = 1 and u.IdUser = @IdUser " +
                                            "and sl.IdSubscribersList in @IdsExcludedSubscriberList";

                var updated = await connection.ExecuteAsync(updateExclusionList, new
                {
                    IdUser = contactPoliciesToInsert.IdUser,
                    IdsExcludedSubscriberList = paramIds
                }, transaction);

                transaction.Commit();
            }
            return await Task.FromResult(true);
        }
        #endregion
    }
}
