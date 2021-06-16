using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using Doppler.ContactPolicies.Data.Access.Core;
using Doppler.ContactPolicies.Data.Access.Entities;

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
                @"select cp.IdUser, cp.Enabled [EnabledExcludedSubscribersList], cp.Active, cp.Interval [IntervalInDays], cp.Amount [EmailsAmountByInterval], u.Email [AccountName]
                from [User] u
                left join [UserShippingLimit] cp on u.IdUser = cp.IdUser
                where u.Email = @Email;
                select sl.IdSubscribersList [Id], sl.Name
                from [SubscribersListXShippingLimit] sls
                inner join [SubscribersList] sl on sl.IdSubscribersList = sls.IdSubscribersList
                inner join [User] u on u.IdUser = sl.IdUser where u.Email = @Email;";

            var queryParams = new {Email = accountName};
            var multipleAsync = await connection.QueryMultipleAsync(query, queryParams);
            var contactPoliciesSettings =
                (await multipleAsync.ReadAsync<Entities.ContactPoliciesSettings>()).FirstOrDefault();

            if (contactPoliciesSettings is {IdUser: null})
                return null;

            if (contactPoliciesSettings.EnabledExcludedSubscribersList)
            {
                contactPoliciesSettings.ExcludedSubscribersLists =
                    (await multipleAsync.ReadAsync<ExcludedSubscribersLists>()).ToList();
            }

            return contactPoliciesSettings;
        }

        #endregion
    }
}
