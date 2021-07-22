using Doppler.ContactPolicies.Api.DopplerSecurity;
using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Business.Logic.UserApiClient.Services;
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
        private readonly IUserFeaturesService _userFeaturesService;

        public ContactPolicies(IContactPoliciesService contactPoliciesService, IUserFeaturesService userFeaturesService)
        {
            _contactPoliciesService = contactPoliciesService;
            _userFeaturesService = userFeaturesService;
        }

        [Authorize(Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpGet("/accounts/{accountName}/settings")]
        public async Task<IActionResult> GetContactPoliciesSettings(string accountName)
        {
            var idUser = await _contactPoliciesService.GetIdUserByAccountName(accountName);
            if (idUser == null)
                return NotFound($"Account {accountName} does not exist.");

            var contactPoliciesSettings = await _contactPoliciesService.GetContactPoliciesSettingsByIdUserAsync(idUser.Value);

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

            var userHasContactPoliciesFeature = await _userFeaturesService.HasContactPoliciesFeatureAsync(accountName);
            if (!userHasContactPoliciesFeature)
            {
                return new ObjectResult($"This action is not allowed for the user with Account {accountName}.") { StatusCode = 403 };
            }

            await _contactPoliciesService.UpdateContactPoliciesSettingsAsync(idUser.Value, contactPoliciesSettings);
            return Ok();
        }
    }
}
