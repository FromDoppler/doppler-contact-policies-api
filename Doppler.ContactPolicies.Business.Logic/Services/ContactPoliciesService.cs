using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Extensions;
using Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public class ContactPoliciesService : IContactPoliciesService
    {
        private readonly IContactPoliciesSettingsRepository _contactPoliciesSettingsRepository;

        public ContactPoliciesService(IContactPoliciesSettingsRepository contactPoliciesSettingsRepository)
        {
            _contactPoliciesSettingsRepository = contactPoliciesSettingsRepository;
        }

        public async Task<ContactPoliciesSettingsDto> GetContactPoliciesSettingsByIdUserAsync(int idUser)
        {
            var contactPoliciesSettings =
                (await _contactPoliciesSettingsRepository.GetContactPoliciesSettingsByIdUserAsync(idUser)).ToDto();
            return contactPoliciesSettings;
        }

        public async Task UpdateContactPoliciesSettingsAsync(int idUser, ContactPoliciesSettingsDto contactPoliciesSettings)
        {
            var contactPoliciesToUpdate = contactPoliciesSettings.ToDao();
            await _contactPoliciesSettingsRepository.UpdateContactPoliciesSettingsAsync(idUser, contactPoliciesToUpdate);
        }

        public async Task<int?> GetIdUserByAccountName(string accountName)
        {
            return await _contactPoliciesSettingsRepository.GetIdUserByAccountName(accountName);
        }
    }
}
