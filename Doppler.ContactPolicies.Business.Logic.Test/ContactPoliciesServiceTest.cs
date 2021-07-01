using AutoFixture;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Data.Access.Entities;
using Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings;
using Moq;
using System.Threading.Tasks;
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
            contactPoliciesRepositoryMock.Setup(x => x.GetContactPoliciesSettingsAsync(It.IsAny<int>()))
                .ReturnsAsync(expected).Verifiable();

            var contactPoliciesSut = new ContactPoliciesService(contactPoliciesRepositoryMock.Object);

            // Act
            var actual = await contactPoliciesSut.GetContactPoliciesSettingsAsync(It.IsAny<int>());

            // Assert
            contactPoliciesRepositoryMock.Verify();
            Assert.Equal(actual.AccountName, expected.AccountName);
            Assert.True(actual.Active);
        }
    }
}
