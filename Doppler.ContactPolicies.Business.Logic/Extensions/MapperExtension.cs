using Doppler.ContactPolicies.Business.Logic.DTO;
using Doppler.ContactPolicies.Data.Access.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Doppler.ContactPolicies.Business.Logic.Extensions
{
    public enum TimeZoneConversionEnum
    {
        CONVERT_TO_UTC,
        CONVERT_TO_USERTIMEZONE,
    }

    public static class MapperExtension
    {
        public static ContactPoliciesSettingsDto ToDto(
            this ContactPoliciesSettings contactPoliciesSettings)
        {
            if (contactPoliciesSettings == null)
                return null;
            return new ContactPoliciesSettingsDto
            {
                AccountName = contactPoliciesSettings.AccountName,
                Active = contactPoliciesSettings.Active,
                EmailsAmountByInterval = contactPoliciesSettings.EmailsAmountByInterval,
                IntervalInDays = contactPoliciesSettings.IntervalInDays,
                ExcludedSubscribersLists = contactPoliciesSettings.ExcludedSubscribersLists?.ToList()
            };
        }

        public static ContactPoliciesSettings ToDao(
            this ContactPoliciesSettingsDto contactPoliciesSettings)
        {
            if (contactPoliciesSettings == null)
                return null;
            return new ContactPoliciesSettings
            {
                Active = contactPoliciesSettings.Active,
                EmailsAmountByInterval = contactPoliciesSettings.EmailsAmountByInterval,
                IntervalInDays = contactPoliciesSettings.IntervalInDays,
                ExcludedSubscribersLists = contactPoliciesSettings.ExcludedSubscribersLists == null
                    ? new List<ExcludedSubscribersLists>()
                    : new List<ExcludedSubscribersLists>(contactPoliciesSettings.ExcludedSubscribersLists)
            };
        }

        public static ContactPoliciesTimeRestrictionDto ToDto(
            this ContactPoliciesTimeRestriction timeRestriction)
        {
            if (timeRestriction == null)
            {
                return null;
            }

            return new ContactPoliciesTimeRestrictionDto
            {
                TimeSlotEnabled = timeRestriction.TimeSlotEnabled,
                HourFrom = ApplyHourOffset(timeRestriction.HourFrom, timeRestriction.TimeZoneOffsetMinutes, TimeZoneConversionEnum.CONVERT_TO_USERTIMEZONE),
                HourTo = ApplyHourOffset(timeRestriction.HourTo, timeRestriction.TimeZoneOffsetMinutes, TimeZoneConversionEnum.CONVERT_TO_USERTIMEZONE),
                WeekdaysEnabled = timeRestriction.WeekdaysEnabled
            };
        }

        public static ContactPoliciesTimeRestriction ToDao(
            this ContactPoliciesTimeRestrictionDto contactPoliciesTimeRestrictionDto,
            int timezoneOffsetMinutes)
        {
            if (contactPoliciesTimeRestrictionDto == null)
            {
                return null;
            }

            return new ContactPoliciesTimeRestriction
            {
                TimeSlotEnabled = contactPoliciesTimeRestrictionDto.TimeSlotEnabled,
                HourFrom = ApplyHourOffset(contactPoliciesTimeRestrictionDto.HourFrom, timezoneOffsetMinutes, TimeZoneConversionEnum.CONVERT_TO_UTC),
                HourTo = ApplyHourOffset(contactPoliciesTimeRestrictionDto.HourTo, timezoneOffsetMinutes, TimeZoneConversionEnum.CONVERT_TO_UTC),
                WeekdaysEnabled = contactPoliciesTimeRestrictionDto.WeekdaysEnabled
            };
        }

        private static int? ApplyHourOffset(int? hour, int offset, TimeZoneConversionEnum conversion)
        {
            if (!hour.HasValue || offset == 0)
            {
                return hour;
            }

            DateTime now = DateTime.Now;
            DateTime auxDate = new DateTime(now.Year, now.Month, now.Day, hour.Value, 0, 0);

            int offsetMinutes = offset * (conversion == TimeZoneConversionEnum.CONVERT_TO_UTC ? -1 : 1);
            DateTime resultDate = auxDate.AddMinutes(offsetMinutes);

            var hour24 = resultDate.ToString("HH");

            // when the offset is not an exact quantity of hours, for example: (GMT+09:30) Adelaide.
            if (resultDate.Minute > 0)
            {
                return (int.Parse(hour24) + (conversion == TimeZoneConversionEnum.CONVERT_TO_UTC ? 1 : 0)) % 24;
            }
            else
            {
                return int.Parse(hour24);
            }
        }
    }
}
