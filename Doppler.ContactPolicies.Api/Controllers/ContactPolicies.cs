using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Api.DopplerSecurity;
using Microsoft.AspNetCore.Authorization;

namespace Doppler.ContactPolicies.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class ContactPolicies : ControllerBase
    {
        private readonly IContactPoliciesService _contactPoliciesService;

        public ContactPolicies(IContactPoliciesService contactPoliciesService)
        {
            _contactPoliciesService = contactPoliciesService;
        }

        [Authorize(Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpGet("/accounts/{accountName}/settings")]
        public async Task<IActionResult> GetContactPoliciesSettings(string accountName)
        {
            var contactPoliciesSettings = await _contactPoliciesService.GetContactPoliciesSettingsAsync(accountName);

            if (contactPoliciesSettings == null)
            {
                return NotFound($"No contact policies settings for account name: {accountName}");
            }

            return new OkObjectResult(contactPoliciesSettings);
        }
    }
}
