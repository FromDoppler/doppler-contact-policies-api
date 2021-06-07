using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;

namespace doppler_contact_policies_api.DopplerSecurity
{
    public class DopplerAuthorizationRequirement: IAuthorizationRequirement
    {
        public bool AllowSuperUser { get; init; }
        public bool AllowOwnResource { get; init; }
    }
}
