using Common;
using Common.ExtensionMethod;
using DataAccess;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmReport : UserControl
    {
        int outstandingMoney = 0;
        public frmReport()
        {
            InitializeComponent();

            comboBox1.DataSource = ReportOption.GetOptions();

            ShowOutstandingMoney();
            ShowTotalAssetMoney();

        }

        private void ToBeClosedSoon()
        {
            var txn = Transaction.GetTransactionsToBeClosedSoon(100);

            dgReports.DataSource = txn;
            dgReports.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            //dgReports.Columns["CustomerId"].Visible = false;
        }

        private void NotGivenForFewDays()
        {
            var txn = Transaction.GetTransactionsNotGivenForFewDays();

            dgReports.DataSource = txn;
            dgReports.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            dgReports.Columns["TxnDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
        }

        private void ShowOutstandingMoney()
        {
            outstandingMoney = Transaction.GetAllOutstandingAmount();
            lblOutStanding.Text = outstandingMoney.ToMoney();
        }

        private void ShowTotalAssetMoney()
        {
            var inHandAndBank = InHandAndBank.GetAllhandMoney();
            lblTotalAsset.Text = (outstandingMoney + inHandAndBank.InHandAmount + inHandAndBank.InBank).ToMoney();
        }


        private void btnClosedTxn_Click(object sender, System.EventArgs e)
        {

            var json = File.ReadAllText(AppConfiguration.TransactionFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);

            if (list == null || list.Count == 0) return;

            var closedIds = list.Where(w => w.Balance == 0).ToList();

            foreach (var item in closedIds)
            {
                var closedTxn = new List<Transaction>();
                closedTxn.AddRange(list.Where(w => w.CustomerId == item.CustomerId && w.CustomerSequenceNo == item.CustomerSequenceNo));
                // Back up closed txn
                Transaction.AddClosedTransaction(closedTxn);

                // Delete Transactions data
                Transaction.DeleteTransactionDetails(item.CustomerId, item.CustomerSequenceNo);

                // Customer.UpdateCustomerDetails(new Customer() { CustomerId = item.CustomerId, CustomerSeqNumber = item.CustomerSequenceNo, IsActive = false });

            }

        }

        private void btnRefresh_Click(object sender, System.EventArgs e)
        {
            btnClosedTxn.Text = $"Run Closed Txn ({Transaction.GetClosedTxn()})";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((ReportOption)comboBox1.SelectedItem).Value;

            if (value == 1)
            {
                ToBeClosedSoon();
            }
            else if (value == 2)
            {
                NotGivenForFewDays();
            }
        }
    }

    public class ReportOption
    {

        public static List<ReportOption> GetOptions()
        {
            return new List<ReportOption>() {

                new ReportOption() { Value = 1, Name =  "TO BE CLOSED SOON"   },
                new ReportOption() { Value = 2, Name =  "NOT GIVEN FOR FEW DAYS"   }

            };
        }

        public int Value { get; set; }

        public string Name { get; set; }

    }
}
