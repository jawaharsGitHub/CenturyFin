using System;

namespace Common
{
    public static class DateHelper
    {

        public static int RemaingDaysOfMonth
        {
            get
            {
                var today = DateTime.Today;
                return (today.Day - DateTime.DaysInMonth(today.Year, today.Month));
            }
        }

        public static int RemaingDaysToNextCycle
        {
            get
            {
                var after100day = DateTime.Today.AddDays(100);
                var days = (new DateTime(after100day.Year, after100day.Month, DateTime.DaysInMonth(after100day.Year, after100day.Month)) - after100day).Days;
                return days;
            }
        }
    }
}
