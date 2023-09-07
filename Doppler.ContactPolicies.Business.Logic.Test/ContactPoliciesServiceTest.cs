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
            var timeRestrictionDto = new ContactPoliciesTimeRestrictionDto()
            {
                TimeSlotEnabled = true,
                HourFrom = 0,
                HourTo = 23,
                WeekdaysEnabled = false,
            };
            var contactPoliciesSettingDto = new ContactPoliciesSettingsDto()
            {
                AccountName = "prueba@makingsense.com",
                Active = true,
                EmailsAmountByInterval = 999,
                IntervalInDays = 30,
                ExcludedSubscribersLists = excludedLists,
                TimeRestriction = timeRestrictionDto,
            };

            var contactPoliciesSettingExpected = new ContactPoliciesSettings()
            {
                AccountName = null,
                Active = true,
                EmailsAmountByInterval = 999,
                IntervalInDays = 30,
                UserHasContactPolicies = false,
                ExcludedSubscribersLists = excludedLists,
            };
            var contactPoliciesTimeRestrictionExpected = new ContactPoliciesTimeRestriction()
            {
                TimeSlotEnabled = true,
                HourFrom = 0,
                HourTo = 23,
                WeekdaysEnabled = false,
            };

            var contactPoliciesSettingsRepositoryMock = new Mock<IContactPoliciesSettingsRepository>();

            var contactPoliciesSut = new ContactPoliciesService(contactPoliciesSettingsRepositoryMock.Object);

            // Act
            await contactPoliciesSut.UpdateContactPoliciesSettingsAsync(idUser, contactPoliciesSettingDto);

            // Assert
            contactPoliciesSettingsRepositoryMock.Verify(
                repo => repo.UpdateContactPoliciesSettingsAsync(
                    idUser,
                    It.Is<ContactPoliciesSettings>(cps =>
                        cps.AccountName == contactPoliciesSettingExpected.AccountName
                        && cps.Active == contactPoliciesSettingExpected.Active
                        && cps.EmailsAmountByInterval == contactPoliciesSettingExpected.EmailsAmountByInterval
                        && cps.IntervalInDays == contactPoliciesSettingExpected.IntervalInDays
                        && cps.UserHasContactPolicies == contactPoliciesSettingExpected.UserHasContactPolicies
                        && cps.ExcludedSubscribersLists.Count == contactPoliciesSettingExpected.ExcludedSubscribersLists.Count
                        && cps.ExcludedSubscribersLists[0] == contactPoliciesSettingExpected.ExcludedSubscribersLists[0]
                    ),
                    It.Is<ContactPoliciesTimeRestriction>(cptr =>
                        cptr.TimeSlotEnabled == contactPoliciesTimeRestrictionExpected.TimeSlotEnabled
                        && cptr.HourFrom == contactPoliciesTimeRestrictionExpected.HourFrom
                        && cptr.HourTo == contactPoliciesTimeRestrictionExpected.HourTo
                        && cptr.WeekdaysEnabled == contactPoliciesTimeRestrictionExpected.WeekdaysEnabled
                    )
                ),
                Times.Once
            );
        }
    }
}
