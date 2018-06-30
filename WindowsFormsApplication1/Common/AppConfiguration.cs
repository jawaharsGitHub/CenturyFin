using System.Configuration;
using System.IO;

namespace Common
{
    public static class AppConfiguration
    {

        public static string CustomerFile { get; } = Path.Combine(ConfigurationManager.AppSettings["SourceFolder"],ConfigurationManager.AppSettings["CustomerFile"]);

        public static string TransactionFile { get; } = Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["TransactionFile"]);

        public static string InvestmentFile { get; } = Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["InvestmentFile"]);

        public static string BackupFolderPath { get; } = Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["BackupFolderPath"]); 

        public static string ExpenditureFile { get; } = Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["ExpenditureFile"]); 

        public static string InHandFile { get; } = Path.Combine(ConfigurationManager.AppSettings["SourceFolder"], ConfigurationManager.AppSettings["InHandFile"]);
    }
}
