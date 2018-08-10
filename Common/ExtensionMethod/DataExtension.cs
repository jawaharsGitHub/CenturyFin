﻿using System;
using System.Collections.Generic;
using System.Globalization;

namespace Common.ExtensionMethod
{
    public static class DataExtension
    {

        public static IEnumerable<TSource> DistinctBy<TSource, TKey>(this IEnumerable<TSource> source, Func<TSource, TKey> keySelector)
        {
            HashSet<TKey> seenKeys = new HashSet<TKey>();
            foreach (TSource element in source)
            {
                if (seenKeys.Add(keySelector(element)))
                {
                    yield return element;
                }
            }
        }

        public static string ToMoney(this int number)
        {
            NumberFormatInfo nfo = new NumberFormatInfo();
            nfo.CurrencyGroupSeparator = ",";
            nfo.CurrencyNegativePattern = Convert.ToInt16(number < 0);
            // you are interested in this part of controlling the group sizes
            nfo.CurrencyGroupSizes = new int[] { 3, 2 };
            nfo.CurrencySymbol = ""; // "Rs.";

            return number.ToString("c0", nfo); // prints 1,50,00,000

        }

        public static string ToMoney(this decimal number)
        {
            var no = Convert.ToInt32(number);
            return ToMoney(no);
        }
    }
}