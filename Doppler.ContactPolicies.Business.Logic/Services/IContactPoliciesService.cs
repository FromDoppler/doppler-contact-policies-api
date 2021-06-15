using System.Threading.Tasks;
using Doppler.ContactPolicies.Business.Logic.DTO;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public interface IContactPoliciesService
    {
        Task<ContactPoliciesSettingsDto> GetContactPoliciesSettingsAsync(string accountName);
    }
}
