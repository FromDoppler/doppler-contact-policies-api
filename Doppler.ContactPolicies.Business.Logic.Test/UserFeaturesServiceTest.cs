using AutoFixture;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Business.Logic.UserApiClient.Services;
using Flurl.Http;
using Flurl.Http.Testing;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Moq;
using System;
using System.Net;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.ContactPolicies.Business.Logic.Test
{
    public class UserFeaturesServiceTest
    {
        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public async Task HasContactPoliciesFeatureAsync_Should_ReturnCorrectResult_When_Features_With_ContactPolicies_IsReturned(bool expectedFeatureContactPolicies)
        {
            // Arrange
            using var httpTest = new HttpTest();

            httpTest
                .RespondWith($"{{\"contactPolicies\":{expectedFeatureContactPolicies.ToString().ToLower()}}}", (int)HttpStatusCode.OK);

            UserFeaturesServiceSettings usersApiUrl = new UserFeaturesServiceSettings { UsersApiURL = "http://test.com/" };

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

        [Theory]
        [InlineData((int)HttpStatusCode.Forbidden, (int)HttpStatusCode.Forbidden)]
        [InlineData((int)HttpStatusCode.NotFound, (int)HttpStatusCode.NotFound)]
        public async Task HasContactPoliciesFeatureAsync_Should_ReturnCorrectStatusCode_When_ThrowException(int mockStatusCode, int expectedStatusCode)
        {
            // Arrange
            using var httpTest = new HttpTest();

            httpTest.RespondWith(string.Empty, mockStatusCode);

            UserFeaturesServiceSettings usersApiUrl = new UserFeaturesServiceSettings { UsersApiURL = "http://test.com/" };

            var usersApiTokenGetterMock = new Mock<IUsersApiTokenGetter>();

            var loggerStub = new Mock<ILogger<UserFeaturesService>>();

            var userFeaturesServiceSettingsStub = new Mock<IOptions<UserFeaturesServiceSettings>>();
            userFeaturesServiceSettingsStub.Setup(x => x.Value).Returns(usersApiUrl);

            var sut = new UserFeaturesService(usersApiTokenGetterMock.Object, userFeaturesServiceSettingsStub.Object, loggerStub.Object);
            var fixture = new Fixture();
            var accountName = fixture.Create<string>();

            // Assert
            var ex = await Assert.ThrowsAsync<FlurlHttpException>(async () => await sut.HasContactPoliciesFeatureAsync(accountName));
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
