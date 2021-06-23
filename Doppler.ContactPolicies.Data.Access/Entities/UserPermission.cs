namespace Doppler.ContactPolicies.Data.Access.Entities
{
    public class UserPermission
    {
        public string AccountName { get; set; }
        public Features Features { get; set; }
    }

    public class Features
    {
        public bool contactPolicies { get; set; }
        public bool contactPoliciesAdvanced { get; set; }
    }
}
