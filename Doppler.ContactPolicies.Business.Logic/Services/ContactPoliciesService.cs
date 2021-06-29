using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Extensions;
using Doppler.ContactPolicies.Data.Access.Entities;
using Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings;
using System;
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

        public async Task<bool> UpdateContactPoliciesSettingsAsync(string accountName, ContactPoliciesSettingsDto contactPoliciesSettings)
        {
            var existingUserContactPolicies = await _contactPoliciesSettingsRepository.GetBasicContactPoliciesSettingsAsync(accountName);

            // User doesnt exist
            if (existingUserContactPolicies is null)
                return false;

            // User doesnt have permissions
            if (existingUserContactPolicies.IdUser is null)
                throw new Exception($"This action is not allowed for the user with Account {accountName}.");

            var contactPoliciesToInsert = CreateUserContactPoliciesToUpdate(contactPoliciesSettings, existingUserContactPolicies);
            var isSuccessfullyInserted = await _contactPoliciesSettingsRepository.UpdateContactPoliciesSettingsAsync(contactPoliciesToInsert);

            return isSuccessfullyInserted;
        }

        private ContactPoliciesSettings CreateUserContactPoliciesToUpdate(ContactPoliciesSettingsDto contactPoliciesSettings, ContactPoliciesSettings existingUserContactPolicies)
        {
            return new ContactPoliciesSettings
            {
                AccountName = existingUserContactPolicies.AccountName,
                IdUser = existingUserContactPolicies.IdUser,
                Active = contactPoliciesSettings.Active,
                EmailsAmountByInterval = contactPoliciesSettings.EmailsAmountByInterval,
                IntervalInDays = contactPoliciesSettings.IntervalInDays,
                ExcludedSubscribersLists = contactPoliciesSettings.ExcludedSubscribersLists
            };
        }
    }
}
