using System;
using System.Collections.Generic;
using System.Globalization;

namespace Common.ExtensionMethod
{
    public static class TypeExtenstion
    {

        public static int ToInt32(this string value)
        {
            if (string.IsNullOrEmpty(value)) return 0;

            return Convert.ToInt32(value);

        }

        public static int ToInt32(this double value)
        {
            return Convert.ToInt32(value);

        }

        public static int ToInt32(this int? value)
        {
            return Convert.ToInt32(value);

        }

        public static int ToInt32(this object value)
        {
            return Convert.ToInt32(value);

        }

        public static decimal ToDecimal(this object value)
        {
            return Convert.ToDecimal(value);
        }

        public static string TokFormat(this int? value)
        {
            if (value == null)
                return "0";
            //throw new Exception("value should not ne null");

            int number = Convert.ToInt32(value);

            return number.TokFormat();

        }

        public static string TokFormat(this double value)
        {
            int number = Convert.ToInt32(value);

            return number.TokFormat();

        }

        public static string TokFormat(this decimal? value)
        {
            int number = Convert.ToInt32(value);

            return number.TokFormat();

        }

        public static string TokFormat(this int number)
        {
            NumberFormatInfo nfo = new NumberFormatInfo();
            nfo.CurrencyGroupSeparator = ",";
            nfo.CurrencyNegativePattern = Convert.ToInt16(number < 0);
            // you are interested in this part of controlling the group sizes
            nfo.CurrencyGroupSizes = new int[] { 3, 2 };
            nfo.CurrencySymbol = ""; // "Rs.";

            return number.ToString("c0", nfo); // prints 1,50,00,000
        }

        public static T NextOf<T>(this IList<T> list, T item)
        {
            var indexOf = list.IndexOf(item);
            if (indexOf == list.Count - 1) return default(T);
            return list[indexOf == list.Count - 1 ? 0 : indexOf + 1];
        }

        public static T ToEnum<T>(this string value)
        {
            return (T)Enum.Parse(typeof(T), value, true);
        }

        public static int PercentageOf(this int value, int toPercentage)
        {
            return (toPercentage / 100) * value;
        }

        public static string PercentageOfStr(this string value, decimal toPercentage)
        {
            return Math.Round((toPercentage / 100) * value.ToInt32()).ToString();
        }


    }
}
