using Doppler.ContactPolicies.Data.Access.Entities;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public interface IUserPermissionClientService
    {
        public UserPermission GetUserPermissions(string accountName);
    }
}
