using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using doppler_contact_policies_api.DopplerSecurity;
using Microsoft.AspNetCore.Authorization;

namespace doppler_contact_policies_api.Controllers
{
    [Authorize]
    [ApiController]
    public class ContactPolicies : ControllerBase
    {
        [Authorize(Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpGet("/accounts/{accountId:long:min(1)}/contact-policies")]
        public string GetContactPolicies(long accountId)
        {
            return $"Hello! \"you\" that have access to the account with ID '{accountId}'";
        }

        [Authorize(Policies.OWN_RESOURCE_OR_SUPERUSER)]
        [HttpGet("/accounts/{accountName}/contact-policies")]
        public string GetContactPolicies(string accountName)
        {
            return $"Hello! \"you\" that have access to the account with accountname '{accountName}'";
        }
    }
}
