using System;
using System.Configuration;
using System.IO;

namespace Common
{
    public static class AppConfiguration
    {

        public static string CustomerFile { get; } = GetFullPath("CustomerFile"); 

        public static string TransactionFile { get; } = GetFullPath("TransactionFile"); 

        public static string InvestmentFile { get; } = GetFullPath("InvestmentFile"); 

        public static string ClosedNotesFile { get; } = GetFullPath("ClosedNotesFile"); 

        public static string ExpenditureFile { get; } = GetFullPath("ExpenditureFile"); 

        public static string InHandFile { get; } = GetFullPath("InHandAndBankFile"); 

        public static string DailyTxnFile { get; } = GetFullPath("DailyTxn"); 

        public static bool usingMenu { get; } = Convert.ToBoolean(ConfigurationManager.AppSettings["usingMenu"]);

        public static string CollectionPerDay { get; } = GetFullPath("CollectionPerDay");

        public static string DailyBatchFile { get; } = GetFullPath("DailyBatchFile");

        private static string GetFullPath(string configKey)
        {
            return Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings[configKey]);

        }
    }
}
