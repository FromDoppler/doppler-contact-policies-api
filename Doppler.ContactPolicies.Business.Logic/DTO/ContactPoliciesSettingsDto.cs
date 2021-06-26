using Doppler.ContactPolicies.Data.Access.Entities;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Doppler.ContactPolicies.Business.Logic.DTO
{
    public class ContactPoliciesSettingsDto
    {
        public string AccountName { get; set; }
        public bool Active { get; set; }
        [Range(0, 999)]
        [Required(ErrorMessage = "Email amount is required.")]
        public int? EmailsAmountByInterval { get; set; }
        [Range(1, 30)]
        [Required(ErrorMessage = "Interval days are required.")]
        public int? IntervalInDays { get; set; }
        public List<ExcludedSubscribersLists> ExcludedSubscribersLists { get; set; }
    }
}
