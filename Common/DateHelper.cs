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

        public static string DaysToMonth(string prefix)
        {
            //DateTime startDate =  new DateTime(2018, 1, 25);
            DateTime endDate = new DateTime(2019, 1, 24);
            var totalDays = (endDate - DateTime.Today).TotalDays;
            //var totalYears = Math.Truncate(totalDays / 365);
            var totalMonths = Math.Truncate((totalDays % 365) / 30);
            var remainingDays = Math.Truncate((totalDays % 365) % 30);
            return $"{prefix} {totalMonths} month(s) and {remainingDays} day(s)";
        }
    }
}
