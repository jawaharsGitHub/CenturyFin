using Common;
using DataAccess.PrimaryTypes;
using System;
using System.Globalization;
using System.Threading;
using System.Windows.Forms;
using System.Linq;
using System.Text;
using Newtonsoft.Json;

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

            if (AppConfiguration.AddOrUpdateAppSettings("SourceFolder", dataFolder))
            {
                LogHelper.WriteLog($"started application");

                // As of 27-1ug-2018
                /*
                var details = (from c in Customer.GetAllCustomer()
                               select new {
                                   c.CustomerId,
                                   c.CustomerSeqNumber,
                                   c.Name ,
                                   c.LoanAmount,
                                   txn = Transaction.GetTransactionDetails(c.CustomerId, c.CustomerSeqNumber, (c.IsActive == false)) }).ToList();

                var data = (from t in details
                            select new {
                                t.CustomerId,
                                t.CustomerSeqNumber,
                                t.Name,
                                t.LoanAmount,
                                TotalReceived = t.txn.Sum(s => s.AmountReceived),
                                MinBalance = t.txn.Min(m => m.Balance),
                                IsCorrect =  ((t.LoanAmount - t.txn.Sum(s => s.AmountReceived)) == t.txn.Min(m => m.Balance))
                            }
                            ).ToList();

                StringBuilder sb = new StringBuilder();

                string jsonString = JsonConvert.SerializeObject(data, Formatting.Indented);
                */




                Application.Run(new frmIndexForm());
                
            }

            //

            //frmPrediction.Predict();


        }




    }

}
