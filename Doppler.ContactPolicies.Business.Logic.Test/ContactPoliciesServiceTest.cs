using System;
using System.Threading.Tasks;
using AutoFixture;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Data.Access.Entities;
using Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings;
using Moq;
using Xunit;

namespace Doppler.ContactPolicies.Business.Logic.Test
{
    public class ContactPoliciesServiceTest
    {
        [Theory]
        [InlineData("prueba@makingsense.com")]
        public async Task
            GetContactPoliciesSettings_Should_ReturnContactPoliciesSetting_When_UserWithSameAccountNameIsFound(
                string accountName)
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            var expected = fixture.Build<ContactPoliciesSettings>()
                .With(x => x.User, fixture.Build<User>()
                    .With(x => x.Email, accountName).Create())
                .Create();

            var contactPoliciesRepositoryMock = new Mock<IContactPoliciesSettingsRepository>();
            contactPoliciesRepositoryMock.Setup(x => x.GetContactPoliciesSettingsAsync(accountName))
                .ReturnsAsync(expected).Verifiable();

            var contactPoliciesSut = new ContactPoliciesService(contactPoliciesRepositoryMock.Object);

            // Act
            var actual = await contactPoliciesSut.GetContactPoliciesSettingsAsync(accountName);

            // Assert
            contactPoliciesRepositoryMock.Verify();
            Assert.Equal(actual.User.Email, expected.User.Email);
        }

        [Theory]
        [InlineData("notfound@makingsense.com")]
        public async Task
            GetContactPoliciesSettings_Should_ReturnNull_When_UserWithSameAccountNameIsNotFound(
                string accountName)
        {
            // Arrange
            var fixture = new Fixture();
            fixture.Behaviors.Remove(new ThrowingRecursionBehavior());
            fixture.Behaviors.Add(new OmitOnRecursionBehavior());
            ContactPoliciesSettings expected = null;

            var contactPoliciesRepositoryMock = new Mock<IContactPoliciesSettingsRepository>();
            contactPoliciesRepositoryMock.Setup(x => x.GetContactPoliciesSettingsAsync(accountName))
                .ReturnsAsync(expected).Verifiable();

            var contactPoliciesSut = new ContactPoliciesService(contactPoliciesRepositoryMock.Object);

            // Act
            var actual = await contactPoliciesSut.GetContactPoliciesSettingsAsync(accountName);

            // Assert
            contactPoliciesRepositoryMock.Verify();
            Assert.Null(actual);
        }
    }
}
