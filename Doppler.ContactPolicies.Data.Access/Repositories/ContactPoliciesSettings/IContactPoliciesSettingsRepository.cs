using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doppler.ContactPolicies.Data.Access.Entities;

namespace Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings
{
    public interface IContactPoliciesSettingsRepository
    {
        Task<Entities.ContactPoliciesSettings> GetContactPoliciesSettingsAsync(string accountName);
    }
}
