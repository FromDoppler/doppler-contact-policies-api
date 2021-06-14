using System;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using AutoFixture;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Data.Access.Entities;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using Xunit;

namespace Doppler.ContactPolicies.Api.Test
{
    public class GetContactPoliciesSettingsTest : IClassFixture<WebApplicationFactory<Startup>>
    {
        const string TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518 =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOjEyMywidW5pcXVlX25hbWUiOiJ0ZXN0MUB0ZXN0LmNvbSIsInJvbGUiOiJVU0VSIiwiZXhwIjoyMDAwMDAwMDAwfQ.E3RHjKx9p0a-64RN2YPtlEMysGM45QBO9eATLBhtP4tUQNZnkraUr56hAWA-FuGmhiuMptnKNk_dU3VnbyL6SbHrMWUbquxWjyoqsd7stFs1K_nW6XIzsTjh8Bg6hB5hmsSV-M5_hPS24JwJaCdMQeWrh6cIEp2Sjft7I1V4HQrgzrkMh15sDFAw3i1_ZZasQsDYKyYbO9Jp7lx42ognPrz_KuvPzLjEXvBBNTFsVXUE-ur5adLNMvt-uXzcJ1rcwhjHWItUf5YvgRQbbBnd9f-LsJIhfkDgCJcvZmGDZrtlCKaU1UjHv5c3faZED-cjL59MbibofhPjv87MK8hhdg";

        private readonly WebApplicationFactory<Startup> _factory;

        public GetContactPoliciesSettingsTest(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("test1@test.com", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518,
            HttpStatusCode.OK)]
        public async Task GetContactPoliciesSettings_Should_ReturnOKWithContactPoliciesResponse_When_UserAccountNameIsFound(string accountName,
            string token, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var expected = fixture.Build<ContactPoliciesSettings>()
                .With(x => x.User, fixture.Build<User>()
                    .With(x => x.Email, accountName).Create())
                .Create();

            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            contactPoliciesMock.Setup(x => x.GetContactPoliciesSettingsAsync(accountName)).ReturnsAsync(expected);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
            })).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, $"/accounts/{accountName}/settings")
            {
                Headers = {{"Authorization", $"Bearer {token}"}}
            };

            // Act
            var response = await client.SendAsync(request);
            var contentObject =
                (ContactPoliciesSettings) await response.Content.ReadFromJsonAsync(typeof(ContactPoliciesSettings));

            // Assert
            Assert.NotNull(response);
            Assert.Equal(expectedStatusCode, response.StatusCode);
            Assert.Equal(expected.User.Email, contentObject?.User.Email);
        }
    }
}
