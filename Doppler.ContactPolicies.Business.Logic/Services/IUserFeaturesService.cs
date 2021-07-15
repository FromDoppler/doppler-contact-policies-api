using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public interface IUserFeaturesService
    {
        Task<bool> GetUserContactPoliciesFeature(string accountName);
    }
}
