using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Doppler.ContactPolicies.Api.DopplerSecurity;
using Microsoft.AspNetCore.Authorization;

namespace Doppler.ContactPolicies.Api.Controllers
{
    [Authorize]
    [ApiController]
    public class ContactPolicies : ControllerBase
    {
        [Authorize(DopplerSecurity.Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpGet("/accounts/{accountName}/settings")]
        public string GetContactPoliciesSettings(string accountName)
        {
            return $"Hello! \"you\" that have access to the account with accountName '{accountName}'";
        }
    }
}
