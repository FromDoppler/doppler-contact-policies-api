using AutoFixture;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Business.Logic.UserApiClient.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.ContactPolicies.Api.Test
{
    public class UpdateContactPoliciesSettingsTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518 =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOjEyMywidW5pcXVlX25hbWUiOiJ0ZXN0MUB0ZXN0LmNvbSIsInJvbGUiOiJVU0VSIiwiZXhwIjoyMDAwMDAwMDAwfQ.E3RHjKx9p0a-64RN2YPtlEMysGM45QBO9eATLBhtP4tUQNZnkraUr56hAWA-FuGmhiuMptnKNk_dU3VnbyL6SbHrMWUbquxWjyoqsd7stFs1K_nW6XIzsTjh8Bg6hB5hmsSV-M5_hPS24JwJaCdMQeWrh6cIEp2Sjft7I1V4HQrgzrkMh15sDFAw3i1_ZZasQsDYKyYbO9Jp7lx42ognPrz_KuvPzLjEXvBBNTFsVXUE-ur5adLNMvt-uXzcJ1rcwhjHWItUf5YvgRQbbBnd9f-LsJIhfkDgCJcvZmGDZrtlCKaU1UjHv5c3faZED-cjL59MbibofhPjv87MK8hhdg";

        private const string CONTACT_POLICIES_SETTINGS_REQUEST_BODY_STUB =
            "{\"accountName\":\"test1@test.com\",\"active\":true,\"emailsAmountByInterval\":5,\"intervalInDays\":10,\"excludedSubscribersLists\":[]}";

        private readonly WebApplicationFactory<Startup> _factory;

        public UpdateContactPoliciesSettingsTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Fact]
        public async Task
            UpdateContactPoliciesSettings_Should_ReturnNotFound_When_UserWithSameAccountNameIsNotFound()
        {
            // Arrange
            const string contactPoliciesRequestBodyStub = CONTACT_POLICIES_SETTINGS_REQUEST_BODY_STUB;
            const string validAccountName = "test1@test.com";
            const string token = TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518;

            int? notFoundedIdUser = null;
            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            contactPoliciesMock.Setup(x => x.GetIdUserByAccountName(validAccountName)).ReturnsAsync(notFoundedIdUser);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
            })).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Put, $"/accounts/{validAccountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } },
                Content = new StringContent(contactPoliciesRequestBodyStub, Encoding.UTF8, "application/json")
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.NotFound, response.StatusCode);
        }

        [Fact]
        public async Task
            UpdateContactPoliciesSettings_Should_ReturnOK_When_UserWithSameAccountNameIsFoundAndHasContactPoliciesFeature()
        {
            // Arrange
            const string contactPoliciesRequestBodyStub = CONTACT_POLICIES_SETTINGS_REQUEST_BODY_STUB;
            const string validAccountName = "test1@test.com";
            const string token = TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518;

            var fixture = new Fixture();
            var foundedIdUser = fixture.Create<int>();

            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            var userFeaturesMock = new Mock<IUserFeaturesService>();
            contactPoliciesMock.Setup(x => x.GetIdUserByAccountName(validAccountName)).ReturnsAsync(foundedIdUser);
            userFeaturesMock.Setup(x => x.HasContactPoliciesFeatureAsync(validAccountName)).ReturnsAsync(true);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
                services.AddSingleton(userFeaturesMock.Object);
            })).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Put, $"/accounts/{validAccountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } },
                Content = new StringContent(contactPoliciesRequestBodyStub, Encoding.UTF8, "application/json")
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
        }

        [Fact]
        public async Task
            UpdateContactPoliciesSettings_Should_ReturnForbidden_When_UserWithSameAccountNameIsFoundAndContactPoliciesFeatureIsFalse()
        {
            // Arrange
            const string contactPoliciesRequestBodyStub = CONTACT_POLICIES_SETTINGS_REQUEST_BODY_STUB;
            const string validAccountName = "test1@test.com";
            const string token = TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518;

            var fixture = new Fixture();
            var foundedIdUser = fixture.Create<int>();

            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            var userFeaturesMock = new Mock<IUserFeaturesService>();
            contactPoliciesMock.Setup(x => x.GetIdUserByAccountName(validAccountName))
            .ReturnsAsync(foundedIdUser);
            userFeaturesMock.Setup(x => x.HasContactPoliciesFeatureAsync(validAccountName)).ReturnsAsync(false);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
                services.AddSingleton(userFeaturesMock.Object);
            })).CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"/accounts/{validAccountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } },
                Content = new StringContent(contactPoliciesRequestBodyStub, Encoding.UTF8, "application/json")
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(HttpStatusCode.Forbidden, response.StatusCode);
        }
    }
}
