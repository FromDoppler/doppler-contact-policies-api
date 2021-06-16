using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Data.Access.Entities
{
    public sealed class ContactPoliciesSettings
    {
        public string AccountName { get; set; }
        public int? IdUser { get; set; }
        public bool EnabledExcludedSubscribersList { get; set; }
        public bool Active { get; set; }
        public int? EmailsAmountByInterval { get; set; }
        public int? IntervalInDays { get; set; }
        public List<ExcludedSubscribersLists> ExcludedSubscribersLists { get; set; }
    }
}
