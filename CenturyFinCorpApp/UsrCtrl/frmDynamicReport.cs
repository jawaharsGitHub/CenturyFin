using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
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

        private void CreditScore()
        {
            try
            {
                var data = (from c in Customer.GetAllCustomer()
                            where c.IsActive == false
                            select Customer.GetCreditScore(c.CustomerId, c.CustomerSeqNumber)).ToList();

                dgReports.DataSource = (from d in data
                                        group d by new { d.CustomerId, d.Name } into ng
                                        select new
                                        {
                                            ng.ToList().Count,
                                            ng.Key.CustomerId,
                                            ng.Key.Name,
                                            InterestRate = (ng.ToList().Sum(s => s.InterestRate) / ng.ToList().Count).RoundMoney(),
                                            PercGainPerMonth = (ng.ToList().Sum(s => s.PercGainPerMonth) / ng.ToList().Count).RoundMoney(),
                                            InterestPerMonth = (ng.ToList().Sum(s => s.InterestPerMonth) / ng.ToList().Count).RoundMoney(),
                                            DaysTaken = (ng.ToList().Sum(s => s.DaysTaken) / ng.ToList().Count),
                                            MissingDays = ng.ToList().Sum(s => s.MissingDays) / ng.ToList().Count
                                        }).OrderByDescending(o => o.DaysTaken).ToList();

            }
            catch (Exception ex)
            {
                throw ex;
            }
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
            else if (value == 4)
            {
                CreditScore();
            }
        }

        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "NOT GIVEN FOR FEW DAYS"),
                   new KeyValuePair<int, string>(2, "TO BE CLOSED SOON"),
                   new KeyValuePair<int, string>(3, "X-CUSTOMER"),
                   new KeyValuePair<int, string>(4, "CREDIT REPORT")
               };

            return myKeyValuePair;

        }
    }
}
