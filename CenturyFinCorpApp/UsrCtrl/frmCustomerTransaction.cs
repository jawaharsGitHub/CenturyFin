using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmCustomerTransaction : UserControl
    {
        int _balance;
        bool _isClosedTx = false;
        [JsonIgnore]
        private Customer customer;
        private List<Transaction> txns;


        public frmCustomerTransaction()
        {
            InitializeComponent();
        }

        public frmCustomerTransaction(Customer _customer, Form parentForm)
        {
            InitializeComponent();

            customer = _customer;
            _isClosedTx = (customer.IsActive == false);

            _balance = _isClosedTx ? 0 : Transaction.GetBalance(customer);

            btnLoan.Text = $"LOAN :  {customer.LoanAmount}";
            btnBalance.Text = $"BALANCE :  {_balance}";
            btnInterest.Text = $"INTEREST :  {customer.Interest}";

            var closedText = string.Empty;
            if (customer.IsForceClosed)
            {
                closedText = "(FORCE CLOSED)";
            }
            else if (_balance == 0)
            {
                closedText = "(CLOSED)";
            }

            var mergedText = (customer.IsMerged) ? $"(MERGED on {customer.MergedDate.Value.ToShortDateString()})" : string.Empty;


            lblDetail.Text = $"{customer.Name} - CutomerId: {customer.CustomerId} SequenceNo: {customer.CustomerSeqNumber} {closedText} {mergedText}";

            txtCollectionAmount.Text = (customer.LoanAmount / 100).ToString();

            LoadTxn();
            LoadCustomerCollectionType();

            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["TxnDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            }
            lblMessage.Text = string.Empty;

            btnReOpen.Visible = (_balance == 0);

        }

        private void LoadCustomerCollectionType()
        {
            cmbReturnType.DataSource = Enum.GetValues(typeof(ReturnTypeEnum));
            cmbReturnDay.DataSource = Enum.GetValues(typeof(DayOfWeek));

            cmbCollectionSpot.DataSource = Customer.GetAllUniqueCustomers();
            cmbCollectionSpot.ValueMember = "CustomerId";
            cmbCollectionSpot.DisplayMember = "IdAndName";


            cmbReturnType.SelectedItem = customer.ReturnType;
            cmbReturnDay.SelectedItem = customer.ReturnDay;
            cmbCollectionSpot.SelectedValue = customer.CollectionSpotId;

            this.cmbReturnType.SelectedIndexChanged += new System.EventHandler(this.cmbReturnType_SelectedIndexChanged);
            this.cmbReturnDay.SelectedIndexChanged += new System.EventHandler(this.cmbReturnDay_SelectedIndexChanged);
            this.cmbCollectionSpot.SelectedIndexChanged += new System.EventHandler(this.cmbCollectionSpot_SelectedIndexChanged);
        }

        public Transaction AddTxn(Customer cus, DateTime txnDate)
        {
            var txn = new Transaction()
            {
                AmountReceived = Convert.ToInt16(txtCollectionAmount.Text),
                CustomerId = customer.CustomerId,
                CustomerSequenceNo = customer.CustomerSeqNumber,
                TransactionId = Transaction.GetNextTransactionId(),
                Balance = (Transaction.GetBalance(customer) - Convert.ToInt16(txtCollectionAmount.Text)),
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
                AmountReceived = Convert.ToInt32(txtCollectionAmount.Text),
                CustomerId = customer.CustomerId,
                CustomerSequenceNo = customer.CustomerSeqNumber,
                TransactionId = Transaction.GetNextTransactionId(),
                Balance = (Transaction.GetBalance(customer) - Convert.ToInt32(txtCollectionAmount.Text)),
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
                Customer.CloseCustomerTxn(customer, false, txn.TxnDate); //new Customer() { CustomerId = _customerId, CustomerSeqNumber = _sequeneNo, IsActive = false, ClosedDate = txn.TxnDate });
            }

            lblMessage.Text = $"Txn  Added Successfully for {customer.Name}";

        }

        private void AddForceCloseTransaction()
        {

            var txn = new Transaction()
            {
                AmountReceived = 0,
                CustomerId = customer.CustomerId,
                CustomerSequenceNo = customer.CustomerSeqNumber,
                TransactionId = Transaction.GetNextTransactionId(),
                Balance = 0,
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
                Customer.CloseCustomerTxn(customer, false, txn.TxnDate); //new Customer() { CustomerId = _customerId, CustomerSeqNumber = _sequeneNo, IsActive = false, ClosedDate = txn.TxnDate });
            }

            lblMessage.Text = $"Txn  Added Successfully for {customer.Name}";
        }

        private void LoadTxn(bool isDesc = true, bool byBalance = false)
        {

            txns = Transaction.GetTransactionDetails(customer);
            var cus = Customer.GetCustomerDetails(customer);

            // Cross verify txn.
            var totalReceived = txns.Where(w => w.AmountReceived > 0).Sum(s => s.AmountReceived);
            var lastBalance = txns.Last().Balance;
            var expectedBalance = cus.LoanAmount - totalReceived;
            var isCorrect = (expectedBalance == lastBalance);
            btnCorrect.Visible = !isCorrect;

            if (isCorrect == false)
            {
                MessageBox.Show($"Loan: {cus.LoanAmount} Total Received: {totalReceived} Actual Balance: {lastBalance} Expected Balance: {expectedBalance}");
            }

            if (txns == null || txns.Count == 0) return;

            var dataDource = txns;


            if (byBalance)
                dataGridView1.DataSource = dataDource.OrderBy(o => o.Balance).ToList();
            else if (isDesc)
                dataGridView1.DataSource = dataDource.OrderByDescending(o => o.TxnDate.Date).ThenByDescending(t => t.Balance).ToList();
            else
                dataGridView1.DataSource = dataDource.OrderBy(o => o.TxnDate.Date).ThenBy(t => t.Balance).ToList();



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

            double perDayAmount = (cus.LoanAmount / 100);

            double perDayValue = (perDayAmount / 100.0);

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

            var expected = (daysTaken * (customer.LoanAmount / 100)) > customer.LoanAmount ? -1 : (daysTaken * (customer.LoanAmount / 100));
            var actual = customer.LoanAmount - _balance;


            lblNoOfDays.Text = $"Days taken to Return {daysTaken} (Expected {expected} ACTUAL {actual}) - MISSING DAYS: {missing.Count}";

            if (expected == -1)
                lblNoOfDays.ForeColor = System.Drawing.Color.IndianRed;
            else if (actual < expected)
                lblNoOfDays.ForeColor = System.Drawing.Color.Red;
            else if (actual > expected)
                lblNoOfDays.ForeColor = System.Drawing.Color.Green;
            else if (actual == expected)
                lblNoOfDays.ForeColor = System.Drawing.Color.Honeydew;


            var interestRate = cus.Interest == 0 ? 0 : ((cus.LoanAmount / cus.Interest) == 12 ? 8.69 : 11.11);


            var percGainPerMonth = Math.Round(((interestRate / daysTaken) * 30), 2); // 8.89% is % of 800 for 9200 for one month.


            lblPercentageGain.Text = $"{percGainPerMonth.ToString()}% Per Month({(percGainPerMonth / 100) * (cus.LoanAmount - cus.Interest)} Rs/Month)";

            dateTimePicker1.Value = lastDate.AddDays(1);

            dataGridView1.Columns["CustomerId"].Visible = false;
            dataGridView1.Columns["IsClosed"].Visible = false;
            dataGridView1.Columns["TxnUpdatedDate"].Visible = false;
            dataGridView1.Columns["CustomerSequenceNo"].Visible = false;

            dataGridView1.Columns["TxnDate"].DisplayIndex = 1;
            dataGridView1.Columns["AmountReceived"].DisplayIndex = 2;
            dataGridView1.Columns["Balance"].DisplayIndex = 3;

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
                    CustomerId = customer.CustomerId,
                    CustomerSequenceNo = customer.CustomerSeqNumber
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

        private void cmbReturnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: need to resue this code
            customer.ReturnType = (ReturnTypeEnum)Enum.Parse(typeof(ReturnTypeEnum), cmbReturnType.SelectedValue.ToString());
            Customer.UpdateCustomerReturnType(customer);
        }

        private void cmbReturnDay_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: need to resue this code
            customer.ReturnDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), cmbReturnDay.SelectedValue.ToString());
            Customer.UpdateCustomerReturnType(customer);
        }

        private void cmbCollectionSpot_SelectedIndexChanged(object sender, EventArgs e)
        {
            //TODO: need to resue this code
            customer.CollectionSpotId = cmbCollectionSpot.SelectedValue.ToInt32();
            Customer.UpdateCustomerReturnType(customer);
        }

        private void btnCorrect_Click(object sender, EventArgs e)
        {
            CorrectData();
        }

        private void CorrectData()
        {
            txns.ForEach(t =>
            {

                if (txns.Where(w => w.TxnDate.Date == t.TxnDate.Date).Count() == 1)
                {
                    var nextTxn = txns.NextOf(t);

                    if (nextTxn != null && nextTxn.Balance != (t.Balance - nextTxn.AmountReceived))
                    {
                        nextTxn.Balance = (t.Balance - nextTxn.AmountReceived);
                        nextTxn.IsClosed = _isClosedTx;
                        Transaction.CorrectTransactionData(nextTxn);
                    }
                }

            });
            LoadTxn();
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

            if (dataGridView1.SelectedRows.Count == 1 && e.Button == MouseButtons.Right)
            {
                ContextMenuStrip strip = new ContextMenuStrip();

                var selectedRow = dataGridView1.SelectedRows[0];
                strip.Tag = (Transaction)selectedRow.DataBoundItem;

                strip.Items.Add("Delete Txn").Name = "Txn";

                strip.Show(dataGridView1, new System.Drawing.Point(e.X, e.Y));

                strip.ItemClicked += Strip_ItemClicked;

            }
        }

        private void Strip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var txn = (Transaction)((ContextMenuStrip)sender).Tag;

            if (e.ClickedItem.Name == "Txn")
            {
                Transaction.DeleteTransactionDetails(txn);
            }

        }

        private void btnMerge_Click(object sender, EventArgs e)
        {
            // update loan and interest
            customer.LoanAmount += Convert.ToInt32(txtMergeAmount.Text);
            customer.Interest += Convert.ToInt32(txtInterest.Text);

            Customer.MergeCustomerLoanAmount(customer);

            // get first txn
            var firstTxn = txns.Where(w => w.AmountReceived == 0).OrderBy(o => o.TxnDate).First();


            // update loan amount.
            firstTxn.Balance += Convert.ToInt32(txtMergeAmount.Text); // TODO: it may affect daily given amount and amount in hand details. be carefull.

            Transaction.MergeTransactionLoanAmount(firstTxn);
            // call correct data.
            CorrectData();



        }

        private void RefreshData()
        {
            throw new NotImplementedException();
        }

        private void txtMergeAmount_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtMergeAmount.Text)) return;

            var loanAmount = Convert.ToInt32(txtMergeAmount.Text);

            var interest = (loanAmount / 100) * 10;
            txtInterest.Text = interest.ToString();
        }

        private void btnForceClose_Click(object sender, EventArgs e)
        {
            AddForceCloseTransaction();
            customer.Interest = customer.Interest - _balance;
            Customer.ForceCloseCustomer(customer);
        }

        private void btnTopup_Click(object sender, EventArgs e)
        {
            // top up.
            var txn = new Transaction()
            {
                AmountReceived = -Convert.ToInt32(txtTopupAmount.Text), // should be a negative number as this is a top up, NOT A TRANSACTION
                CustomerId = customer.CustomerId,
                CustomerSequenceNo = customer.CustomerSeqNumber,
                TransactionId = Transaction.GetNextTransactionId(),
                Balance = (Transaction.GetBalance(customer) + Convert.ToInt32(txtTopupAmount.Text)),  // should be (+) as this is a top up, NOT A TRANSACTION
                TxnDate = dateTimePicker1.Value,
                IsClosed = _isClosedTx

            };

            if (txn.Balance < 0)
            {
                MessageBox.Show("Please check that ur txn is overpaid. Txn Cancelled");
                return;
            }

            

            btnBalance.Text = txn.Balance.ToString();
            
            //if (txn.Balance == 0)
            //{
            //    MessageBox.Show("This Txn is completed Successfully!");
            //    Customer.CloseCustomerTxn(customer, false, txn.TxnDate); //new Customer() { CustomerId = _customerId, CustomerSeqNumber = _sequeneNo, IsActive = false, ClosedDate = txn.TxnDate });
            //}


            // update interest
            customer.Interest += txtTopupInterest.Text.ToInt32();
            customer.LoanAmount += txtTopupAmount.Text.ToInt32();
            Customer.UpdateCustomerLoanAndInterest(customer);

            TopupCustomer topupcus = new TopupCustomer();

            customer.CopyTo(topupcus);

            topupcus.LoanAmount = txtTopupAmount.Text.ToInt32();
            topupcus.Interest = txtTopupInterest.Text.ToInt32();
            topupcus.AmountGivenDate = txn.TxnDate;


            // Add top up customers.
            TopupCustomer.AddTopupCustomer(topupcus);
            Transaction.AddTransaction(txn);
            LoadTxn();

            lblMessage.Text = $"{topupcus.LoanAmount} Top up Successfull for {customer.Name}";


        }

        private void txtTopupAmount_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtTopupAmount.Text)) return;

            var loanAmount = Convert.ToInt32(txtTopupAmount.Text);

            var interest = (loanAmount / 100) * 10;
            txtTopupInterest.Text = interest.ToString();
        }
    }
}
