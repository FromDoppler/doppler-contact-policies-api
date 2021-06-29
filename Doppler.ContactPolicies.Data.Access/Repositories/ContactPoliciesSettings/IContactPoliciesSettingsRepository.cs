using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings
{
    public interface IContactPoliciesSettingsRepository
    {
        Task<Entities.ContactPoliciesSettings> GetContactPoliciesSettingsAsync(string accountName);

        Task<Entities.ContactPoliciesSettings> GetBasicContactPoliciesSettingsAsync(string accountName);
        Task<bool> UpdateContactPoliciesSettingsAsync(Entities.ContactPoliciesSettings contactPoliciesToInsert);
    }
}
