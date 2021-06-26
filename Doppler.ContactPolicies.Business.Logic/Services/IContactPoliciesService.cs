using Doppler.ContactPolicies.Business.Logic.DTO;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public interface IContactPoliciesService
    {
        Task<ContactPoliciesSettingsDto> GetContactPoliciesSettingsAsync(string accountName);
        Task<bool> InsertContactPoliciesSettingsAsync(string accountName, ContactPoliciesSettingsDto contactPoliciesSettings);
    }
}
