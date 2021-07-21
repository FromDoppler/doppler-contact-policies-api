using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.UserApiClient.Services
{
    public class UserFeaturesService : IUserFeaturesService
    {
        private readonly IHttpClientFactory _clientFactory;
        private readonly UserFeaturesServiceSettings _userFeaturesServiceSettings;
        private readonly ILogger<UserFeaturesService> _logger;

        public UserFeaturesService(IHttpClientFactory clientFactory, IOptions<UserFeaturesServiceSettings> userFeaturesServiceSettings, ILogger<UserFeaturesService> logger)
        {
            _clientFactory = clientFactory;
            _userFeaturesServiceSettings = userFeaturesServiceSettings.Value;
            _logger = logger;
        }
        public async Task<bool> GetUserContactPoliciesFeatureAsync(string accountName)
        {
            try
            {
                var baseUri = _userFeaturesServiceSettings.UsersApiUrl;
                var uri = new Uri(baseUri + $"/accounts/{accountName}/features");

                var client = _clientFactory.CreateClient("users-api");
                var responseString = await client.GetStringAsync(uri);

                var features = JsonConvert.DeserializeObject<Features>(responseString);
                return features.ContactPolicies;
            }
            catch (Exception exception)
            {
                _logger.LogError(exception, $"Failed to get contact policies feature with account name {accountName}");
                throw;
            }
        }
    }
}
