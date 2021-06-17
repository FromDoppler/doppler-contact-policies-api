using Doppler.ContactPolicies.Api.DopplerSecurity;
using Doppler.ContactPolicies.Business.Logic.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

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
                return NotFound($"Account {accountName} does not exist.");
            }

            return new OkObjectResult(contactPoliciesSettings);
        }
    }
}
