using AutoFixture;
using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Extensions;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Data.Access.Entities;
using Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.ContactPolicies.Api.Test
{
    public class UpdateContactPoliciesSettingsTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        const string TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518 =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOjEyMywidW5pcXVlX25hbWUiOiJ0ZXN0MUB0ZXN0LmNvbSIsInJvbGUiOiJVU0VSIiwiZXhwIjoyMDAwMDAwMDAwfQ.E3RHjKx9p0a-64RN2YPtlEMysGM45QBO9eATLBhtP4tUQNZnkraUr56hAWA-FuGmhiuMptnKNk_dU3VnbyL6SbHrMWUbquxWjyoqsd7stFs1K_nW6XIzsTjh8Bg6hB5hmsSV-M5_hPS24JwJaCdMQeWrh6cIEp2Sjft7I1V4HQrgzrkMh15sDFAw3i1_ZZasQsDYKyYbO9Jp7lx42ognPrz_KuvPzLjEXvBBNTFsVXUE-ur5adLNMvt-uXzcJ1rcwhjHWItUf5YvgRQbbBnd9f-LsJIhfkDgCJcvZmGDZrtlCKaU1UjHv5c3faZED-cjL59MbibofhPjv87MK8hhdg";

        private const string CONTACT_POLICIES_SETTINGS =
            "{\"accountName\":\"test1@test.com\",\"active\":true,\"emailsAmountByInterval\":5,\"intervalInDays\":10,\"excludedSubscribersLists\":[]}";

        private readonly WebApplicationFactory<Startup> _factory;

        public UpdateContactPoliciesSettingsTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("test1@test.com", CONTACT_POLICIES_SETTINGS, TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518,
            HttpStatusCode.NotFound)]
        public async Task
            UpdateContactPoliciesSettings_Should_ReturnNotFound_When_UserWithSameAccountNameIsNotFound(
                string accountName, string contactPoliciesRequestBody, string token, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            int? expectedNotFoundedIdUser = null;

            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            contactPoliciesMock.Setup(x => x.GetIdUserByAccountName(accountName));

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
            })).CreateClient();



            var request = new HttpRequestMessage(HttpMethod.Put, $"/accounts/{accountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } },
                Content = new StringContent(contactPoliciesRequestBody, Encoding.UTF8, "application/json")
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
            UpdateContactPoliciesSettings_Should_ReturnOK_When_UserWithSameAccountNameIsFound(
                string accountName, string token, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var fixture = new Fixture();
            var expectedFoundedIdUser = fixture.Create<int>();

            var expectedContactPoliciesSetting =
                SetUpExpectedContactPoliciesSetting(accountName, out var expectedResultAsString, true);

            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            contactPoliciesMock.Setup(x => x.GetIdUserByAccountName(accountName)).ReturnsAsync(expectedFoundedIdUser);
            contactPoliciesMock.Setup(x => x.UpdateContactPoliciesSettingsAsync(expectedFoundedIdUser, expectedContactPoliciesSetting));

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
            })).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Put, $"/accounts/{accountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } },
                Content = new StringContent(expectedResultAsString, Encoding.UTF8, "application/json")
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Theory]
        [InlineData("test1@test.com", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518,
            HttpStatusCode.InternalServerError)]
        public async Task
            UpdateContactPoliciesSettings_Should_ReturnInternalServerError_When_UserWithSameAccountNameIsFoundButDoesNotHavePermissions(
                string accountName, string token, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var fixture = new Fixture();
            var expectedFoundedIdUser = fixture.Create<int>();

            var expectedContactPoliciesSetting =
                SetUpExpectedContactPoliciesSetting(accountName, out var expectedResultAsString, true);
            var expectedContactPoliciesSettingDao = expectedContactPoliciesSetting.ToDao();

            var contactPoliciesRepositoryMock = new Mock<IContactPoliciesSettingsRepository>(MockBehavior.Strict);

            contactPoliciesRepositoryMock.Setup(x => x.GetIdUserByAccountName(accountName))
                .ReturnsAsync(expectedFoundedIdUser);
            contactPoliciesRepositoryMock.Setup(x => x.UpdateContactPoliciesSettingsAsync(expectedFoundedIdUser, expectedContactPoliciesSettingDao))
                .ThrowsAsync(new Exception("This action is not allowed for this user."));

            var contactService = new ContactPoliciesService(contactPoliciesRepositoryMock.Object);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesRepositoryMock.Object);
                services.AddSingleton(contactService);
            })).CreateClient();
            var request = new HttpRequestMessage(HttpMethod.Put, $"/accounts/{accountName}/settings")
            {
                Headers = { { "Authorization", $"Bearer {token}" } },
                Content = new StringContent(expectedResultAsString, Encoding.UTF8, "application/json")
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.NotNull(response);
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
                $"{{\"accountName\":\"test1@test.com\",\"active\":{isActive.ToString().ToLower()},\"emailsAmountByInterval\":100,\"intervalInDays\":10,\"excludedSubscribersLists\":[{{\"id\":34,\"name\":\"Listado_Marketing\"}},{{\"id\":169,\"name\":\"Testing_ExcludeList\"}}]}}";
            return expectedContactPoliciesSetting;
        }
    }
}
