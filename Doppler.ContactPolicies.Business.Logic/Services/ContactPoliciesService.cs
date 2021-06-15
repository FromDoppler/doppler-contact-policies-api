using System.Threading.Tasks;
using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Extensions;
using Doppler.ContactPolicies.Data.Access.Entities;
using Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public class ContactPoliciesService : IContactPoliciesService
    {
        private readonly IContactPoliciesSettingsRepository _settingsRepository;

        public ContactPoliciesService(IContactPoliciesSettingsRepository settingsRepository)
        {
            _settingsRepository = settingsRepository;
        }

        public async Task<ContactPoliciesSettingsDto> GetContactPoliciesSettingsAsync(string accountName)
        {
            var contactPoliciesSettings =
                (await _settingsRepository.GetContactPoliciesSettingsAsync(accountName)).ToDto();
            return contactPoliciesSettings;
        }
    }
}
