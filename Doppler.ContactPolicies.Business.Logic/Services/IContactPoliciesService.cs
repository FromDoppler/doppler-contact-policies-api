using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doppler.ContactPolicies.Data.Access.Entities;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public interface IContactPoliciesService
    {
        Task<ContactPoliciesSettings> GetContactPoliciesSettingsAsync(string accountName);
    }
}
