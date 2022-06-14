using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ECommerce.Utility
{
    public static class TimeZoneExtension
    {
        public static DateTime CurrentZone(this DateTime dateTime, string zone)
        {
            return ConvertTime(dateTime, zone);
        }
        public static DateTime CurrentZone(this DateTime? dateTime, string zone)
        {
            return ConvertTime(dateTime, zone);
        }

        private static DateTime ConvertTime(DateTime? dateTime, string zone)
        {
            if (!string.IsNullOrEmpty(zone))
            {
                var info = TimeZoneInfo.FindSystemTimeZoneById(zone);

                DateTimeOffset localServerTime = (DateTime)dateTime!;
                DateTimeOffset usersTime = TimeZoneInfo.ConvertTime(localServerTime, info);
                return usersTime.DateTime;
            }
            else
            {
                return (DateTime)dateTime!;
            }
        }
    }
}
