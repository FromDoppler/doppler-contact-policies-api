using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace doppler_contact_policies_api.DopplerSecurity
{
    public static class Policies
    {
        public const string ONLY_SUPERUSER = nameof(ONLY_SUPERUSER);
        public const string OWN_RESOURCE_OR_SUPERUSER = nameof(OWN_RESOURCE_OR_SUPERUSER);
    }
}
