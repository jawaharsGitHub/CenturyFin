using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmDynamicReport : UserControl
    {
        int outstandingMoney = 0;
        public frmDynamicReport()
        {
            InitializeComponent();

            comboBox1.DataSource = GetOptions();

            ShowOutstandingMoney();
            ShowTotalAssetMoney();

        }

        private void ToBeClosedSoon()
        {
            var txn = Transaction.GetTransactionsToBeClosedSoon(100);

            dgReports.DataSource = txn;
            dgReports.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
        }

        private void NotGivenForFewDays()
        {
            var txn = Transaction.GetTransactionsNotGivenForFewDays();

            dgReports.DataSource = txn;
            dgReports.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            dgReports.Columns["LastTxnDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
        }

        private void XCustomer()
        {
            // get all active customers
            var activeCus = Customer.GetAllCustomer().Where(w => w.IsActive).ToList();

            var xCus = Customer.GetAllCustomer().Where(w => activeCus.Select(s => s.CustomerId).Contains(w.CustomerId) == false && w.IsActive == false).ToList();

            dgReports.DataSource = xCus.Select(s => new { s.Name, s.CustomerId }).Distinct().ToList();


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
            }

        }

        private void btnRefresh_Click(object sender, System.EventArgs e)
        {
            btnClosedTxn.Text = $"Run Closed Txn ({Transaction.GetClosedTxn()})";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;

            if (value == 1)
            {
                NotGivenForFewDays();
            }
            else if (value == 2)
            {
                ToBeClosedSoon();
            }
            else if (value == 3)
            {
                XCustomer();
            }
            
        }

        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "NOT GIVEN FOR FEW DAYS"),
                   new KeyValuePair<int, string>(2, "TO BE CLOSED SOON"),
                   new KeyValuePair<int, string>(3, "X-CUSTOMER")
               };

            return myKeyValuePair;

        }

        private void dgReports_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var value = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;

            if (value == 1)
            {
                if (this.dgReports.Columns["NotGivenFor"] == null) return;
                if (e.RowIndex >= 0 && e.ColumnIndex == this.dgReports.Columns["NotGivenFor"].Index)
                {
                    if (e.Value != null)
                    {
                        int dayNotGiven = Convert.ToInt32(e.Value);
                        if (dayNotGiven >= 15)
                        {
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Orange;
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (dayNotGiven >= 7)
                        {
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Black;
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                        }
                    }
                }
            }

            else if (value == 2)
            {
                if (this.dgReports.Columns["NeedToClose"] == null) return;
                if (e.RowIndex >= 0 && e.ColumnIndex == this.dgReports.Columns["NeedToClose"].Index)
                {
                    if (e.Value != null)
                    {
                        int needToClose = Convert.ToInt32(e.Value);
                        if (needToClose == 0)
                        {
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Black;
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (needToClose <= 7)
                        {
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Orange;
                        }
                    }
                }
            }
        }
    }
}
