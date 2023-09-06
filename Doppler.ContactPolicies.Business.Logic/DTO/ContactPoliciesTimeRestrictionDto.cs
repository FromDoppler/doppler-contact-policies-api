using System;
using System.ComponentModel.DataAnnotations;

namespace Doppler.ContactPolicies.Business.Logic.DTO
{
    public class ContactPoliciesTimeRestrictionDto
    {
        public bool TimeSlotEnabled { get; set; }

        [Range(0, 23)]
        [Required(ErrorMessage = "HourFrom is required.")]
        public int? HourFrom { get; set; }

        [Range(0, 23)]
        [Required(ErrorMessage = "HourFrom is required.")]
        public int? HourTo { get; set; }

        public bool WeekdaysEnabled { get; set; }
    }
}
