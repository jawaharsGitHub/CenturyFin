using System;
using System.Collections.Generic;

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

        public static string TokFormat(this int? value)
        {
            if (value == null)
                throw new Exception("value should not ne null");

            int number = Convert.ToInt32(value);

            return string.Format("{0:n0}", number);

        }

        public static T NextOf<T>(this IList<T> list, T item)
        {
            var indexOf = list.IndexOf(item);
            if (indexOf == list.Count - 1) return default(T);
            return list[indexOf == list.Count - 1 ? 0 : indexOf + 1];
        }

    }
}
