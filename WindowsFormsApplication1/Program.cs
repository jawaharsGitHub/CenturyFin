using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;

namespace WindowsFormsApplication1
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

            CultureInfo myCI = new CultureInfo("ta-IN", false);
            myCI.DateTimeFormat.ShortDatePattern = "dd-MMM-yyyy";

            Thread.CurrentThread.CurrentCulture = myCI;


            Application.Run(new frmIndexForm());
        }


    }

}
