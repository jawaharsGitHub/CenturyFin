using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.IO;
using Common;
using DataAccess.PrimaryTypes;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmBatches : UserControl
    {
        public frmBatches()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {

            if (Directory.Exists(AppConfiguration.DailyBatchFile) == false)
            {
                Directory.CreateDirectory(AppConfiguration.DailyBatchFile);
            }

            var firstDate = new DateTime(2018, 1, 25);
            var lastDate = DateTime.Today.Date.AddDays(-1);


            while (firstDate <= lastDate)
            {
                // Search active and closed txn file.

                //1. Search Transaction.json
                var activeTxn = Transaction.GetTransactionForDate(lastDate);


                //2. Search Closed Notes.
                var closedTxn = Transaction.GetClosedTransactionForDate(lastDate);

                activeTxn.AddRange(closedTxn);

                Transaction.AddBatchTransactions(activeTxn, $"{lastDate.ToString("dd-MM-yyyy")}");

                // Save daily txn.


                //Transaction.AddBatchTransactions(a)

                // Set to previous day.
                lastDate = lastDate.AddDays(-1);
            }

        }
    }
}
