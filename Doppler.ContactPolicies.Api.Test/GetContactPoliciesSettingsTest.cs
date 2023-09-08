using AutoFixture;
using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Business.Logic.UserApiClient.Services;
using Doppler.ContactPolicies.Data.Access.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.ContactPolicies.Api.Test
{
    public class GetContactPoliciesSettingsTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        private const string TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518 =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOjEyMywidW5pcXVlX25hbWUiOiJ0ZXN0MUB0ZXN0LmNvbSIsInJvbGUiOiJVU0VSIiwiZXhwIjoyMDAwMDAwMDAwfQ.E3RHjKx9p0a-64RN2YPtlEMysGM45QBO9eATLBhtP4tUQNZnkraUr56hAWA-FuGmhiuMptnKNk_dU3VnbyL6SbHrMWUbquxWjyoqsd7stFs1K_nW6XIzsTjh8Bg6hB5hmsSV-M5_hPS24JwJaCdMQeWrh6cIEp2Sjft7I1V4HQrgzrkMh15sDFAw3i1_ZZasQsDYKyYbO9Jp7lx42ognPrz_KuvPzLjEXvBBNTFsVXUE-ur5adLNMvt-uXzcJ1rcwhjHWItUf5YvgRQbbBnd9f-LsJIhfkDgCJcvZmGDZrtlCKaU1UjHv5c3faZED-cjL59MbibofhPjv87MK8hhdg";

        private readonly WebApplicationFactory<Startup> _factory;

        public GetContactPoliciesSettingsTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("test1@test.com", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518,
            HttpStatusCode.OK)]
        public async Task
            GetContactPoliciesSettings_Should_ReturnOKWithContactPoliciesResponse_When_UserAccountNameIsFoundAndContactPoliciesAreActivated(
                string accountName,
                string token, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var fixture = new Fixture();
            var expectedIdUser = fixture.Create<int>();

            var expectedContactPoliciesDto =
                SetUpExpectedContactPoliciesSetting(accountName, out var expectedResultAsString, true);

            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            contactPoliciesMock.Setup(x => x.GetIdUserByAccountName(accountName)).ReturnsAsync(expectedIdUser);
            contactPoliciesMock.Setup(x => x.GetContactPoliciesSettingsByIdUserAsync(expectedIdUser)).ReturnsAsync(expectedContactPoliciesDto);

            var userFeaturesMock = new Mock<IUserFeaturesService>();
            userFeaturesMock.Setup(x => x.HasContactPoliciesFeatureAsync(accountName)).ReturnsAsync(true);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
                services.AddSingleton(userFeaturesMock.Object);
            })).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/accounts/{accountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } }
            };

            // Act
            var response = await client.SendAsync(request);
            var contentAsString = await response.Content.ReadAsStringAsync();

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Contains(expectedResultAsString, contentAsString);
        }

        [Theory]
        [InlineData("test1@test.com", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518,
            HttpStatusCode.NotFound)]
        public async Task
            GetContactPoliciesSettings_Should_ReturnNotFound_When_UserWithSameAccountNameIsNotFound(
                string accountName, string token, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            int? expectedNotFoundedIdUser = null;

            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            contactPoliciesMock.Setup(x => x.GetIdUserByAccountName(accountName)).ReturnsAsync(expectedNotFoundedIdUser);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
            })).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/accounts/{accountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } }
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Theory]
        [InlineData("test1@test.com", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518,
            HttpStatusCode.OK)]
        public async Task
            GetContactPoliciesSettings_Should_ReturnOKWithContactPoliciesSettings_When_UserWithSameAccountNameIsFoundAndContactPoliciesIsNotActivated(
                string accountName, string token, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var fixture = new Fixture();
            var expectedIdUser = fixture.Create<int>();
            var expectedContactPoliciesDto =
                SetUpExpectedContactPoliciesSetting(accountName, out var expectedResultAsString, false);

            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            contactPoliciesMock.Setup(x => x.GetIdUserByAccountName(accountName)).ReturnsAsync(expectedIdUser);
            contactPoliciesMock.Setup(x => x.GetContactPoliciesSettingsByIdUserAsync(expectedIdUser))
                .ReturnsAsync(expectedContactPoliciesDto);

            var userFeaturesMock = new Mock<IUserFeaturesService>();
            userFeaturesMock.Setup(x => x.HasContactPoliciesFeatureAsync(accountName)).ReturnsAsync(true);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
                services.AddSingleton(userFeaturesMock.Object);
            })).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/accounts/{accountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } }
            };

            // Act
            var response = await client.SendAsync(request);
            var contentAsString = await response.Content.ReadAsStringAsync();


            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Contains(expectedResultAsString, contentAsString);
        }

        [Theory]
        [InlineData("test1@test.com", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518,
            HttpStatusCode.OK)]
        public async Task
            GetContactPoliciesSettings_Should_ReturnOKWithExcludedSubscribersListsAsNull_When_UserWithSameAccountNameIsFoundAndContactPoliciesIsNotEnabled(
                string accountName, string token, HttpStatusCode expectedStatusCode)
        {
            var fixture = new Fixture();
            var expectedIdUser = fixture.Create<int>();
            var expectedContactPoliciesDto = new ContactPoliciesSettingsDto()
            {
                AccountName = accountName,
                Active = false,
                EmailsAmountByInterval = null,
                IntervalInDays = null,
                ExcludedSubscribersLists = null
            };
            const string expectedResultAsString =
                "accountName\":\"test1@test.com\",\"active\":false,\"emailsAmountByInterval\":null,\"intervalInDays\":null,\"excludedSubscribersLists\":null,\"timeRestriction\":null}";

            var contactPoliciesServiceMock = new Mock<IContactPoliciesService>();
            contactPoliciesServiceMock.Setup(x => x.GetIdUserByAccountName(accountName)).ReturnsAsync(expectedIdUser);
            contactPoliciesServiceMock.Setup(x => x.GetContactPoliciesSettingsByIdUserAsync(expectedIdUser))
                .ReturnsAsync(expectedContactPoliciesDto).Verifiable();

            var userFeaturesMock = new Mock<IUserFeaturesService>();
            userFeaturesMock.Setup(x => x.HasContactPoliciesFeatureAsync(accountName)).ReturnsAsync(true);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesServiceMock.Object);
                services.AddSingleton(userFeaturesMock.Object);
            })).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/accounts/{accountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } }
            };

            // Act
            var response = await client.SendAsync(request);
            var contentAsString = await response.Content.ReadAsStringAsync();


            // Assert
            contactPoliciesServiceMock.Verify();
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Contains(expectedResultAsString, contentAsString);
        }

        [Theory]
        [InlineData("test1@test.com", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518,
            HttpStatusCode.Forbidden)]
        public async Task
            GetContactPoliciesSettings_Should_ReturnForbidden_When_UserHasNotContactPoliciesFeature(
                string accountName, string token, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var fixture = new Fixture();

            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            contactPoliciesMock.Setup(x => x.GetIdUserByAccountName(accountName)).ReturnsAsync(fixture.Create<int>());

            var userFeaturesMock = new Mock<IUserFeaturesService>();
            userFeaturesMock.Setup(x => x.HasContactPoliciesFeatureAsync(accountName)).ReturnsAsync(false);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
                services.AddSingleton(userFeaturesMock.Object);
            })).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/accounts/{accountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } }
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        private ContactPoliciesSettingsDto SetUpExpectedContactPoliciesSetting(string accountName,
            out string expectedResultAsString, bool isActive)
        {
            var expectedContactPoliciesSetting = new ContactPoliciesSettingsDto()
            {
                Active = isActive,
                AccountName = accountName,
                EmailsAmountByInterval = 100,
                IntervalInDays = 10,
                ExcludedSubscribersLists = new List<ExcludedSubscribersLists>()
                {
                    new ExcludedSubscribersLists()
                    {
                        Id = 34, Name = "Listado_Marketing"
                    },
                    new ExcludedSubscribersLists()
                    {
                        Id = 169, Name = "Testing_ExcludeList"
                    }
                }
            };
            expectedResultAsString =
                $"{{\"accountName\":\"test1@test.com\",\"active\":{isActive.ToString().ToLower()},\"emailsAmountByInterval\":100,\"intervalInDays\":10,\"excludedSubscribersLists\":[{{\"id\":34,\"name\":\"Listado_Marketing\"}},{{\"id\":169,\"name\":\"Testing_ExcludeList\"}}],\"timeRestriction\":null}}";
            return expectedContactPoliciesSetting;
        }
    }
}
