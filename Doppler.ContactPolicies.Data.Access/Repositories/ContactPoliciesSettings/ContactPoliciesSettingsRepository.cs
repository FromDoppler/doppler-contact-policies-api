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

        public async Task<Entities.ContactPoliciesSettings> GetContactPoliciesSettingsAsync(string accountName)
        {
            using var connection = await _databaseConnectionFactory.GetConnection();
            var query = @"select cp.IdUser, cp.Enabled,cp.Active,cp.Interval,cp.Amount,u.IdUser,Email,sl.IdSubscribersList, sl.Name
                from [User] u
                inner join [UserShippingLimit] cp on u.IdUser=cp.IdUser 
                inner join [SubscribersListXShippingLimit] sls on sls.IdUser = cp.IdUser
                inner join [SubscribersList] sl on sl.IdSubscribersList = sls.IdSubscribersList where u.Email=@Email";

            var queryParams = new {Email = accountName};

            var contactPoliciesSettings = await connection
                .QueryAsync<Entities.ContactPoliciesSettings, User, SubscribersList, Entities.ContactPoliciesSettings>(
                    query, (contactPolicies, user, subscribers) =>
                    {
                        contactPolicies.User = user;
                        contactPolicies.SubscribersLists ??= new List<SubscribersList>();
                        contactPolicies.SubscribersLists.Add(subscribers);
                        return contactPolicies;
                    }, queryParams, splitOn: "IdUser,IdSubscribersList");

            Entities.ContactPoliciesSettings result = null;
            var contactPoliciesSettingsEnumerable = contactPoliciesSettings.ToList();
            if (contactPoliciesSettingsEnumerable.Any())
            {
                result = contactPoliciesSettingsEnumerable.GroupBy(ct => ct.User.IdUser).Select(g =>
                {
                    var groupedPost = g.First();
                    groupedPost.SubscribersLists = g.Select(ct => ct.SubscribersLists.Single()).ToList();
                    return groupedPost;
                }).First();
            }

            return result;
        }
    }
}
