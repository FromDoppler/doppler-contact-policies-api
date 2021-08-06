using Doppler.ContactPolicies.Business.Logic.UserApiClient.Services;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Api.UsersApi
{
    public class UsersApiTokenGetter : IUsersApiTokenGetter
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public UsersApiTokenGetter(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public async Task<string> GetTokenAsync()
        {
            return await _httpContextAccessor.HttpContext.GetTokenAsync("Bearer", "access_token");
        }
    }
}
