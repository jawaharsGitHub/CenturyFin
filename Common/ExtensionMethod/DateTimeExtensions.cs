using System;
using System.Collections.Generic;
using System.Linq;

namespace Common.ExtensionMethod
{
    public static class DateTimeExtensions
    {
        public static DateTime StartOfWeek(this DateTime dt, DayOfWeek startOfWeek)
        {
            int diff = dt.DayOfWeek - startOfWeek;
            if (diff < 0)
            {
                diff += 7;
            }
            return dt.AddDays(-1 * diff).Date;
        }

        public static string WithDateSuffix(this DateTime dt)
        {
            var dateFormat = $"{dt.ToString("dd MMMM yyyy")}";

            var dayPart = Convert.ToInt16(dateFormat.Split(' ')[0]);

            var newDayFormat = $"{dayPart}{General.GetDaySuffix(dayPart)}";

            return dateFormat.Replace(dateFormat.Split(' ')[0], newDayFormat); ;
        }

        public static string Ddmmyy(this DateTime dt, string seperator = ".")
        {
            return dt.ToString($"dd{seperator}MM{seperator}yy");
        }

        public static string Plainddmmyyyy(this DateTime dt)
        {
            return dt.ToString($"ddMMyy");
        }
        public static (DateTime fd, DateTime ld) GetFirstAndLastDate(DateTime date)
        {
            var firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            var lastDayOfMonth = firstDayOfMonth.AddMonths(1).AddDays(-1);

            return (fd: firstDayOfMonth, ld: lastDayOfMonth);
        }

        public static List<(DateTime fd, DateTime ld)> GetAllMonths(this DateTime dt)

        {
            var start = new DateTime(2018, 1, 25);
            var end = DateTime.Today;

            // set end-date to end of month
            end = new DateTime(end.Year, end.Month, DateTime.DaysInMonth(end.Year, end.Month));

            var diff = Enumerable.Range(0, Int32.MaxValue)
                                 .Select(e => start.AddMonths(e))
                                 .TakeWhile(e => e <= end)
                                 .Select(e => GetFirstAndLastDate(e));

            return diff.ToList();
        }


    }
}
