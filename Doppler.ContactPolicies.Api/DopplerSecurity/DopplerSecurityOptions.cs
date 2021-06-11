using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace Doppler.ContactPolicies.Api.DopplerSecurity
{
    public class DopplerSecurityOptions
    {
        public IEnumerable<SecurityKey> SigningKeys { get; set; } = new SecurityKey[0];
    }
}
