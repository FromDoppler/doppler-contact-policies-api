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

        public async Task<ContactPoliciesSettingsDto> GetContactPoliciesSettingsAsync(string accountName)
        {
            var contactPoliciesSettings =
                (await _contactPoliciesSettingsRepository.GetContactPoliciesSettingsAsync(accountName)).ToDto();
            return contactPoliciesSettings;
        }

        public async Task UpdateContactPoliciesSettingsAsync(string accountName, ContactPoliciesSettingsDto contactPoliciesSettings)
        {
            var contactPoliciesToUpdate = contactPoliciesSettings.ToDao();
            await _contactPoliciesSettingsRepository.UpdateContactPoliciesSettingsAsync(accountName, contactPoliciesToUpdate);
        }
    }
}
