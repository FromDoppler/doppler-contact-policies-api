using Doppler.ContactPolicies.Business.Logic.DTO;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.UserApiClient.Services
{
    public class UserFeaturesService : IUserFeaturesService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly IConfiguration _configuration;
        private readonly ILogger<UserFeaturesService> _logger;

        public UserFeaturesService(IHttpClientFactory clientFactory, IConfiguration configuration, ILogger<UserFeaturesService> logger)
        {
            _clientFactory = clientFactory;
            _configuration = configuration;
            _logger = logger;
        }
        public async Task<bool> GetUserContactPoliciesFeatureAsync(string accountName)
        {
            try
            {
                var baseUri = "https://apisint.fromdoppler.net/doppler-users-api";
                var uri = new Uri(baseUri + $"/accounts/{accountName}/features");

                var client = _clientFactory.CreateClient("users-api");
                var responseString = await client.GetStringAsync(uri);

                var features = JsonConvert.DeserializeObject<Features>(responseString);
                return features.ContactPolicies;
            }
            catch (HttpRequestException httpRequestException)
            {
                _logger.LogError(httpRequestException.Message, "An error occurred.");
                throw;
            }
        }
    }
}
