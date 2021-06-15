using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Data.Access.Entities;

namespace Doppler.ContactPolicies.Business.Logic.Extensions
{
    public static class MapperExtension
    {
        public static ContactPoliciesSettingsDto ToDto(
            this ContactPoliciesSettings contactPoliciesSettings)
        {
            if (contactPoliciesSettings is null)
                return null;
            return new()
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
    }
}
