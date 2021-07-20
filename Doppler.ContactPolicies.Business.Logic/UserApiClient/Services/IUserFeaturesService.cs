using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.UserApiClient.Services
{
    public interface IUserFeaturesService
    {
        Task<bool> GetUserContactPoliciesFeatureAsync(string accountName);
    }
}
