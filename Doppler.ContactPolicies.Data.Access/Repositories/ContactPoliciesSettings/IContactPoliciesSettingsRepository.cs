using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings
{
    public interface IContactPoliciesSettingsRepository
    {
        Task<Entities.ContactPoliciesSettings> GetContactPoliciesSettingsAsync(string accountName);
        Task UpdateContactPoliciesSettingsAsync(string accountName, Entities.ContactPoliciesSettings contactPoliciesToInsert);
        Task<int?> GetUserIdByAccountName(string accountName);
    }
}
