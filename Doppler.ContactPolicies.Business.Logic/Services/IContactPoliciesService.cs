using Doppler.ContactPolicies.Business.Logic.DTO;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public interface IContactPoliciesService
    {
        Task<ContactPoliciesSettingsDto> GetContactPoliciesSettingsAsync(int idUser);
        Task UpdateContactPoliciesSettingsAsync(int idUser, ContactPoliciesSettingsDto contactPoliciesSettings);
        Task<int?> GetIdUserByAccountName(string accountName);
    }
}
