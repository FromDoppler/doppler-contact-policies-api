using Doppler.ContactPolicies.Data.Access.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings
{
    public interface IContactPoliciesSettingsRepository
    {
        Task<Entities.ContactPoliciesSettings> GetContactPoliciesSettingsAsync(string accountName);

        Task<int?> GetUserIdAsync(string accountName);

        Task UpdateContactPoliciesSettingsAsync(Entities.ContactPoliciesSettings contactPoliciesSettings);

        Task<List<ExcludedSubscribersLists>> VerifyUserSubscriberListExist(int? idUser,
    List<ExcludedSubscribersLists> excludedSubscribersLists);

        Task UpdateExludedSubscribersListAsync(int? IdUser, List<int> excludedSubscribersToInsert,
            List<int> excludedSubscribersToDelete);
    }
}
