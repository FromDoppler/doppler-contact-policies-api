using Doppler.ContactPolicies.Business.Logic.DTO;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.Services
{
    public class UserFeaturesService : IUserFeaturesService
    {
        private readonly HttpClient _httpClient;
        private readonly IHttpContextAccessor _httpContextAccessor;
        private readonly IConfiguration _configuration;

        public UserFeaturesService(HttpClient httpClient, IHttpContextAccessor httpContextAccessor, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _httpContextAccessor = httpContextAccessor;
            _configuration = configuration;
        }
        public async Task<bool> GetUserContactPoliciesFeature(string accountName)
        {
            try
            {
                var baseUri = _configuration.GetSection("UsersApiURL").Value;
                var uri = new Uri(baseUri + $"/accounts/{accountName}/features");
                // remove Bearer word from result
                var headers = _httpContextAccessor.HttpContext.Request.Headers["Authorization"].ToString().Replace("Bearer ", string.Empty);

                _httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", headers);
                var responseString = await _httpClient.GetStringAsync(uri);

                var features = JsonConvert.DeserializeObject<Features>(responseString);
                return features.ContactPolicies;
            }
            catch (HttpRequestException httpRequestException)
            {
                return false;
            }
        }
    }
}
