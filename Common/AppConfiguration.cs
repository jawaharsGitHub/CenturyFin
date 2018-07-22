using System;
using System.Configuration;
using System.IO;

namespace Common
{
    public static class AppConfiguration
    {

        public static string CustomerFile { get; } = GetFullPath("CustomerFile"); //Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["CustomerFile"]);

        public static string TransactionFile { get; } = GetFullPath("TransactionFile"); // Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["TransactionFile"]);

        public static string InvestmentFile { get; } = GetFullPath("InvestmentFile"); //Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["InvestmentFile"]);

        public static string BackupFolderPath { get; } = GetFullPath("BackupFolderPath"); //Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["BackupFolderPath"]);

        public static string ExpenditureFile { get; } = GetFullPath("ExpenditureFile"); //Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["ExpenditureFile"]);

        public static string InHandFile { get; } = GetFullPath("InHandAndBankFile"); //Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["InHandAndBankFile"]);

        public static string DailyTxnFile { get; } = GetFullPath("DailyTxn"); //Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["DailyTxn"]);

        public static bool usingMenu { get; } = Convert.ToBoolean(ConfigurationManager.AppSettings["usingMenu"]);


        private static string GetFullPath(string configKey)
        {
            return Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings[configKey]);

        }
    }
}
