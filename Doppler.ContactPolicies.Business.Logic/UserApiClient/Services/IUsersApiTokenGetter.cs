using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.UserApiClient.Services
{
    public interface IUsersApiTokenGetter
    {
        Task<string> GetTokenAsync();
    }
}
