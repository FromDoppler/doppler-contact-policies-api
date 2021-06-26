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
            {
                return null;
            }

            if (contactPoliciesSettings.IdUser == null)
            {
                return contactPoliciesSettings;
            }

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
            var contactPoliciesSettings = await connection.QueryFirstOrDefaultAsync<Entities.ContactPoliciesSettings>(query, queryParams);

            if (contactPoliciesSettings == null)
            {
                return null;
            }

            if (contactPoliciesSettings.IdUser == null)
            {
                return contactPoliciesSettings;
            }

            return contactPoliciesSettings;
        }

        public async Task<bool> InsertContactPoliciesSettingsAsync(Entities.ContactPoliciesSettings contactPoliciesToInsert)
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

                int insertedRows = 0;
                foreach (var item in contactPoliciesToInsert.ExcludedSubscribersLists)
                {

                    insertedRows += await connection.ExecuteAsync(
                                @"if exists (select IdUser,IdSubscribersList from [SubscribersList] where IdUser = @IdUser and IdSubscribersList = @IdSubscribersList)
                                if not exists (select IdUser,IdSubscribersList from [SubscribersListXShippingLimit] where IdUser = @IdUser and IdSubscribersList = @IdSubscribersList)
                                insert [SubscribersListXShippingLimit] (IdUser, IdSubscribersList, Active)
                                values(@IdUser,@IdSubscribersList, @Active)", new
                                { IdUser = contactPoliciesToInsert.IdUser, IdSubscribersList = item.Id, Active = true }, transaction: transaction);
                }

                transaction.Commit();
            }


            return await Task.FromResult(true);
        }
        #endregion
    }
}
