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

        public async Task<bool> InsertContactPoliciesSettingsAsync(string accountName, ContactPoliciesSettingsDto contactPoliciesSettings)
        {
            var existingUsercontactPolicies = await _contactPoliciesSettingsRepository.GetBasicContactPoliciesSettingsAsync(accountName);

            /// User doesnt exist
            if (existingUsercontactPolicies is null)
                return false;

            /// User doesnt have permissions
            if (existingUsercontactPolicies.IdUser is null)
                throw new Exception($"This action is not allowed for the user with Account {accountName}.");

            var contactPoliciesToInsert = CreateUserContactPoliciesToInsert(contactPoliciesSettings, existingUsercontactPolicies);
            var isSuccsesfulyInserted = await _contactPoliciesSettingsRepository.InsertContactPoliciesSettingsAsync(contactPoliciesToInsert);

            return isSuccsesfulyInserted;
        }

        private ContactPoliciesSettings CreateUserContactPoliciesToInsert(ContactPoliciesSettingsDto contactPoliciesSettings, ContactPoliciesSettings existingUsercontactPolicies)
        {
            return new ContactPoliciesSettings
            {
                AccountName = existingUsercontactPolicies.AccountName,
                IdUser = existingUsercontactPolicies.IdUser,
                Active = contactPoliciesSettings.Active,
                EmailsAmountByInterval = contactPoliciesSettings.EmailsAmountByInterval,
                IntervalInDays = contactPoliciesSettings.IntervalInDays,
                ExcludedSubscribersLists = contactPoliciesSettings.ExcludedSubscribersLists
            };
        }
    }
}
