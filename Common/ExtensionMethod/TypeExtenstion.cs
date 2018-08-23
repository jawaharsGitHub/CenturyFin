using System;

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
    }
}
