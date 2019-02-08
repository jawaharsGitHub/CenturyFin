using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmDailyEntry : UserControl
    {

        private int ActualCollection;
        private int ExpectedCollection;

        private List<CustomerDailyTxn> result;

        public frmDailyEntry()
        {
            InitializeComponent();


            dateTimePicker1.Value = GlobalValue.CollectionDate.Value;

            LoadDailyCollection(dateTimePicker1.Value, true);
            LoadAllHsitoryDailyCollections();

            //lblOutStanding.Text = Transaction.GetAllOutstandingAmount().ToMoney();
            AdjustColumnOrder();

        }

        private void LoadAllHsitoryDailyCollections()
        {
            var data = BaseClass.ReadFileAsObjects<DailyCollectionDetail>(AppConfiguration.DailyTxnFile);

            var result = (from d in data
                          //join c in Customer.GetAllCustomer()
                          //on Convert.ToDateTime(d.Date) equals c.ClosedDate
                          select
                          new ExtDailyTxn()
                          {
                              Date = Convert.ToDateTime(d.Date).ToString("dd-MM-yyyy dddd"),
                              CollectionAmount = d.CollectionAmount,
                              // ExpectedCollectionAmount = LoadDailyCollection(Convert.ToDateTime(d.Date), true) // TODO: will use when we want it.
                              //Closed = cus.Where(w => w.ClosedDate == Convert.ToDateTime(d.Date)).Sum(s => s.Interest)
                          }).ToList();

            var max = result.OrderBy(o => o.CollectionAmount).Last();


            lblMax.Text = $"Max Cxn - {max.CollectionAmount.ToMoney()} on {max.Date}";

            dgvAllDailyCollection.DataSource = result;

            // Customer Collectin Average By Day.
            var averagePerDay = (from r in result
                                 group r by Convert.ToDateTime(r.Date).DayOfWeek into newGroup
                                 select new
                                 {
                                     Day = newGroup.Key,
                                     Avg = newGroup.Average(a => a.CollectionAmount).RoundMoneyOnly()

                                 }).OrderByDescending(o => o.Avg).ToList();


            dgAvgPerDay.DataSource = averagePerDay;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDailyCollection(dateTimePicker1.Value);
        }

        private void AdjustColumnOrder()
        {
            dataGridView1.Columns["CustomerId"].Visible = false;
            dataGridView1.Columns["CustomerSeqId"].Visible = false;
            dataGridView1.Columns["Interest"].Visible = false;
            dataGridView1.Columns["TxnDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
        }


        private int? LoadDailyCollection(DateTime chooseDate, bool versionZero = false)
        {
            var txn = new List<Transaction>();

            if (versionZero)
            {
                txn = Transaction.GetDailyCollectionDetails_V0(chooseDate);
            }
            else
            {
                txn = Transaction.GetDailyCollectionDetails(chooseDate);
            }


            var cus = from c in Customer.GetAllCustomer()
                      where c.AmountGivenDate.Value.Date <= chooseDate.Date && (c.ClosedDate == null || c.ClosedDate.Value.Date >= chooseDate.Date)
                      select new
                      {
                          c.CustomerSeqNumber,
                          c.Name,
                          CS = c.CollectionSpotId,
                          c.IsActive,
                          c.Interest,
                          c.LoanAmount,
                          c.CustomerId,
                          c.AmountGivenDate,
                          c.ReturnType
                      };


            if (txn != null)
            {
                result = (from t in txn
                          join c in cus
                          on t.CustomerSequenceNo equals c.CustomerSeqNumber
                          select new CustomerDailyTxn
                          {
                              TransactionId = t.TransactionId,
                              TxnDate = t.TxnDate,
                              CustomerName = c.Name,
                              Loan = c.LoanAmount,
                              CSId = c.CS,
                              AmountReceived = t.AmountReceived,
                              Balance = t.Balance,
                              CustomerId = c.CustomerId,
                              CustomerSeqId = c.CustomerSeqNumber,
                              Interest = c.Interest,
                              ReturnType = c.ReturnType
                          }).Distinct().ToList();
            }

            var amountReceived = result.Sum(s => s.AmountReceived);

            ActualCollection = amountReceived;
            ExpectedCollection = (cus.Where(w => w.AmountGivenDate.Value.Date != chooseDate.Date && w.ReturnType != ReturnTypeEnum.Monthly).Sum(s => s.LoanAmount) / 100);

            label1.Text = $"Total Collection is: {amountReceived.ToMoney()}";

            label2.Text = $"{result.Count(c => c.AmountReceived > 0)} (Rs.{amountReceived.ToMoney()}) customers paid out of {cus.Count()} (Rs.{ExpectedCollection.ToMoney()}) {Environment.NewLine}" +
                $"CLOSED:{result.Count(c => c.Balance == 0)} ({result.Where(w => w.Balance == 0).Sum(s => s.Interest).ToMoney()}) NEW:{result.Count(c => c.AmountReceived == 0)} ({result.Where(w => w.AmountReceived == 0).Sum(s => s.Interest).ToMoney()})";


            dataGridView1.DataSource = result;
            dataGridView1.Columns["AmountReceived"].ReadOnly = false;
            dataGridView1.Columns["AmountReceived"].DisplayIndex = 1;
            dataGridView1.Columns["Balance"].DisplayIndex = 2;

            lblCollSpot.Text = $"Went to {result.GroupBy(g => g.CSId).Count()} place to collect?";

            return ExpectedCollection;
        }

        private void LoadNotGivenCustomer()
        {
            var txn = Transaction.GetDailyCollectionDetails(dateTimePicker1.Value).Select(s => s.CustomerSequenceNo).ToList();
            var activeCustomers = Customer.GetAllCustomer().Where(w => w.IsActive == true).ToList();

            var cus = (from c in activeCustomers
                       where txn.Contains(c.CustomerSeqNumber) == false
                       select new { c.CustomerId, c.Name, c.IsActive, c.LoanAmount, c.Interest }).ToList();

            label2.Text = $"{cus.Count()} customers NOT PAID out of {activeCustomers.Count} (MISSING AMOUNT: Rs.{(cus.Sum(s => s.LoanAmount) / 100)})";

            dataGridView1.DataSource = cus;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmInHand fd = new frmInHand();
            //fd.ShowDialog();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedRows = (sender as DataGridView).SelectedRows;

            if (selectedRows.Count != 1) return;

            var selectedCustomer = (selectedRows[0].DataBoundItem as dynamic);

            var mainForm = (frmIndexForm)(((DataGridView)sender).Parent.Parent.Parent); //new frmIndexForm(true);

            mainForm.ShowForm(selectedCustomer.TransactionId);

        }

        private void chkNotGivenCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNotGivenCustomer.Checked)
            {
                // Load Not Given CUstomer;
                dataGridView1.DataSource = null;
                LoadNotGivenCustomer();
            }
            else
            {
                LoadDailyCollection(dateTimePicker1.Value);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadDailyCollection(dateTimePicker1.Value, true);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (sender as DataGridView);
            var rowIndex = grid.CurrentCell.RowIndex;

            var seqNo = FormGeneral.GetGridCellValue(grid, rowIndex, "CustomerSeqId");
            var customerId = FormGeneral.GetGridCellValue(grid, rowIndex, "CustomerId");
            var txnId = FormGeneral.GetGridCellValue(grid, rowIndex, "TransactionId");
            var amountReceived = FormGeneral.GetGridCellValue(grid, rowIndex, "AmountReceived");

            Transaction.UpdateTransactionDetails(new Transaction()
            {
                AmountReceived = amountReceived.ToInt32(),
                TransactionId = txnId.ToInt32(),
                CustomerId = customerId.ToInt32(),
                CustomerSequenceNo = seqNo.ToInt32()
            });

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
            LoadDailyCollection(dateTimePicker1.Value, true);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);
            LoadDailyCollection(dateTimePicker1.Value, true);
        }


        // Shows the sum of previous txns of the selected txns.
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var neededRow = (CustomerDailyTxn)((sender as DataGridView).Rows[e.RowIndex]).DataBoundItem;

            var sum = result.Where(w => w.TransactionId <= neededRow.TransactionId).Sum(s => s.AmountReceived);

            dataGridView1.Rows[e.RowIndex].Cells["AmountReceived"].ToolTipText = sum.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) == false)
            {
                var searchResult = (from r in result
                                    where r.CustomerName.ToLower().Contains(txtSearch.Text.ToLower())
                                    select r).ToList();

                dataGridView1.DataSource = searchResult;

            }
            else
            {
                dataGridView1.DataSource = result;
            }
        }
    }


}
