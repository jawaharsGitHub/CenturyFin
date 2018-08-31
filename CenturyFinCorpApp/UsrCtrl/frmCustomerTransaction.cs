using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmCustomerTransaction : UserControl
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

        public frmCustomerTransaction(Customer _customer, Form parentForm)
        {
            InitializeComponent();

            _customerId = _customer.CustomerId;
            _sequeneNo = _customer.CustomerSeqNumber;
            _loan = _customer.LoanAmount;
            _customerName = _customer.Name;
            _isClosedTx = (_customer.IsActive == false);

            _balance = _isClosedTx ? 0 : Transaction.GetBalance(_loan, _sequeneNo, _customerId);



            btnLoan.Text = $"LOAN :  {_loan}";
            btnBalance.Text = $"BALANCE :  {_balance}";
            var closedText = (_balance == 0) ? "(CLOSED)" : string.Empty;


            lblDetail.Text = $"{_customerName} - CutomerId: {_customerId} SequenceNo: {_sequeneNo} {closedText}";

            txtCollectionAmount.Text = (_loan / 100).ToString();

            btnBalance.Visible = groupBox1.Visible = (_balance > 0);

            LoadTxn();

            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["TxnDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            }
            lblMessage.Text = string.Empty;
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
            if (txn.Balance == 0)
            {
                MessageBox.Show("This Txn is completed Successfully!");
                Customer.UpdateCustomerDetails(new Customer() { CustomerId = _customerId, CustomerSeqNumber = _sequeneNo, IsActive = false, ClosedDate = txn.TxnDate });
            }

            lblMessage.Text = $"Txn  Added Successfully for {_customerName}";

        }

        private void LoadTxn(bool isDesc = true, bool byBalance = false)
        {

            var txns = Transaction.GetTransactionDetails(_customerId, _sequeneNo, _isClosedTx);
            var cus = Customer.GetCustomerDetails(_customerId, _sequeneNo);

            if (txns == null || txns.Count == 0) return;

            var dataDource = txns;


            if (byBalance)
                dataGridView1.DataSource = dataDource.OrderBy(o => o.Balance).ToList();
            else if (isDesc)
                dataGridView1.DataSource = dataDource.OrderByDescending(o => o.TxnDate).ThenBy(t => t.Balance).ToList();
            else
                dataGridView1.DataSource = dataDource.OrderBy(o => o.TxnDate).ToList();

            var lastBalance = txns.Min(m => m.Balance);

            var startDate = dataDource.Select(s => s.TxnDate).Min();
            var lastDate = dataDource.Select(s => s.TxnDate).Max();
            var daysTaken = (lastBalance == 0) ? lastDate.Date.Subtract(startDate).Days + 2 : DateTime.Now.Date.Subtract(startDate).Days + 2;


            // Calculate Credit Score 
            // TODO: Need to move as a seperate method - CalculateCreditScore()

            double _creditScore = 0;
            List<DateTime> col = txns.Select(s => s.TxnDate.Date).ToList();
            var _missingLastDate = _isClosedTx ? lastDate : DateTime.Today.Date;


            var range = (Enumerable.Range(0, (int)(_missingLastDate - startDate).TotalDays + 1)
                                  .Select(i => startDate.AddDays(i).Date)).ToList();

            var perDayAmount = (cus.LoanAmount / 100);

            var perDayValue = (perDayAmount / 100);

            var missing = range.Except(col).ToList();

            
            _creditScore -= missing.Count * perDayValue;  // 1.Missing Days (value = -1)

            
            _creditScore -= (daysTaken > 100) ? ((daysTaken - 100) * 0.75 * perDayValue) : 0; // 2.Above 100 Days (value = -1.5)

            
            // 3.Lumb amount (value = +0.75)
            var lumbCount = (from t in txns
                             where t.AmountReceived > (cus.LoanAmount / 100)
                             select ((t.AmountReceived - perDayAmount) / perDayAmount)).ToList();
            _creditScore += (lumbCount.Sum() * 0.75 * perDayValue);

            
            if (_isClosedTx)    // 4.Number days saved(value = 1)
                _creditScore += (daysTaken < 100) ? ((100 - daysTaken) * 1 * perDayValue) : 0;


            // TOOD: 5.Extra credit score for each transactions from 2nd txns.


            btnCreditScore.Text = $"CREDIT SCORE IS: {Environment.NewLine} {_creditScore}";

            lblStartDate.Text = $"Start Date: {startDate.Date.ToShortDateString()}";
            lblLastDate.Text = $"Last Date: {lastDate.Date.ToShortDateString()}";

            var expected = (daysTaken * (_loan / 100)) > _loan ? -1 : (daysTaken * (_loan / 100));
            var actual = _loan - _balance;


            lblNoOfDays.Text = $"Days taken to Return {daysTaken} (Expected {expected} ACTUAL {actual}) - MISSING DAYS: {missing.Count}";

            if (expected == -1)
                lblNoOfDays.ForeColor = System.Drawing.Color.IndianRed;
            else if (actual < expected)
                lblNoOfDays.ForeColor = System.Drawing.Color.Red;
            else if (actual > expected)
                lblNoOfDays.ForeColor = System.Drawing.Color.Green;
            else if (actual == expected)
                lblNoOfDays.ForeColor = System.Drawing.Color.Honeydew;


            var interestRate = (cus.LoanAmount / cus.Interest) == 12 ? 8.69 : 11.11;


            var percGainPerMonth = Math.Round(((interestRate / daysTaken) * 30), 2); // 8.89% is % of 800 for 9200 for one month.


            lblPercentageGain.Text = $"{percGainPerMonth.ToString()}% Per Month({(percGainPerMonth / 100) * (cus.LoanAmount - cus.Interest)} Rs/Month)";

            dateTimePicker1.Value = lastDate.AddDays(1);

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

        private void chkByBalance_CheckedChanged(object sender, EventArgs e)
        {
            if (chkByBalance.Checked)
            {
                LoadTxn(byBalance: true);
            }
        }
    }
}
