namespace Doppler.ContactPolicies.Data.Access.Entities
{
    public sealed class ContactPoliciesTimeRestriction
    {

        public string AccountName { get; set; }
        public bool TimeSlotEnabled { get; set; }
        public int? HourFrom { get; set; }
        public int? HourTo { get; set; }
        public bool WeekdaysEnabled { get; set; }
    }
}
