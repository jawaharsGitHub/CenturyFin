﻿using DataAccess;
using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class frmCustomerTransaction : Form
    {
        int _customerId;
        int _sequeneNo;
        int _loan;
        int _balance;
        string _customerName;
        bool _isClosedTx = false;


        public frmCustomerTransaction()
        {
            InitializeComponent();
        }

        public frmCustomerTransaction(int sequenceNo, int customerId, int loan, string customerName, bool isClosedTx)
        {
            InitializeComponent();

            _customerId = customerId;
            _sequeneNo = sequenceNo;
            _loan = loan;
            _customerName = customerName;
            _isClosedTx = isClosedTx;

            _balance = _isClosedTx ? 0 : Transaction.GetBalance(_loan, _sequeneNo, _customerId);



            btnLoan.Text = _loan.ToString();
            btnBalance.Text = _balance.ToString();
            this.Text = $"{_customerName} - CutomerId: {_customerId} SequenceNo: {_sequeneNo}";
            txtCollectionAmount.Text = (_loan / 100).ToString();

            InitializeListView();
            LoadTxn();
        }

        public Transaction AddTxn(Customer cus, DateTime txnDate)
        {
            var txn = new Transaction()
            {
                AmountReceived = Convert.ToInt16(txtCollectionAmount.Text),
                CustomerId = _customerId,
                CustomerSequenceNo = _sequeneNo,
                TransactionId = Transaction.GetNextTransactionId(),
                Balance = (Transaction.GetBalance(_loan, _sequeneNo, _customerId) - Convert.ToInt16(txtCollectionAmount.Text)),
                TxnDate = dateTimePicker1.Value,
                IsClosed = _isClosedTx

            };

            //var txn = new Transaction()
            //{
            //    TxnCustomer = cus,
            //    TransactionId = Transaction.GetNextTransactionId(),
            //    //Balance = (Transaction.GetBalance(_loan, _sequeneNo, _customerId) - Convert.ToInt16(txtCollectionAmount.Text)),
            //    TxnDate = dateTimePicker1.Value

            //};


            if (txn.Balance < 0)
            {
                MessageBox.Show("Please check that ur txn is overpaid. Txn Cancelled");
                return null;
            }

            Transaction.AddTransaction(txn);
            return txn;

        }

        private void btnAddTxn_Click(object sender, EventArgs e)
        {
            //var txn = AddTxn(_customer, dateTimePicker1.Value);
            //if (txn == null) return;
            var txn = new Transaction()
            {
                AmountReceived = Convert.ToInt16(txtCollectionAmount.Text),
                CustomerId = _customerId,
                CustomerSequenceNo = _sequeneNo,
                TransactionId = Transaction.GetNextTransactionId(),
                Balance = (Transaction.GetBalance(_loan, _sequeneNo, _customerId) - Convert.ToInt16(txtCollectionAmount.Text)),
                TxnDate = dateTimePicker1.Value,
                IsClosed = _isClosedTx

            };

            if (txn.Balance < 0)
            {
                MessageBox.Show("Please check that ur txn is overpaid. Txn Cancelled");
                return;
            }

            Transaction.AddTransaction(txn);

            btnBalance.Text = txn.Balance.ToString();
            LoadTxn();
            if (txn.Balance == 0) MessageBox.Show("This Txn is completed Successfully!");

            lblMessage.Text = $"Txn  Added Successfully for {_customerName}";

            // Add InHand
            InHand.AddInHand(txn.AmountReceived);



        }


        private void InitializeListView()
        {

        }

        private void LoadTxn(bool isDesc = true)
        {

            var txns = Transaction.GetTransactionDetails(_customerId, _sequeneNo, _isClosedTx);
            var cus = Customer.GetCustomerDetails(_customerId, _sequeneNo);

            if (txns == null || txns.Count == 0) return;


            var dataDource = txns; // from t in txns
                             //select new
                             //{
                             //    t.TransactionId,
                             //    t.TxnDate,
                             //    t.AmountReceived,
                             //    t.Balance
                             //};



            if (isDesc)
                dataGridView1.DataSource = dataDource.OrderByDescending(o => o.TxnDate).ThenBy(t => t.Balance).ToList();
            else
                dataGridView1.DataSource = dataDource.OrderBy(o => o.TxnDate).ToList();




            var startDate = dataDource.Select(s => s.TxnDate).Min();
            var lastDate = dataDource.Select(s => s.TxnDate).Max();
            var DaysTaken = lastDate.Date.Subtract(startDate).Days + 2;


            lblStartDate.Text = $"Start Date: {startDate.Date}";
            lblLastDate.Text = $"Last Date: {lastDate.Date}";

            lblNoOfDays.Text = $"Days taken to Return {DaysTaken}";

            var percGainPerMonth = Math.Round(((8.89 / DaysTaken) * 30), 2); // 8.89% is % of 800 for 9200 for one month.


            lblPercentageGain.Text = $"{percGainPerMonth.ToString()}% Per Month({(percGainPerMonth / 100) * (cus.LoanAmount - cus.Interest)} Rs/Month)";

            dateTimePicker1.Value = lastDate.AddDays(1);

            //dataGridView1.ReadOnly = false;

            dataGridView1.Columns["CustomerId"].Visible = false;
            dataGridView1.Columns["IsClosed"].Visible = false;
            dataGridView1.Columns["TxnUpdatedDate"].Visible = false;
            dataGridView1.Columns["CustomerSequenceNo"].Visible = false;

        }

        private void rdbAsc_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAsc.Checked)
            {
                LoadTxn(false);

            }

        }

        private void rdbDesc_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbDesc.Checked)
            {
                LoadTxn(true);

            }

        }

        private void btnNextDayTxn_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value;
            btnAddTxn_Click(null, null);

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (sender as DataGridView);
            var rowIndex = grid.CurrentCell.RowIndex;

            var txnId = GetGridCellValue(grid, rowIndex, "TransactionId");
            var txnDate = GetGridCellValue(grid, rowIndex, "TxnDate");
            var amountReceived = GetGridCellValue(grid, rowIndex, "AmountReceived");
            var balance = GetGridCellValue(grid, rowIndex, "Balance");

            // Update transaction detail.

            Transaction.CorrectTransactionData(
                new Transaction()
                {
                    TransactionId = Convert.ToInt32(txnId),
                    TxnDate = Convert.ToDateTime(txnDate),
                    AmountReceived = Convert.ToInt32(amountReceived),
                    Balance = Convert.ToInt32(balance),
                    IsClosed = _isClosedTx,
                    CustomerId = _customerId,
                    CustomerSequenceNo = _sequeneNo
                });

        }

        private string GetGridCellValue(DataGridView grid, int rowIndex, string columnName)
        {
            return Convert.ToString(grid.Rows[grid.CurrentCell.RowIndex].Cells[columnName].Value);
        }


    }
}