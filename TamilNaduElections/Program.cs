using Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace TamilNaduElections
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

            var dataFolder = General.GetDataFolder("CenturyFinCorpApp\\bin\\Debug", "TamilNaduElections\\DB\\");

            AppConfiguration.AddOrUpdateAppSettings("SourceFolder", dataFolder);


            Application.Run(new Form1());
        }
    }
}
