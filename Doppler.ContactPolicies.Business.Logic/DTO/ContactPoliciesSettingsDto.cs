using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Doppler.ContactPolicies.Data.Access.Entities;

namespace Doppler.ContactPolicies.Business.Logic.DTO
{
    public class ContactPoliciesSettingsDto
    {
        public string AccountName { get; set; }
        public bool Active { get; set; }
        public int? EmailsAmountByInterval { get; set; }
        public int? IntervalInDays { get; set; }
        public List<ExcludedSubscribersLists> ExcludedSubscribersLists { get; set; }
    }
}
