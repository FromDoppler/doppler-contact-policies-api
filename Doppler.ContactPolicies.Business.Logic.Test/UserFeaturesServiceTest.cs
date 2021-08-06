using AutoFixture;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Business.Logic.UserApiClient.Services;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using Moq.Protected;
using System;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.ContactPolicies.Business.Logic.Test
{
    public class UserFeaturesServiceTest
    {
        [Theory(Skip = "Working on http client mock up")]
        [InlineData(true)]
        [InlineData(false)]
        public async Task HasContactPoliciesFeatureAsync_Should_ReturnCorrectResult_When_Features_With_ContactPolicies_IsReturned(bool expectedFeatureContactPolicies)
        {
            // Arrange
            var httpMessageHandlerStub = new Mock<HttpMessageHandler>();
            httpMessageHandlerStub.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(),
                    ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = HttpStatusCode.OK,
                    Content = new StringContent(
                        $"{{\"contactPolicies\":{expectedFeatureContactPolicies.ToString().ToLower()}}}"),
                });
            UserFeaturesServiceSettings usersApiUrl = new UserFeaturesServiceSettings { UsersApiURL = "http://test.com/" };

            var httpClient = new HttpClient(httpMessageHandlerStub.Object);
            var usersApiTokenGetterMock = new Mock<IUsersApiTokenGetter>();

            var loggerStub = new Mock<ILogger<UserFeaturesService>>();

            var userFeaturesServiceSettingsStub = new Mock<IOptions<UserFeaturesServiceSettings>>();
            userFeaturesServiceSettingsStub.Setup(x => x.Value).Returns(usersApiUrl);

            var sut = new UserFeaturesService(usersApiTokenGetterMock.Object, userFeaturesServiceSettingsStub.Object, loggerStub.Object);

            // Act
            var result = await sut.HasContactPoliciesFeatureAsync(It.IsAny<string>());

            // Assert
            Assert.Equal(expectedFeatureContactPolicies, result);
        }

        [Theory(Skip = "Working on http client mock up")]
        [InlineData(HttpStatusCode.Forbidden, HttpStatusCode.Forbidden)]
        [InlineData(HttpStatusCode.NotFound, HttpStatusCode.NotFound)]
        public async Task HasContactPoliciesFeatureAsync_Should_ReturnCorrectStatusCode_When_ThrowException(HttpStatusCode mockStatusCode, HttpStatusCode expectedStatusCode)
        {
            // Arrange
            var httpMessageHandlerStub = new Mock<HttpMessageHandler>();
            httpMessageHandlerStub.Protected()
                .Setup<Task<HttpResponseMessage>>("SendAsync", ItExpr.IsAny<HttpRequestMessage>(), ItExpr.IsAny<CancellationToken>())
                .ReturnsAsync(new HttpResponseMessage
                {
                    StatusCode = mockStatusCode
                })
                .Verifiable();
            UserFeaturesServiceSettings usersApiUrl = new UserFeaturesServiceSettings { UsersApiURL = "http://test.com/" };

            var httpClient = new HttpClient(httpMessageHandlerStub.Object);
            var usersApiTokenGetterMock = new Mock<IUsersApiTokenGetter>();

            var loggerStub = new Mock<ILogger<UserFeaturesService>>();

            var userFeaturesServiceSettingsStub = new Mock<IOptions<UserFeaturesServiceSettings>>();
            userFeaturesServiceSettingsStub.Setup(x => x.Value).Returns(usersApiUrl);

            var sut = new UserFeaturesService(usersApiTokenGetterMock.Object, userFeaturesServiceSettingsStub.Object, loggerStub.Object);
            var fixture = new Fixture();
            var accountName = fixture.Create<string>();

            // Assert
            var ex = await Assert.ThrowsAsync<HttpRequestException>(async () => await sut.HasContactPoliciesFeatureAsync(accountName));
            loggerStub.Verify(
                x => x.Log(
                    It.Is<LogLevel>(l => l == LogLevel.Error),
                    It.IsAny<EventId>(),
                    It.Is<It.IsAnyType>((v, t) => v.ToString() == $"Failed to get contact policies feature with account name {accountName}"),
                    It.IsAny<Exception>(),
                    It.Is<Func<It.IsAnyType, Exception, string>>((v, t) => true)),
                Times.Once);
            Assert.Equal(expectedStatusCode, ex.StatusCode);
        }
    }
}
