using System;

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
    }
}
