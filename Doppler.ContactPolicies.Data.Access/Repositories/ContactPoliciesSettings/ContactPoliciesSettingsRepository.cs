using Dapper;
using Doppler.ContactPolicies.Data.Access.Core;
using Doppler.ContactPolicies.Data.Access.Entities;
using System.Collections.Generic;
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

        public async Task<int?> GetUserIdAsync(string accountName)
        {
            using var connection = await _databaseConnectionFactory.GetConnection();
            const string query =
                 @"select IdUser from [User] u where u.Email = @Email;";
            var queryParams = new { Email = accountName };
            return await connection.QueryFirstOrDefaultAsync<int?>(query, queryParams);
        }

        public async Task UpdateContactPoliciesSettingsAsync(Entities.ContactPoliciesSettings contactPoliciesSettings)
        {
            using var connection = await _databaseConnectionFactory.GetConnection();

            const string updateQuery =
                @"UPDATE [UserShippingLimit] SET Active = @Active, Interval = @IntervalInDays, Amount = @EmailsAmountByInterval WHERE IdUser = @IdUser;";
            await connection.ExecuteAsync(updateQuery, contactPoliciesSettings
            );
        }

        public async Task<List<ExcludedSubscribersLists>> VerifyUserSubscriberListExist(int? idUser,
    List<ExcludedSubscribersLists> excludedSubscribersLists)
        {
            using var connection = await _databaseConnectionFactory.GetConnection();

            const string query =
                @"Select Count (IdUser) FROM [SubscribersList] WHERE IdSubscribersList IN @Ids and IdUser = @IdUser;
                Select sl.IdSubscribersList [Id], sl.Name
                from [SubscribersListXShippingLimit] sls
                inner join [SubscribersList] sl on sl.IdSubscribersList = sls.IdSubscribersList
                inner join [User] u on u.IdUser = sl.IdUser where sl.IdUser = @IdUser;";
            var queryParams = new
            {
                IdUser = idUser,
                Ids = excludedSubscribersLists.Select(x => x.Id)
            };
            var multipleAsync = await connection.QueryMultipleAsync(query, queryParams);

            var existExcludedSubscriberList = (await multipleAsync.ReadAsync<int>()).FirstOrDefault();

            // Excluded list for that user doesnt match
            if (excludedSubscribersLists.Count != existExcludedSubscriberList)
                return null;

            // Verify if can be null in case user doesnt have any List
            return (await multipleAsync.ReadAsync<ExcludedSubscribersLists>()).ToList();
        }

        public async Task UpdateExludedSubscribersListAsync(int? IdUser, List<int> excludedSubscribersToInsert,
            List<int> excludedSubscribersToDelete)
        {
            using var connection = await _databaseConnectionFactory.GetConnection();

            if (excludedSubscribersToDelete.Count > 0)
            {
                var deletedRows = await connection.ExecuteAsync(
                    @"DELETE FROM [SubscribersListXShippingLimit] WHERE IdSubscribersList In @excludedSubscribersToDelete;",
                    new { excludedSubscribersToDelete });
            }

            if (excludedSubscribersToInsert.Count > 0)
            {

                var insertedRows = await connection.ExecuteAsync(
                    @"INSERT [SubscribersListXShippingLimit] (IdUser, IdSubscribersList, Active) 
                    values(@IdUser,@IdSubscribersList, @Active)",
                    excludedSubscribersToInsert.Select(id => new
                    { IdUser = IdUser, IdSubscribersList = id, Active = true }));
            }
        }

        #endregion
    }
}
