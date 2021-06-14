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
            var query =
                @"select cp.IdUser, cp.Enabled, cp.Active, cp.Interval [IntervalInDays], cp.Amount [EmailsAmountByInterval], u.Email [AccountName]
                from [User] u
                inner join [UserShippingLimit] cp on u.IdUser = cp.IdUser where u.Email = @Email;
                select sl.IdSubscribersList [Id], sl.Name
                from [SubscribersListXShippingLimit] sls
                inner join [SubscribersList] sl on sl.IdSubscribersList = sls.IdSubscribersList
                inner join [User] u on u.IdUser = sl.IdUser where u.Email = @Email;";

            var queryParams = new {Email = accountName};
            var multipleAsync = await connection.QueryMultipleAsync(query, queryParams);
            var userWithContactPoliciesFound = (await multipleAsync.ReadAsync()).Select(x => new
            {
                x.AccountName,
                x.Enabled,
                x.Active,
                x.EmailsAmountByInterval,
                x.IntervalInDays
            }).FirstOrDefault();

            Entities.ContactPoliciesSettings contactPoliciesResult = null;

            if (userWithContactPoliciesFound != null)
            {
                if (userWithContactPoliciesFound.Enabled)
                {
                    contactPoliciesResult = CreateBasicContactPoliciesSettings(userWithContactPoliciesFound.AccountName,
                        userWithContactPoliciesFound.Active, userWithContactPoliciesFound.EmailsAmountByInterval,
                        userWithContactPoliciesFound.IntervalInDays);
                    contactPoliciesResult.ExcludedSubscribersLists = contactPoliciesResult.Active
                        ? (await multipleAsync.ReadAsync<ExcludedSubscribersLists>()).ToList()
                        : null;
                }
                else
                {
                    contactPoliciesResult = CreateBasicContactPoliciesSettings(userWithContactPoliciesFound.AccountName,
                        userWithContactPoliciesFound.Active, null,
                        null);
                }
            }

            return contactPoliciesResult;
        }

        #endregion

        #region private methods

        private Entities.ContactPoliciesSettings CreateBasicContactPoliciesSettings(string accountName, bool active,
            int? emailsAmountByInterval, int? intervalInDays)
        {
            return new()
            {
                AccountName = accountName,
                Active = active,
                EmailsAmountByInterval = emailsAmountByInterval,
                IntervalInDays = intervalInDays
            };
        }

        #endregion
    }
}
