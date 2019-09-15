using System;

namespace Common
{
    public static class GlobalValue
    {

        public static string NoteOption { get; set; }

        public static string SearchText { get; set; }

        public static DateTime? CollectionDate { get; set; } = DateTime.Today;

        public static int SortingByValue { get; set; }

        public static int ReturnTypeValue { get; set; }

        public static bool FriendAlsoValue { get; set; }

    }
}
