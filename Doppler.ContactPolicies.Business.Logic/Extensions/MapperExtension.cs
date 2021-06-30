using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Data.Access.Entities;
using System.Collections.Generic;

namespace Doppler.ContactPolicies.Business.Logic.Extensions
{
    public static class MapperExtension
    {
        public static ContactPoliciesSettingsDto ToDto(
            this ContactPoliciesSettings contactPoliciesSettings)
        {
            if (contactPoliciesSettings is null)
                return null;
            return new ContactPoliciesSettingsDto
            {
                AccountName = contactPoliciesSettings.AccountName,
                Active = contactPoliciesSettings.Active,
                EmailsAmountByInterval = contactPoliciesSettings.EmailsAmountByInterval,
                IntervalInDays = contactPoliciesSettings.IntervalInDays,
                ExcludedSubscribersLists = contactPoliciesSettings.ExcludedSubscribersLists is null
                    ? null
                    : new List<ExcludedSubscribersLists>(contactPoliciesSettings.ExcludedSubscribersLists)
            };
        }

        public static ContactPoliciesSettings ToDao(
            this ContactPoliciesSettingsDto contactPoliciesSettings)
        {
            if (contactPoliciesSettings is null)
                return null;
            return new ContactPoliciesSettings
            {
                Active = contactPoliciesSettings.Active,
                EmailsAmountByInterval = contactPoliciesSettings.EmailsAmountByInterval,
                IntervalInDays = contactPoliciesSettings.IntervalInDays,
                ExcludedSubscribersLists = contactPoliciesSettings.ExcludedSubscribersLists is null
                    ? null
                    : new List<ExcludedSubscribersLists>(contactPoliciesSettings.ExcludedSubscribersLists)
            };
        }
    }
}
