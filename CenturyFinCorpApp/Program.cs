using System;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Reflection;
using System.Threading;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            //Common.DataCorrection();

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

           CultureInfo myCI = new CultureInfo("en-GB", false);
            myCI.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";

            Thread.CurrentThread.CurrentCulture = myCI;

            string exeFile = (new Uri(Assembly.GetEntryAssembly().CodeBase)).AbsolutePath;
            string exeDir = Path.GetDirectoryName(exeFile);
            string dataFolder = exeDir.Replace("CenturyFinCorpApp\\bin\\Debug", "DataAccess\\Data\\");

            AddOrUpdateAppSettings("SourceFolder", dataFolder);

            //

            //frmPrediction.Predict();

            Application.Run(new frmIndexForm());
        }

        public static void AddOrUpdateAppSettings(string key, string value)
        {
            try
            {
                var configFile = ConfigurationManager.OpenExeConfiguration(ConfigurationUserLevel.None);
                var settings = configFile.AppSettings.Settings;
                if (settings[key] == null)
                {
                    settings.Add(key, value);
                }
                else
                {
                    settings[key].Value = value;
                }
                configFile.Save(ConfigurationSaveMode.Modified);
                ConfigurationManager.RefreshSection(configFile.AppSettings.SectionInformation.Name);
            }
            catch (ConfigurationErrorsException)
            {
                Console.WriteLine("Error writing app settings");
            }
        }


    }

}
