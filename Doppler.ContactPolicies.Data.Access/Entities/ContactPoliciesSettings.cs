using System.Collections.Generic;

namespace Doppler.ContactPolicies.Data.Access.Entities
{
    public sealed class ContactPoliciesSettings
    {

        public string AccountName { get; set; }
        public bool UserHasContactPolicies { get; set; }
        public bool Active { get; set; }
        public int? EmailsAmountByInterval { get; set; }
        public int? IntervalInDays { get; set; }
        public List<ExcludedSubscribersLists> ExcludedSubscribersLists { get; set; }
    }
}
