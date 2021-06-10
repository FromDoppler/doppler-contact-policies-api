using System.Collections.Generic;
using Microsoft.IdentityModel.Tokens;

namespace doppler_contact_policies_api.DopplerSecurity
{
    public class DopplerSecurityOptions
    {
        public IEnumerable<SecurityKey> SigningKeys { get; set; } = new SecurityKey[0];
    }
}
