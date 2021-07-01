using AutoFixture;
using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Services;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.Extensions.DependencyInjection;
using Moq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.ContactPolicies.Api.Test
{
    public class Authorization : IClassFixture<WebApplicationFactory<Startup>>
    {
        const string TOKEN_EXPIRE_20330518 =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJleHAiOjIwMDAwMDAwMDB9.mll33c0kstVIN9Moo4HSw0CwRjn0IuDc2h1wkRrv2ahQtIG1KV5KIxYw-H3oRfd-PiCWHhIVIYDP3mWDZbsOHTlnpRGpHp4f26LAu1Xp1hDJfOfxKYEGEE62Xt_0qp7jSGQjrx-vQey4l2mNcWkOWiE0plOws7cX-wLUvA3NLPoOvEegjM0Wx6JFcvYLdMGcTGT5tPd8Pq8pe9VYstCbhOClzI0bp81iON3f7VQP5d0n64eb_lvEPFu5OfURD4yZK2htyQK7agcNNkP1c5mLEfUi39C7Qtx96aAhOjir6Wfhzv_UEs2GQKXGTHl6_-HH-ecgOdIvvbqXGLeDmTkXUQ";

        const string TOKEN_SUPERUSER_FALSE_EXPIRE_20330518 =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc1NVIjpmYWxzZSwiZXhwIjoyMDAwMDAwMDAwfQ.qMY3h8VhNxuOBciqrmXpTrRk8ElwDlT_3CYFzqJdXNjnJhKihFVMwjkWVw1EEckCWbKsRoBr-NgRV0SZ0JKWbMr2oGhZJWtqmKA05d8-i_MuuYbxtt--NUoQxg6AsMX989PGf6fSBzo_4szb7J0G6nUvvRxXfMnHMpaIAQUiBLNOoeKwnzsZFfI1ehmYGNmtc-2XyXOEHAnfZeBZw8uMWOp4A5hFBpVsaVCUiRirokjeCMWViVWT9NnVWbA60e_kfLjghEcXWaZfNnX9qtj4OC8QUB33ByUmwuYlTxNnu-qiEaJmbaaTeDD2JrKHf6MR59MlCHbb6BDWt20DBy73WQ";

        const string TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518 =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJuYW1laWQiOjEyMywidW5pcXVlX25hbWUiOiJ0ZXN0MUB0ZXN0LmNvbSIsInJvbGUiOiJVU0VSIiwiZXhwIjoyMDAwMDAwMDAwfQ.E3RHjKx9p0a-64RN2YPtlEMysGM45QBO9eATLBhtP4tUQNZnkraUr56hAWA-FuGmhiuMptnKNk_dU3VnbyL6SbHrMWUbquxWjyoqsd7stFs1K_nW6XIzsTjh8Bg6hB5hmsSV-M5_hPS24JwJaCdMQeWrh6cIEp2Sjft7I1V4HQrgzrkMh15sDFAw3i1_ZZasQsDYKyYbO9Jp7lx42ognPrz_KuvPzLjEXvBBNTFsVXUE-ur5adLNMvt-uXzcJ1rcwhjHWItUf5YvgRQbbBnd9f-LsJIhfkDgCJcvZmGDZrtlCKaU1UjHv5c3faZED-cjL59MbibofhPjv87MK8hhdg";

        const string TOKEN_SUPERUSER_EXPIRE_20330518 =
            "eyJhbGciOiJSUzI1NiIsInR5cCI6IkpXVCJ9.eyJpc1NVIjp0cnVlLCJleHAiOjIwMDAwMDAwMDB9.rUtvRqMxrnQzVHDuAjgWa2GJAJwZ-wpaxqdjwP7gmVa7XJ1pEmvdTMBdirKL5BJIE7j2_hsMvEOKUKVjWUY-IE0e0u7c82TH0l_4zsIztRyHMKtt9QE9rBRQnJf8dcT5PnLiWkV_qEkpiIKQ-wcMZ1m7vQJ0auEPZyyFBKmU2caxkZZOZ8Kw_1dx-7lGUdOsUYad-1Rt-iuETGAFijQrWggcm3kV_KmVe8utznshv2bAdLJWydbsAUEfNof0kZK5Wu9A80DJd3CRiNk8mWjQxF_qPOrGCANOIYofhB13yuYi48_8zVPYku-llDQjF77BmQIIIMrCXs8IMT3Lksdxuw";

        private readonly WebApplicationFactory<Startup> _factory;

        public Authorization(WebApplicationFactory<Startup> factory)
        {
            _factory = factory;
        }

        [Theory]
        [InlineData("/accounts/test1@test.com/settings", TOKEN_EXPIRE_20330518, HttpStatusCode.Forbidden)]
        [InlineData("/accounts/test1@test.com/settings", TOKEN_SUPERUSER_FALSE_EXPIRE_20330518,
            HttpStatusCode.Forbidden)]
        [InlineData("/accounts/test2@test.com/settings", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518,
            HttpStatusCode.Forbidden)]
        public async Task
            GET_account_endpoint_should_require_a_valid_token_with_isSU_flag_or_a_token_for_the_right_account(
                string url, string token, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var client = _factory.CreateClient(new WebApplicationFactoryClientOptions());

            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers = { { "Authorization", $"Bearer {token}" } }
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }

        [Theory]
        [InlineData("/accounts/test1@test.com/settings", TOKEN_SUPERUSER_EXPIRE_20330518, HttpStatusCode.OK)]
        [InlineData("/accounts/test1@test.com/settings", TOKEN_ACCOUNT_123_TEST1_AT_TEST_DOT_COM_EXPIRE_20330518,
            HttpStatusCode.OK)]
        public async Task
            GET_account_endpoint_should_accept_valid_token_with_isSU_flag_or_a_token_for_the_right_account(string url,
                string token, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var fixture = new Fixture();
            var accountName = "test1@test.com";
            var contactPoliciesSettings = fixture.Create<ContactPoliciesSettingsDto>();

            var contactPoliciesMock = new Mock<IContactPoliciesService>();
            contactPoliciesMock.Setup(x => x.GetIdUserByAccountName(accountName)).ReturnsAsync(It.IsAny<int>());
            contactPoliciesMock.Setup(x => x.GetContactPoliciesSettingsAsync(It.IsAny<int>())).ReturnsAsync(contactPoliciesSettings);

            var client = _factory.WithWebHostBuilder((e) => e.ConfigureTestServices(services =>
            {
                services.AddSingleton(contactPoliciesMock.Object);
            })).CreateClient();

            var request = new HttpRequestMessage(HttpMethod.Get, url)
            {
                Headers = { { "Authorization", $"Bearer {token}" } }
            };

            // Act
            var response = await client.SendAsync(request);

            // Assert
            Assert.Equal(expectedStatusCode, response.StatusCode);
        }
    }
}
