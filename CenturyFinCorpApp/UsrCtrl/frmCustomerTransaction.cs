using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
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
        int daysTaken = 0;


        public frmCustomerTransaction()
        {
            InitializeComponent();
            LoadExistingCustomers();
        }

        private void LoadExistingCustomers()
        {
            cmbExistingCustomer.DataSource = Customer.GetAllCustomer(); // Customer.GetAllCustomer().(d => d.CustomerId).OrderBy(o => o.Name).ToList().Where(w => w.Name.StartsWith("Rab")).ToList();
            cmbExistingCustomer.DisplayMember = "NameAndSeqId";
            cmbExistingCustomer.ValueMember = "CustomerId";
            cmbExistingCustomer.DropDownStyle = ComboBoxStyle.DropDown;
            cmbExistingCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbExistingCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
        }

        public frmCustomerTransaction(Customer _customer, Form parentForm)
        {
            InitializeComponent();
            LoadExistingCustomers();

            customer = _customer;
            _isClosedTx = (customer.IsActive == false);

            _balance = _isClosedTx ? 0 : Transaction.GetBalance(customer);

            btnLoan.Text = $"LOAN :  {customer.LoanAmount.ToMoneyFormat()}";
            btnBalance.Text = $"BALANCE :  {_balance.ToMoneyFormat()}";
            btnInterest.Text = $"INTEREST :  {customer.Interest.ToMoneyFormat()}";

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
            btnCusName.Text = customer.Name;
            txtCollectionAmount.Text = (customer.LoanAmount / 100).ToString();

            LoadTxn();
            LoadCustomerCollectionType();

            if (dataGridView1.Columns.Count > 0)
            {
                dataGridView1.Columns["TxnDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
                dataGridView1.Columns["TxnDate"].HeaderText = "தேதி";
                dataGridView1.Columns["AmountReceived"].HeaderText = "வரவு ரூபாய்";
                dataGridView1.Columns["Balance"].HeaderText = "பாக்கி ரூபாய்";
                //dataGridView1.Columns["TransactionId"].Visible = false;
            }
            lblMessage.Text = string.Empty;

            btnReOpen.Visible = (_balance == 0);

            var perDayInterest = 0.1;  // 100/10



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
            var forceClosedDate = txns.Where(w => w.AmountReceived > 0).Max(m => m.TxnDate);


            var txn = new Transaction()
            {
                AmountReceived = 0,
                CustomerId = customer.CustomerId,
                CustomerSequenceNo = customer.CustomerSeqNumber,
                TransactionId = Transaction.GetNextTransactionId(),
                Balance = 0,
                TxnDate = forceClosedDate, //dateTimePicker1.Value,
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

        private void LoadTxn(bool isDesc = false, bool byBalance = false)
        {

            txns = Transaction.GetTransactionDetails(customer);
            var cus = Customer.GetCustomerDetails(customer);

            // Cross verify txn.
            var totalReceived = txns.Where(w => w.AmountReceived > 0).Sum(s => s.AmountReceived);
            var lastBalance = txns.Last().Balance;
            var expectedBalance = cus.LoanAmount - totalReceived;
            var isCorrect = (expectedBalance == lastBalance);
            btnCorrect.Visible = !isCorrect;

            btnInterestOnly.Text = "int:" + (totalReceived - customer.LoanAmount + lastBalance + customer.Interest + customer.InitialInterest).ToString();


            if (isCorrect == false)
            {
                MessageBox.Show($"Loan: {cus.LoanAmount} Total Received: {totalReceived} Actual Balance: {lastBalance} Expected Balance: {expectedBalance}");
            }

            if (txns == null || txns.Count == 0) return;

            var dataDource = txns;


            if (byBalance)
                dataGridView1.DataSource = dataDource.OrderBy(t => t.TransactionId).ToList();
            else if (isDesc)
                dataGridView1.DataSource = dataDource.OrderByDescending(t => t.TransactionId).ToList();
            else
                dataGridView1.DataSource = dataDource.OrderBy(t => t.TransactionId).ToList();



            var startDate = dataDource.Select(s => s.TxnDate).Min();
            var lastDate = dataDource.Select(s => s.TxnDate).Max();
            daysTaken = (lastBalance == 0) ? lastDate.Date.Subtract(startDate).Days + 2 : DateTime.Now.Date.Subtract(startDate).Days + 2;


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



            CalculateNewBalanceAsOfToday(cus, daysTaken);




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

            dateTimePicker1.Value = GlobalValue.CollectionDate.Value; //lastDate.AddDays(1);

            dataGridView1.Columns["CustomerId"].Visible = false;
            dataGridView1.Columns["IsClosed"].Visible = false;
            dataGridView1.Columns["TxnUpdatedDate"].Visible = false;
            dataGridView1.Columns["CustomerSequenceNo"].Visible = false;

            dataGridView1.Columns["TxnDate"].DisplayIndex = 1;
            dataGridView1.Columns["AmountReceived"].DisplayIndex = 2;
            dataGridView1.Columns["Balance"].DisplayIndex = 3;

        }

        private void CalculateNewBalanceAsOfToday(Customer cus, int daysTaken)
        {

            if (cus.ReturnType == ReturnTypeEnum.NI) return;
            // Balance as per today.
            var perDayInt = 0.1; // 10/100;

            var newIntPerc = daysTaken * perDayInt;
            var newIntNo = newIntPerc * (cus.LoanAmount / 100);
            var intSaved = cus.Interest - newIntNo;

            var newBalance = _balance - intSaved;
            var oldInt = cus.Interest == 0 ? 0 : (cus.LoanAmount / cus.Interest);

            btnClosingBalance.Text = $"old balance: {_balance} \n new balance: {newBalance} \n Savings: {intSaved} \n old Int: {oldInt}% \n new Int: {newIntPerc}";
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

        //private void cmbReturnType_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //TODO: need to resue this code
        //    customer.ReturnType = (ReturnTypeEnum)Enum.Parse(typeof(ReturnTypeEnum), cmbReturnType.SelectedValue.ToString());
        //    Customer.UpdateCustomerReturnType(customer);
        //}

        //private void cmbReturnDay_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //TODO: need to resue this code
        //    customer.ReturnDay = (DayOfWeek)Enum.Parse(typeof(DayOfWeek), cmbReturnDay.SelectedValue.ToString());
        //    Customer.UpdateCustomerReturnType(customer);
        //}

        //private void cmbCollectionSpot_SelectedIndexChanged(object sender, EventArgs e)
        //{
        //    //TODO: need to resue this code
        //    customer.CollectionSpotId = cmbCollectionSpot.SelectedValue.ToInt32();
        //    Customer.UpdateCustomerReturnType(customer);
        //}

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

            if (DialogResult.Yes == MessageBox.Show($"merge  {this.customer.Name}  to {(cmbExistingCustomer.SelectedItem as Customer).Name}", "confirmation", MessageBoxButtons.YesNo))
            {
                // current customer balance = interest = 0
                // closed this customer. with force closed.

                var toMergeCustomer = (cmbExistingCustomer.SelectedItem as Customer);
                // Merge the balance and interest
                Customer.AppendCustomerLoanAmountAndBalance(toMergeCustomer, customer);

                // Delete all customer and txn details.
                Customer.DeleteCustomerDetails(customer.CustomerId, customer.CustomerSeqNumber);
                Transaction.DeleteTransactionDetails(customer.CustomerId, customer.CustomerSeqNumber);



            }


            //// update loan and interest
            //customer.LoanAmount += Convert.ToInt32(txtMergeAmount.Text);
            //customer.Interest += Convert.ToInt32(txtInterest.Text);

            //Customer.MergeCustomerLoanAmount(customer);

            //// get first txn
            //var firstTxn = txns.Where(w => w.AmountReceived == 0).OrderBy(o => o.TxnDate).First();


            //// update loan amount.
            //firstTxn.Balance += Convert.ToInt32(txtMergeAmount.Text); // TODO: it may affect daily given amount and amount in hand details. be carefull.

            //Transaction.MergeTransactionLoanAmount(firstTxn);
            //// call correct data.
            //CorrectData();



        }

        private void RefreshData()
        {
            throw new NotImplementedException();
        }



        private void btnForceClose_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Do you want to close it Forcefully?", "FORCE CLOSE?", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                AddForceCloseTransaction();
                customer.Interest = customer.Interest - _balance;
                Customer.ForceCloseCustomer(customer);
            }
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

            // update interest
            customer.Interest += txtTopupInterest.Text.ToInt32();
            customer.LoanAmount += txtTopupAmount.Text.ToInt32();
            Customer.UpdateCustomerLoanAndInterest(customer);

            TopupCustomer topupcus = new TopupCustomer();
            customer.CopyTo(topupcus);

            topupcus.LoanAmount = txtTopupAmount.Text.ToInt32();
            topupcus.Interest = txtTopupInterest.Text.ToInt32();
            topupcus.AmountGivenDate = dateTimePicker1.Value; //DateTime.Today.Date;
            topupcus.ReturnType = customer.ReturnType;


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

        private void btnConvertToMonthly_Click(object sender, EventArgs e)
        {
            var newAmount = customer.LoanAmount;

            if (string.IsNullOrEmpty(txtNewAmount.Text) == false)
            {
                newAmount = txtNewAmount.Text.ToInt32();
            }


            var newInterest = txtNewInterest.Text.ToInt32();

            if (customer.LoanAmount != newAmount)
            {
                customer.LoanAmount = newAmount;
                Customer.UpdateCustomerLoanAmount(customer);
            }


            if (customer.Interest != newInterest)
            {
                customer.Interest = newInterest;
                Customer.UpdateCustomerInterest(customer);
            }
        }

        private void cmbExistingCustomer_TextChanged(object sender, EventArgs e)
        {
            cmbExistingCustomer.DroppedDown = false;
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var neededRow = (Transaction)((sender as DataGridView).Rows[e.RowIndex]).DataBoundItem;

            var sum = txns.Where(w => w.AmountReceived > 0 && w.TransactionId <= neededRow.TransactionId).Sum(s => s.AmountReceived);

            dataGridView1.Rows[e.RowIndex].Cells["AmountReceived"].ToolTipText = sum.ToString();
        }

        private void btnCapturePic_Click(object sender, EventArgs e)
        {
            //CaptureMyScreen();
            //return;

            // WhatsAppMessage.SendMsg();

            /* 1.Transaction Image*/
            int height = dataGridView1.Height;
            dataGridView1.Height = (dataGridView1.RowCount * dataGridView1.RowTemplate.Height) + 100;
            Bitmap bitmapTxn = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
            dataGridView1.DrawToBitmap(bitmapTxn, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
            //Resize DataGridView back to original height.
            dataGridView1.Height = height;

            //var ImgTxn = $@"E:\{customer.Name}_txn.jpg";
            //Save the Bitmap to folder.
            //bitmapTxn.Save(ImgTxn);

            /* 2.Name Image*/
            Bitmap bitmapName = new Bitmap(this.btnCusName.Width, this.btnCusName.Height);
            btnCusName.DrawToBitmap(bitmapName, new Rectangle(0, 0, this.btnCusName.Width, this.btnCusName.Height));

            //var ImgName = $@"E:\{customer.Name}_Name.jpg";
            //Save the Bitmap to folder.
            //bitmapName.Save(ImgName);


            /* 3.Merge 2 images*/
            Bitmap firstTxn = bitmapTxn;
            Bitmap secondName = bitmapName;
            
            Bitmap result = new Bitmap(Math.Max(firstTxn.Width, secondName.Width), firstTxn.Height + secondName.Height + 30);
            Graphics g = Graphics.FromImage(result);
            g.DrawImageUnscaled(firstTxn, 0, 30);
            g.DrawImageUnscaled(secondName, 0, 0);
            var txnFileName = $@"E:\{customer.Name}.jpg";
            result.Save(txnFileName);

            AppCommunication.SendCustomerTxnEmail(customer.Name, DateTime.Today, txnFileName);
            MessageBox.Show("Mail Send!");
        }

        private void CaptureMyScreen()

        {
            try
            {
                //Creating a new Bitmap object
                Bitmap captureBitmap = new Bitmap(500, 430, PixelFormat.Format32bppArgb);

                //capture our Current Screen
                Rectangle captureRectangle = Screen.AllScreens[0].Bounds;

                //Creating a New Graphics Object
                Graphics captureGraphics = Graphics.FromImage(captureBitmap);
                //Copying Image from The Screen
                captureGraphics.CopyFromScreen(captureRectangle.Left, captureRectangle.Top, 0, -230, captureRectangle.Size);

                //Saving the Image File (I am here Saving it in My E drive).
                captureBitmap.Save($@"E:\{customer.CustomerSeqNumber}_{customer.Name}.jpg", ImageFormat.Jpeg);
                //Displaying the Successfull Result
                MessageBox.Show("Screen Captured");
            }

            catch (Exception ex)

            {

                MessageBox.Show(ex.Message);

            }

        }

        private void button1_Click(object sender, EventArgs e)
        {
            txns = Transaction.GetTransactionDetails(customer);
            var cus = Customer.GetCustomerDetails(customer);

            // Cross verify txn.
            var totalReceived = txns.Where(w => w.AmountReceived > 0).Sum(s => s.AmountReceived);
            var lastBalance = txns.Last().Balance;
            var expectedBalance = cus.LoanAmount - totalReceived;
            var inhandGivenMoney = customer.LoanAmount - customer.InitialInterest;
            // var isCorrect = (expectedBalance == lastBalance);
            // btnCorrect.Visible = !isCorrect;

            var localInt = string.IsNullOrEmpty(txtNewInt.Text) ? customer.InitialInterest : txtNewInt.Text.ToInt32();

            if (localInt <= 0)
            {
                MessageBox.Show("Please provide Iniial Interest");
                txtNewInt.Focus();
            }

            var askedAMount = (inhandGivenMoney + (localInt / 30) * daysTaken) - totalReceived;

            btnNewInt.Text = $"Asked Amt: {askedAMount}";
        }
    }
}
