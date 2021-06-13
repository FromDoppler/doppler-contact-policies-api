using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public async Task<ContactPoliciesSettings> GetContactPoliciesSettingsAsync(string accountName)
        {
            var contactPoliciesSettings = await _settingsRepository.GetContactPoliciesSettingsAsync(accountName);
            return contactPoliciesSettings;
        }
    }
}
