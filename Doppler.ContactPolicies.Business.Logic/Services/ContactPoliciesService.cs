using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Extensions;
using Doppler.ContactPolicies.Data.Access.Entities;
using Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public class ContactPoliciesService : IContactPoliciesService
    {
        private readonly IContactPoliciesSettingsRepository _contactPoliciesSettingsRepository;
        private readonly IUserPermissionClientService _userPermissionClientService;

        public ContactPoliciesService(IContactPoliciesSettingsRepository contactPoliciesSettingsRepository, IUserPermissionClientService userPermissionClientService)
        {
            _contactPoliciesSettingsRepository = contactPoliciesSettingsRepository;
            _userPermissionClientService = userPermissionClientService;
        }

        #region public methods
        public async Task<ContactPoliciesSettingsDto> GetContactPoliciesSettingsAsync(string accountName)
        {
            var contactPoliciesSettings =
                (await _contactPoliciesSettingsRepository.GetContactPoliciesSettingsAsync(accountName)).ToDto();
            return contactPoliciesSettings;
        }

        public async Task<bool> InsertContactPoliciesSettings(string accountName,
            ContactPoliciesSettingsDto contactPoliciesSettingsToUpdate)
        {
            var contactPoliciesSettingsToUpdateDao = contactPoliciesSettingsToUpdate.ToDao();

            var idUser = await _contactPoliciesSettingsRepository.GetUserIdAsync(accountName);

            // User doesnt exist
            if (idUser is null)
                return false;

            contactPoliciesSettingsToUpdateDao.IdUser = idUser;
            var userFeature = _userPermissionClientService.GetUserPermissions(accountName);

            ValidateUserContactPoliciesPermission(accountName, userFeature, contactPoliciesSettingsToUpdateDao.ExcludedSubscribersLists);

            await UpdateContactPoliciesAsync(contactPoliciesSettingsToUpdateDao);

            await UpdateExcludedSubscribersListAsync(accountName, userFeature, contactPoliciesSettingsToUpdateDao);
            return true;
        }
        #endregion

        #region private methods
        private void ValidateUserContactPoliciesPermission(string accountName, UserPermission userFeature, List<ExcludedSubscribersLists> excludedSubscribersLists)
        {
            // User doesnt have contact policies feature permissions
            if (!userFeature.Features.contactPolicies)
                throw new Exception($"This action is not allowed for the user with Account {accountName}.");

            // User doesnt have contact policies advenced feature permissions
            if (!userFeature.Features.contactPoliciesAdvanced && excludedSubscribersLists != null)
                throw new Exception($"This action is not allowed for the user with Account {accountName}.");
        }

        private async Task UpdateExcludedSubscribersListAsync(string accountName, UserPermission userFeature, ContactPoliciesSettings contactPoliciesSettingsToUpdate)
        {
            List<ExcludedSubscribersLists> existingExcludedSubscribersListForUser = await _contactPoliciesSettingsRepository.VerifyUserSubscriberListExist(
                contactPoliciesSettingsToUpdate.IdUser,
                contactPoliciesSettingsToUpdate.ExcludedSubscribersLists);

            if (existingExcludedSubscribersListForUser is null)
                throw new Exception($"This action is not allowed for the user with Account {accountName}.");

            var excludedSubscribersIdListToInsertAndDelete = VerifyExclusionListToBeUpdateOrDelete(existingExcludedSubscribersListForUser,
                contactPoliciesSettingsToUpdate.ExcludedSubscribersLists);

            await _contactPoliciesSettingsRepository.UpdateExludedSubscribersListAsync(contactPoliciesSettingsToUpdate.IdUser,
                excludedSubscribersIdListToInsertAndDelete.Item1,
                excludedSubscribersIdListToInsertAndDelete.Item2);
        }

        private async Task UpdateContactPoliciesAsync(ContactPoliciesSettings contactPoliciesSettingsToUpdate)
        {
            await _contactPoliciesSettingsRepository.UpdateContactPoliciesSettingsAsync(contactPoliciesSettingsToUpdate);
        }

        private Tuple<List<int>, List<int>> VerifyExclusionListToBeUpdateOrDelete(List<ExcludedSubscribersLists> excludedListFromDataBase, List<ExcludedSubscribersLists> newExcludedList)
        {
            Tuple<List<int>, List<int>> processedlists = new Tuple<List<int>, List<int>>(new List<int>(), new List<int>());

            processedlists.Item1.AddRange(newExcludedList.Select(f => f.Id).Except(excludedListFromDataBase.Select(b => b.Id))); // for insert
            processedlists.Item2.AddRange(excludedListFromDataBase.Select(f => f.Id).Except(newExcludedList.Select(b => b.Id))); // for delete
            return processedlists;
        }
        #endregion
    }
}
