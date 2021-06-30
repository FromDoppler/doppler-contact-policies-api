using Doppler.ContactPolicies.Api.DopplerSecurity;
using Doppler.ContactPolicies.Business.Logic.DTO;
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
            // TODO: replace this validation to get UserId by accountName
            var contactPoliciesSettings = await _contactPoliciesService.GetContactPoliciesSettingsAsync(accountName);

            if (contactPoliciesSettings == null)
                return NotFound($"Account {accountName} does not exist.");

            return new OkObjectResult(contactPoliciesSettings);
        }

        [Authorize(Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpPut("/accounts/{accountName}/settings")]
        public async Task<IActionResult> UpdateContactPoliciesSettings(string accountName,
                    [FromBody] ContactPoliciesSettingsDto contactPoliciesSettings)
        {
            // TODO: replace this validation to get UserId by accountName
            var currentContactPoliciesSetting = await _contactPoliciesService.GetContactPoliciesSettingsAsync(accountName);
            if (currentContactPoliciesSetting == null)
                return NotFound($"Account {accountName} does not exist.");

            await _contactPoliciesService.UpdateContactPoliciesSettingsAsync(accountName, contactPoliciesSettings);
            return Ok();
        }
    }
}
