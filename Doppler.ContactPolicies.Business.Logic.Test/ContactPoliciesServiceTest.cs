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
                .With(x => x.HourFrom, 1)
                .With(x => x.HourTo, 2)
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

        [Theory]
        [InlineData(22, 23, 19, 20, -180)]
        [InlineData(1, 2, 22, 23, -180)]
        [InlineData(22, 23, 1, 2, 180)]
        [InlineData(1, 2, 4, 5, 180)]
        public async Task
            GetContactPoliciesSettings_Should_Convert_TimeRestrictionHours_To_UserTimeZone_When_OffsetMinutes_ExactHours
            (
                int hourFromInDB,
                int hourToInDB,
                int hourFromForResponse,
                int hourToFoResponse,
                int offsetMinutes
            )
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
                .With(x => x.HourFrom, hourFromInDB)
                .With(x => x.HourTo, hourToInDB)
                .With(x => x.TimeZoneOffsetMinutes, offsetMinutes)
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
            Assert.Equal(actual.TimeRestriction.HourFrom, hourFromForResponse);
            Assert.Equal(actual.TimeRestriction.HourTo, hourToFoResponse);
        }

        [Theory]
        [InlineData(22, 23, 19, 20, -150)]
        [InlineData(1, 2, 22, 23, -150)]
        [InlineData(22, 23, 0, 1, 150)]
        [InlineData(1, 2, 3, 4, 150)]
        public async Task
            GetContactPoliciesSettings_Should_Convert_TimeRestrictionHours_To_UserTimeZone_When_OffsetMinutes_NonExactHours
            (
                int hourFromInDB,
                int hourToInDB,
                int hourFromForResponse,
                int hourToFoResponse,
                int offsetMinutes
            )
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
                .With(x => x.HourFrom, hourFromInDB)
                .With(x => x.HourTo, hourToInDB)
                .With(x => x.TimeZoneOffsetMinutes, offsetMinutes)
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
            Assert.Equal(actual.TimeRestriction.HourFrom, hourFromForResponse);
            Assert.Equal(actual.TimeRestriction.HourTo, hourToFoResponse);
        }

        [Theory]
        [InlineData(null, null, null, null, 180)]
        [InlineData(1, 2, 1, 2, 0)]
        public async Task GetContactPoliciesSettings_ShouldNot_Convert_TimeRestrictionHours_When_OffsetMinutesIsZero_Or_HoursAreNull
            (
                int? hourFromInDB,
                int? hourToInDB,
                int? hourFromForResponse,
                int? hourToFoResponse,
                int offsetMinutes
            )
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
                .With(x => x.HourFrom, hourFromInDB)
                .With(x => x.HourTo, hourToInDB)
                .With(x => x.TimeZoneOffsetMinutes, offsetMinutes)
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
            Assert.Equal(actual.TimeRestriction.HourFrom, hourFromForResponse);
            Assert.Equal(actual.TimeRestriction.HourTo, hourToFoResponse);
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

        [Theory]
        [InlineData(22, 23, 19, 20, -180)]
        [InlineData(1, 2, 22, 23, -180)]
        [InlineData(22, 23, 1, 2, 180)]
        [InlineData(1, 2, 4, 5, 180)]
        public async Task
            UpdateContactPoliciesSettings_Should_Convert_TimeRestrictionHours_To_UTC_When_OffsetMinutes_ExactHours
            (
                int hourFromForDB,
                int hourToForDB,
                int hourFromInRequest,
                int hourToInRequest,
                int offsetMinutes
            )
        {
            // Arrange
            var idUser = 1000;

            var excludedLists = new List<ExcludedSubscribersLists>
            {
                new ExcludedSubscribersLists() { Id = 1, Name = "Test" }
            };

            var fixture = new Fixture();
            var timeRestriction = new ContactPoliciesTimeRestrictionDto()
            {
                HourFrom = hourFromInRequest,
                HourTo = hourToInRequest,
            };
            var contactPoliciesSettingsDto = fixture.Build<ContactPoliciesSettingsDto>()
                .With(x => x.ExcludedSubscribersLists, excludedLists)
                .With(x => x.TimeRestriction, timeRestriction)
                .Create();

            var contactPoliciesSettingsRepositoryMock = new Mock<IContactPoliciesSettingsRepository>();
            contactPoliciesSettingsRepositoryMock.Setup(x => x.GetTimezoneOffsetMinutes(It.IsAny<int>()))
                .ReturnsAsync(offsetMinutes).Verifiable();

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
                        && cptr.HourFrom == hourFromForDB
                        && cptr.HourTo == hourToForDB
                        && cptr.WeekdaysEnabled == contactPoliciesSettingsDto.TimeRestriction.WeekdaysEnabled
                    )
                ),
                Times.Once
            );
        }

        [Theory]
        [InlineData(22, 23, 19, 20, -150)]
        [InlineData(1, 2, 22, 23, -150)]
        [InlineData(22, 23, 0, 1, 150)]
        [InlineData(1, 2, 3, 4, 150)]
        public async Task
            UpdateContactPoliciesSettings_Should_Convert_TimeRestrictionHours_To_UTC_When_OffsetMinutes_NonExactHours
            (
                int hourFromForDB,
                int hourToForDB,
                int hourFromInRequest,
                int hourToInRequest,
                int offsetMinutes
            )
        {
            // Arrange
            var idUser = 1000;

            var excludedLists = new List<ExcludedSubscribersLists>
            {
                new ExcludedSubscribersLists() { Id = 1, Name = "Test" }
            };

            var fixture = new Fixture();
            var timeRestriction = new ContactPoliciesTimeRestrictionDto()
            {
                HourFrom = hourFromInRequest,
                HourTo = hourToInRequest,
            };
            var contactPoliciesSettingsDto = fixture.Build<ContactPoliciesSettingsDto>()
                .With(x => x.ExcludedSubscribersLists, excludedLists)
                .With(x => x.TimeRestriction, timeRestriction)
                .Create();

            var contactPoliciesSettingsRepositoryMock = new Mock<IContactPoliciesSettingsRepository>();
            contactPoliciesSettingsRepositoryMock.Setup(x => x.GetTimezoneOffsetMinutes(It.IsAny<int>()))
                .ReturnsAsync(offsetMinutes).Verifiable();

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
                        && cptr.HourFrom == hourFromForDB
                        && cptr.HourTo == hourToForDB
                        && cptr.WeekdaysEnabled == contactPoliciesSettingsDto.TimeRestriction.WeekdaysEnabled
                    )
                ),
                Times.Once
            );
        }
    }
}
