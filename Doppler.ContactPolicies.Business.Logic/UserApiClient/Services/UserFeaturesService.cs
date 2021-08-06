using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Doppler.ContactPolicies.Business.Logic.UserApiClient.Services
{
    public class UserFeaturesService : IUserFeaturesService
    {
        private readonly IUsersApiTokenGetter _usersApiTokenGetter;
        private readonly UserFeaturesServiceSettings _userFeaturesServiceSettings;
        private readonly ILogger<UserFeaturesService> _logger;

        private static readonly HttpClient client = new HttpClient();

        public UserFeaturesService(IUsersApiTokenGetter usersApiTokenGetter, IOptions<UserFeaturesServiceSettings> userFeaturesServiceSettings, ILogger<UserFeaturesService> logger)
        {
            _usersApiTokenGetter = usersApiTokenGetter;
            _userFeaturesServiceSettings = userFeaturesServiceSettings.Value;
            _logger = logger;
        }
        public async Task<bool> HasContactPoliciesFeatureAsync(string accountName)
        {
            try
            {
                var baseUri = _userFeaturesServiceSettings.UsersApiURL;
                var uri = new Uri(baseUri + $"/accounts/{accountName}/features");

                var usersApiToken = await _usersApiTokenGetter.GetTokenAsync();

                var request = new HttpRequestMessage(HttpMethod.Get, uri)
                {
                    Headers = { { "Authorization", $"Bearer {usersApiToken}" } }
                };

                var response = await client.SendAsync(request);

                var features = await response.Content.ReadFromJsonAsync<Features>();

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
