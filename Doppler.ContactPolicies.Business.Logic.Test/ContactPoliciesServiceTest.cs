using AutoFixture;
using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Business.Logic.Services;
using Doppler.ContactPolicies.Data.Access.Entities;
using Doppler.ContactPolicies.Data.Access.Repositories.ContactPoliciesSettings;
using Moq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Xunit;

namespace Doppler.ContactPolicies.Business.Logic.Test
{
    public class ContactPoliciesServiceTest
    {
        [Fact]
        public async Task
            GetContactPoliciesSettings_Should_ReturnContactPoliciesSettings_When_UserWithSameAccountNameIsFoundAndContactPoliciesAreActivated()
        {
            // Arrange
            var fixture = new Fixture();
            string accountName = "prueba@makingsense.com";
            var expected = fixture.Build<ContactPoliciesSettings>()
                .With(x => x.AccountName, accountName)
                .With(x => x.Active, true)
                .Create();
            var expectedTimeRestriction = fixture.Build<ContactPoliciesTimeRestriction>()
                .With(x => x.AccountName, accountName)
                .Create();

            var contactPoliciesRepositoryMock = new Mock<IContactPoliciesSettingsRepository>();
            contactPoliciesRepositoryMock.Setup(x => x.GetContactPoliciesSettingsByIdUserAsync(It.IsAny<int>()))
                .ReturnsAsync(expected).Verifiable();
            contactPoliciesRepositoryMock.Setup(x => x.GetContactPoliciesTimeRestrictionByIdUserAsync(It.IsAny<int>()))
                .ReturnsAsync(expectedTimeRestriction).Verifiable();

            var contactPoliciesSut = new ContactPoliciesService(contactPoliciesRepositoryMock.Object);

            // Act
            var actual = await contactPoliciesSut.GetContactPoliciesSettingsByIdUserAsync(It.IsAny<int>());

            // Assert
            contactPoliciesRepositoryMock.Verify();
            Assert.Equal(actual.AccountName, expected.AccountName);
            Assert.True(actual.Active);
        }

        [Fact]
        public async Task
            UpdateContactPoliciesSettings_Should_InvokeContactPoliciesSettingsRepository_With_ProperParameters()
        {
            // Arrange
            var idUser = 1000;

            var excludedLists = new List<ExcludedSubscribersLists>
            {
                new ExcludedSubscribersLists() { Id = 1, Name = "Test" }
            };

            var fixture = new Fixture();
            var contactPoliciesSettingsDto = fixture.Build<ContactPoliciesSettingsDto>()
                .With(x => x.ExcludedSubscribersLists, excludedLists)
                .Create();

            var contactPoliciesSettingsRepositoryMock = new Mock<IContactPoliciesSettingsRepository>();

            var contactPoliciesSut = new ContactPoliciesService(contactPoliciesSettingsRepositoryMock.Object);

            // Act
            await contactPoliciesSut.UpdateContactPoliciesSettingsAsync(idUser, contactPoliciesSettingsDto);

            // Assert
            contactPoliciesSettingsRepositoryMock.Verify(
                repo => repo.UpdateContactPoliciesSettingsAsync(
                    idUser,
                    It.Is<ContactPoliciesSettings>(cps =>
                        cps.AccountName == null
                        && cps.Active == contactPoliciesSettingsDto.Active
                        && cps.EmailsAmountByInterval == contactPoliciesSettingsDto.EmailsAmountByInterval
                        && cps.IntervalInDays == contactPoliciesSettingsDto.IntervalInDays
                        && cps.UserHasContactPolicies == false
                        && cps.ExcludedSubscribersLists.Count == contactPoliciesSettingsDto.ExcludedSubscribersLists.Count
                        && cps.ExcludedSubscribersLists[0] == contactPoliciesSettingsDto.ExcludedSubscribersLists[0]
                    ),
                    It.Is<ContactPoliciesTimeRestriction>(cptr =>
                        cptr.TimeSlotEnabled == contactPoliciesSettingsDto.TimeRestriction.TimeSlotEnabled
                        && cptr.HourFrom == contactPoliciesSettingsDto.TimeRestriction.HourFrom
                        && cptr.HourTo == contactPoliciesSettingsDto.TimeRestriction.HourTo
                        && cptr.WeekdaysEnabled == contactPoliciesSettingsDto.TimeRestriction.WeekdaysEnabled
                    )
                ),
                Times.Once
            );
        }
    }
}
