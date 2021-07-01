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
            var idUser = await _contactPoliciesService.GetIdUserByAccountName(accountName);
            if (idUser == null)
                return NotFound($"Account {accountName} does not exist.");

            var contactPoliciesSettings = await _contactPoliciesService.GetContactPoliciesSettingsAsync(idUser.Value);

            return new OkObjectResult(contactPoliciesSettings);
        }

        [Authorize(Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpPut("/accounts/{accountName}/settings")]
        public async Task<IActionResult> UpdateContactPoliciesSettings(string accountName,
                            [FromBody] ContactPoliciesSettingsDto contactPoliciesSettings)
        {
            var idUser = await _contactPoliciesService.GetIdUserByAccountName(accountName);
            if (idUser == null)
                return NotFound($"Account {accountName} does not exist.");

            await _contactPoliciesService.UpdateContactPoliciesSettingsAsync(idUser.Value, contactPoliciesSettings);
            return Ok();
        }
    }
}
