using System;
using System.Collections.Generic;

namespace Common
{
    public static class DateHelper
    {

        public static int RemaingDaysOfMonth
        {
            get
            {
                var today = DateTime.Today;
                return (DateTime.DaysInMonth(today.Year, today.Month) - today.Day);
            }
        }

        public static (int NoOfDays, string NextMonthName) GetRemaingDaysToNextCycle()
        {
            var after100day = DateTime.Today.AddDays(100);
            var days = (new DateTime(after100day.Year, after100day.Month, DateTime.DaysInMonth(after100day.Year, after100day.Month)) - after100day).Days;

            return (days, after100day.AddMonths(1).ToString("Y"));
        }

        public static string DaysToMonth(string prefix, DateTime startDate, DateTime endDate)
        {

            var totalDays = (endDate - startDate).TotalDays;
            var totalYears = Math.Truncate(totalDays / 365);
            var totalMonths = Math.Truncate((totalDays % 365) / 30);
            var remainingDays = Math.Truncate((totalDays % 365) % 30);
            return $"{prefix} {totalYears} year(s) {totalMonths} month(s) and {remainingDays} day(s)";
        }

        public static IEnumerable<DateTime> GetDateRange(DateTime startDate, DateTime endDate)
        {
            if (endDate < startDate)
                throw new ArgumentException("endDate must be greater than or equal to startDate");

            while (startDate <= endDate)
            {
                yield return startDate;
                startDate = startDate.AddDays(1);
            }
        }

    }
}
