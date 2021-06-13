using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Data.Access.Entities
{
    public sealed class ContactPoliciesSettings
    {

        public User User { get; set; }
        public bool Enabled { get; set; }
        public bool Active { get; set; }
        public int Amount { get; set; }
        public int Interval { get; set; }
        public List<SubscribersList> SubscribersLists { get; set; }
    }
}
