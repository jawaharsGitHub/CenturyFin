using Common;
using System;
using System.Globalization;
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

            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            CultureInfo myCI = new CultureInfo("en-GB", false);
            myCI.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";

            Thread.CurrentThread.CurrentCulture = myCI;

            var dataFolder = General.GetDataFolder("CenturyFinCorpApp\\bin\\Debug", "DataAccess\\Data\\");

            AppConfiguration.AddOrUpdateAppSettings("SourceFolder", dataFolder);

            //

            //frmPrediction.Predict();

            Application.Run(new frmIndexForm());
        }




    }

}
