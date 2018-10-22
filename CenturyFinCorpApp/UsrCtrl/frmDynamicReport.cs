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

        public frmDynamicReport()
        {
            InitializeComponent();

            comboBox1.DataSource = GetOptions();


            RefreshClosed();

        }

        private void RefreshClosed()
        {
            btnClosedTxn.Text = $"Run Closed Txn ({Transaction.GetClosedTxn()})";
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

            RefreshClosed();

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
                CustomerCollectionSpot();
            }
            else if (value == 5)
            {
                AmountGroups();
            }
            else if (value == 6)
            {
                InterestGroups();
            }
            else if (value == 7)
            {
                OverdueReports();
            }

        }

        private void OverdueReports()
        {
            var overdueDays = (from c in Customer.GetAllCustomer()
                               where c.IsActive && (DateTime.Today.Date - c.AmountGivenDate.Value.Date).TotalDays > 100
                               select new
                               {
                                   c.Name,
                                   c.CustomerSeqNumber,
                                   (DateTime.Today.Date - c.AmountGivenDate.Value.Date).TotalDays,
                                   Balance = Transaction.GetBalance(c)

                               }).OrderByDescending(O => O.TotalDays).ToList();

            dgReports.DataSource = overdueDays;
        }

        private void InterestGroups()
        {
            var cus = Customer.GetAllCustomer().Where(w => w.IsActive == false).ToList();

            var groupsByInterest = (from c in cus
                                    group c by c.CustomerId into newGroup
                                    select new
                                    {
                                        CustomerId = newGroup.Key,
                                        Name = newGroup.First().Name,
                                        TotalInterest = newGroup.Sum(s => s.Interest),
                                        Count = newGroup.Count()
                                    }).OrderByDescending(o => o.TotalInterest).ToList();

            dgReports.DataSource = groupsByInterest;

        }

        private void AmountGroups()
        {
            var cus = Customer.GetAllCustomer().ToList();

            var groupsByAmount = cus.GroupBy(c => c.LoanAmount)
                                .Select(g => new
                                {
                                    Amount = g.Key,
                                    ActiveCount = g.Where(c => c.IsActive == true).Count(),
                                    ActiveTotal = g.Where(c => c.IsActive == true).Sum(s => s.LoanAmount),
                                    ClosedCount = g.Where(c => c.IsActive == false).Count(),
                                    ClosedTotal = g.Where(c => c.IsActive == false).Sum(s => s.LoanAmount),
                                    AllCount = g.Count(),
                                    AllTotal = g.Sum(s => s.LoanAmount),
                                })
                                .OrderByDescending(o => o.AllTotal).ToList();

            dgReports.DataSource = groupsByAmount;

        }

        private void CustomerCollectionSpot()
        {

            var cus = Customer.GetAllCustomer().Where(w => w.IsActive).ToList();
            //cus.ForEach(c =>
            //{
            //    if (c.CollectionSpotId == 0) c.CollectionSpotId = c.CustomerId;
            //});

            // get all active customers
            var activeCus = (from c in cus
                             group c by c.CollectionSpotId into newGroup
                             select new
                             {
                                 Spot = cus.Where(w => w.CustomerId == newGroup.Key).First().Name,
                                 Count = newGroup.Count(),
                                 Amount = newGroup.Sum(s => (s.LoanAmount / 100)),
                                 Customers = string.Join($", {Environment.NewLine}", newGroup.Select(i => $"{i.CustomerId}-{i.Name}"))
                             }).OrderByDescending(o => o.Count);

            dgReports.DataSource = activeCus.Where(w => w.Count > 1).ToList();
            var singleCount = activeCus.Where(w => w.Count == 1);
            var multipleCount = activeCus.Where(w => w.Count > 1);

            lblDetails.Text = $"Single Collection Spot {singleCount.Count()} ({singleCount.Sum(s => s.Amount)}) {Environment.NewLine} " +
                $"Multi Collection Spot {multipleCount.Count()} ({multipleCount.Sum(s => s.Amount)}) {Environment.NewLine}" +
                $"Total Spots {singleCount.Count() + multipleCount.Count()} ({singleCount.Sum(s => s.Amount) + multipleCount.Sum(s => s.Amount)})";


        }


        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "NOT GIVEN FOR FEW DAYS"),
                   new KeyValuePair<int, string>(2, "TO BE CLOSED SOON"),
                   new KeyValuePair<int, string>(3, "X-CUSTOMER"),
                   new KeyValuePair<int, string>(4, "CUSTOMER-COLLECTION SPOT"),
                   new KeyValuePair<int, string>(5, "AMOUNT-GROUPS"),
                   new KeyValuePair<int, string>(6, "INTEREST-GROUPS"),
                   new KeyValuePair<int, string>(7, "OVERDUE-CUSTOMER")
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
