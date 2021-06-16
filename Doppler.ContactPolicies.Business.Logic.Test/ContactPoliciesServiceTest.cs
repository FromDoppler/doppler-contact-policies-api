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
            GetContactPoliciesSettings_Should_ReturnContactPoliciesSettings_When_UserWithSameAccountNameIsFoundAndContactPoliciesAreActivated(
                string accountName)
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Build<ContactPoliciesSettings>()
                .With(x => x.AccountName, accountName)
                .With(x => x.Active, true)
                .Create();

            var contactPoliciesRepositoryMock = new Mock<IContactPoliciesSettingsRepository>();
            contactPoliciesRepositoryMock.Setup(x => x.GetContactPoliciesSettingsAsync(accountName))
                .ReturnsAsync(expected).Verifiable();

            var contactPoliciesSut = new ContactPoliciesService(contactPoliciesRepositoryMock.Object);

            // Act
            var actual = await contactPoliciesSut.GetContactPoliciesSettingsAsync(accountName);

            // Assert
            contactPoliciesRepositoryMock.Verify();
            Assert.Equal(actual.AccountName, expected.AccountName);
            Assert.True(actual.Active);
        }

        [Theory]
        [InlineData("not_found@makingsense.com")]
        public async Task
            GetContactPoliciesSettings_Should_ReturnNull_When_UserWithSameAccountNameIsNotFound(
                string accountName)
        {
            // Arrange
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

        [Theory]
        [InlineData("user_exist@makingsense.com")]
        public async Task
            GetContactPoliciesSettings_Should_ReturnContactPoliciesSettings_When_UserWithSameAccountNameIsFoundAndContactPoliciesIsNotActivated(
                string accountName)
        {
            // Arrange
            var fixture = new Fixture();
            var expected = fixture.Build<ContactPoliciesSettings>()
                .With(x => x.AccountName, accountName)
                .With(x => x.Active, false)
                .Create();

            var contactPoliciesRepositoryMock = new Mock<IContactPoliciesSettingsRepository>();
            contactPoliciesRepositoryMock.Setup(x => x.GetContactPoliciesSettingsAsync(accountName))
                .ReturnsAsync(expected).Verifiable();

            var contactPoliciesSut = new ContactPoliciesService(contactPoliciesRepositoryMock.Object);

            // Act
            var actual = await contactPoliciesSut.GetContactPoliciesSettingsAsync(accountName);

            // Assert
            contactPoliciesRepositoryMock.Verify();
            Assert.Equal(actual.AccountName, expected.AccountName);
            Assert.False(actual.Active);
        }

        [Theory]
        [InlineData("user_exist@makingsense.com")]
        public async Task
            GetContactPoliciesSettings_Should_ReturnExcludedSubscribersListsAsNull_When_UserWithSameAccountNameIsFoundAndContactPoliciesIsNotEnabled(
                string accountName)
        {
            // Arrange
            var expected = new ContactPoliciesSettings()
            {
                AccountName = accountName,
                Active = false,
                EmailsAmountByInterval = null,
                IntervalInDays = null,
                EnabledExcludedSubscribersList = false,
                ExcludedSubscribersLists = null
            };

            var contactPoliciesRepositoryMock = new Mock<IContactPoliciesSettingsRepository>();
            contactPoliciesRepositoryMock.Setup(x => x.GetContactPoliciesSettingsAsync(accountName))
                .ReturnsAsync(expected).Verifiable();

            var contactPoliciesSut = new ContactPoliciesService(contactPoliciesRepositoryMock.Object);

            // Act
            var actual = await contactPoliciesSut.GetContactPoliciesSettingsAsync(accountName);

            // Assert
            contactPoliciesRepositoryMock.Verify();
            Assert.Equal(actual.AccountName, expected.AccountName);
            Assert.False(actual.Active);
            Assert.Null(actual.ExcludedSubscribersLists);
        }
    }
}