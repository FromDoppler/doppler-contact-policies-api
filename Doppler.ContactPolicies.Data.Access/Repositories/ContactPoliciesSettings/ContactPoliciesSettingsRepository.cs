using System;
using System.Collections.Generic;
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

        public async Task<Entities.ContactPoliciesSettings> GetContactPoliciesSettingsAsync(string accountName)
        {
            using var connection = await _databaseConnectionFactory.GetConnection();
            var query = @"select cp.IdUser, cp.Enabled,cp.Active,cp.Interval,cp.Amount,sl.IdSubscribersList, sl.Name
                from [UserShippingLimit] cp 
                inner join [SubscribersListXShippingLimit] sls on sls.IdUser = cp.IdUser
                inner join [SubscribersList] sl on sl.IdSubscribersList = sls.IdSubscribersList";
            var contactPoliciesSettings = await connection
                .QueryAsync<Entities.ContactPoliciesSettings, SubscribersList, Entities.ContactPoliciesSettings>(
                    query, (contactPolicies, subscribers) =>
                    {
                        contactPolicies.SubscribersLists ??= new List<SubscribersList>();
                        contactPolicies.SubscribersLists.Add(subscribers);
                        return contactPolicies;
                    }, splitOn: "IdSubscribersList");
            var result = contactPoliciesSettings.GroupBy(ct => ct.IdUser).Select(g =>
            {
                var groupedPost = g.First();
                groupedPost.SubscribersLists = g.Select(ct => ct.SubscribersLists.Single()).ToList();
                return groupedPost;
            });
            return result.SingleOrDefault();
        }
    }
}
