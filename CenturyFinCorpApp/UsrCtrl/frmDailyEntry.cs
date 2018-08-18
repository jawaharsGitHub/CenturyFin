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

        public frmDailyEntry()
        {
            InitializeComponent();

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["usingMenu"]) == true)
                button2.Visible = false;
            else
                button2.Visible = true;

            LoadDailyCollection(true);
            LoadAllHsitoryDailyCollections();

            lblOutStanding.Text = Transaction.GetAllOutstandingAmount().ToMoney();
            AdjustColumnOrder();

        }

        private void LoadAllHsitoryDailyCollections()
        {
            var data = BaseClass.ReadFileAsObjects<DailyCollectionDetail>(AppConfiguration.DailyTxnFile);

            var result = (from d in data
                          select
                          new ExtDailyTxn() { Date = d.Date, CollectionAmount = d.CollectionAmount }).ToList();

            dgvAllDailyCollection.DataSource = result;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDailyCollection();
        }

        private void AdjustColumnOrder()
        {
            dataGridView1.Columns["CustomerId"].Visible = false;
            dataGridView1.Columns["CustomerSeqId"].Visible = false;
            dataGridView1.Columns["TxnDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
        }


        private void LoadDailyCollection(bool versionZero = false)
        {
            var txn = new List<Transaction>();

            if (versionZero)
            {
                txn = Transaction.GetDailyCollectionDetails_V0(dateTimePicker1.Value);
            }
            else
            {
                txn = Transaction.GetDailyCollectionDetails(dateTimePicker1.Value);
            }
            var cus = from c in Customer.GetAllCustomer()
                      select new { c.CustomerSeqNumber, c.Name, c.IsActive, c.Interest, c.LoanAmount, c.CustomerId };

            var result = new List<CustomerDailyTxn>();

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
                              AmountReceived = t.AmountReceived,
                              Balance = t.Balance,
                              CustomerId = c.CustomerId,
                              CustomerSeqId = c.CustomerSeqNumber
                          }).Distinct().ToList();
            }

            var amountReceived = result.Sum(s => s.AmountReceived);


            result = result.Where(w => w.AmountReceived != 0).ToList();

            ActualCollection = amountReceived;
            ExpectedCollection = (cus.Where(w => w.IsActive).Sum(s => s.LoanAmount) / 100);

            label1.Text = $"Total Collection is: {amountReceived}";
            label2.Text = $"{result.Count()} (Rs.{amountReceived}) customers paid out of {cus.Count(c => c.IsActive)} (Rs.{ExpectedCollection})";


            //CollectionPerDay.AddObjectsToJson<CollectionPerDay>(Common.AppConfiguration.CollectionPerDay, 
            //   new List<CollectionPerDay>()(new CollectionPerDay() {
            //        Date = dateTimePicker1.Value.Date.ToShortDateString(),
            //        ActualCollection = ActualCollection,
            //        ExpectedCollection = ExpectedCollection}));

            dataGridView1.DataSource = result;
            dataGridView1.Columns["AmountReceived"].ReadOnly = false;
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
                LoadDailyCollection();
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadDailyCollection(true);
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
        
    }


}
