using Doppler.ContactPolicies.Data.Access.Entities;
using System.Collections.Generic;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public class UserPermissionClientService : IUserPermissionClientService
    {
        List<UserPermission> users;
        public UserPermissionClientService()
        {
            users = new List<UserPermission>();
            CreateUsersPermision();
        }
        public UserPermission GetUserPermissions(string accountName)
        {
            return users.Find(x => x.AccountName == accountName);
        }

        private void CreateUsersPermision()
        {
            users.Add(new UserPermission()
            {
                AccountName = "prueba@makingsense.com",
                Features = new Features()
                {
                    contactPolicies = true,
                    contactPoliciesAdvanced = true
                }
            });
            users.Add(new UserPermission()
            {
                AccountName = "juan_test@makingsense.com",
                Features = new Features()
                {
                    contactPolicies = true,
                    contactPoliciesAdvanced = true
                }
            });
            users.Add(new UserPermission()
            {
                AccountName = "paolo_test@makingsense.com",
                Features = new Features()
                {
                    contactPolicies = true,
                    contactPoliciesAdvanced = false
                }
            });
            users.Add(new UserPermission()
            {
                AccountName = "nopermission@makingsense.com",
                Features = new Features()
                {
                    contactPolicies = false,
                    contactPoliciesAdvanced = false
                }
            });

        }
    }
}
