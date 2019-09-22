using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmCustomers : UserControl
    {
        private List<Customer> customers;

        public frmCustomers()
        {
            InitializeComponent();

            cmbFilters.DataSource = GetOptions();
            SetReturnTypes();

            // Get the table from the data set
            SetCustomers();

            if (customers == null) return;

            //Create New DataGridViewTextBoxColumn
            DataGridViewTextBoxColumn textboxColumn = new DataGridViewTextBoxColumn();
            textboxColumn.HeaderText = "Cxn Amount";
            textboxColumn.Name = "CollectionAmt";
            textboxColumn.Width = 50;

            dataGridView1.DataSource = customers;

            //Add TextBoxColumn dynamically to DataGridView
            dataGridView1.Columns.Add(textboxColumn);
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            AdjustColumnOrder();
            chkAllColumns.Checked = false;


            switch (GlobalValue.NoteOption)
            {
                case "AN":
                    rdbAll.Checked = true;
                    break;
                case "CN":
                    rdbClosed.Checked = true;
                    break;
                case "RL":
                default:
                    rdbActive.Checked = true;
                    break;
            }


            txtSearch.Text = GlobalValue.SearchText;
            cmbFilters.SelectedIndex = GlobalValue.SortingByValue;
            cmbReturnTypes.SelectedIndex = GlobalValue.ReturnTypeValue;
            chkFriends.Checked = GlobalValue.FriendAlsoValue;


            GlobalValue.CollectionDate = DailyCollectionDetail.GetLatesttDailyTxnDate();

            dateTimePicker1.Value = GlobalValue.CollectionDate.Value;


        }


        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "By Amount"),
                   new KeyValuePair<int, string>(2, "By Sequence No"),
                   new KeyValuePair<int, string>(3, "By Customer Id"),
                   new KeyValuePair<int, string>(4, "By Customer Name"),
                   new KeyValuePair<int, string>(10, "Return By Yesterday"),
                   new KeyValuePair<int, string>(5, "Return By Today"),
                   new KeyValuePair<int, string>(6, "Return By Tomorrow"),
                   new KeyValuePair<int, string>(7, "By Return Day"),
                   new KeyValuePair<int, string>(8, "By Return Type"),
                   new KeyValuePair<int, string>(9, "By CollectionSpot"),
                   new KeyValuePair<int, string>(10, "By AdjustedAmount"),
                   new KeyValuePair<int, string>(11, "Not Eligible Members"),
                   new KeyValuePair<int, string>(12, "Need Investigation Members"),
                   new KeyValuePair<int, string>(13, "Only Note With us")

               };

            return myKeyValuePair;

        }

        public void SetReturnTypes()
        {
            cmbReturnTypes.DataSource = Enum.GetValues(typeof(ReturnTypeEnum));
        }


        private void SetCustomers()
        {
            customers = Customer.GetAllCustomer()
                        .OrderByDescending(o => o.AmountGivenDate).ToList();

            var monthlyCustomers = Customer.GetAllCustomer()
                        .Where(w => w.IsMonthly())
                        .Select(s => new { Balance = Transaction.GetBalance(s), s.IsActive, s.Interest, s.LoanAmount, s.MonthlyInterest });

            var allGivenAmount = customers.Where(w => w.IsNotMonthly()).Sum(s => s.LoanAmount);

            var NoInterestactiveTxn = customers.Count(c => c.IsActive == true && c.Interest == 0);
            var monthlyINtTxn = customers.Count(c => c.IsActive == true && c.IsMonthly());
            var activeTxn = customers.Count(c => c.IsActive == true && c.Interest > 0 && c.IsNotMonthly());
            var closedTxn = customers.Count(c => c.IsActive == false);
            var totalTxn = activeTxn + closedTxn + monthlyINtTxn;

            this.Text = $"WELCOME TO Running Notes: {activeTxn} Closed Notes: {closedTxn} Total Notes: {totalTxn}";

            rdbActive.Text = $"RUNNING NOTES ({activeTxn + NoInterestactiveTxn + monthlyINtTxn}) {Environment.NewLine}({activeTxn}R + {NoInterestactiveTxn}NI + {monthlyINtTxn}MI)";
            rdbClosed.Text = $"CLOSED NOTES ({closedTxn})";
            rdbAll.Text = $"ALL NOTES ({totalTxn})";

            // Customer day and count ratio.
            var days = (DateTime.Today - new DateTime(2018, 1, 25)).TotalDays;

            var activeMonthlyCustomers = monthlyCustomers.Where(s => s.IsActive).ToList();
            var closedMonthlyCustomers = monthlyCustomers.Where(s => s.IsActive == false).ToList();

            var myData = new
            {
                NewLine = Environment.NewLine,
                TotalNotes = $"{totalTxn} notes in {days} days",
                DaysToMonth = $"{DateHelper.DaysToMonth("Running Days", new DateTime(2018, 1, 25), DateTime.Today)}",
                NotesPerDay = $"{Math.Round(totalTxn / days, 2)} note(s) per day",
                ClosedNotesPerDay = $"{Math.Round(closedTxn / days, 2)} closed note(s) per day",
                Data1 = $"{closedMonthlyCustomers.Sum(s => s.Interest).ToMoneyFormat()}(C) [{closedMonthlyCustomers.Sum(s => s.LoanAmount).ToMoneyFormat()}] + {activeMonthlyCustomers.Sum(s => s.Interest).ToMoneyFormat()}(A) [{activeMonthlyCustomers.Sum(s => s.Balance).ToMoneyFormat()}] = {monthlyCustomers.Sum(s => s.Interest).ToMoneyFormat()} out of ({monthlyCustomers.Sum(s => s.LoanAmount).ToMoneyFormat()}).",
                Data2 = $"{Math.Round(allGivenAmount / days).TokFormat()} Rs. per day ({((Math.Round(allGivenAmount / days) / 10) * 30).TokFormat()} per month)",
                Data3 = activeMonthlyCustomers.Sum(s => s.MonthlyInterest),
                Data4 = monthlyCustomers.Sum(s => s.Interest)
            };

            label1.Text = $"{myData.TotalNotes} {Environment.NewLine} " +
              $"{myData.DaysToMonth} {Environment.NewLine} " +
              $"{myData.NotesPerDay} {Environment.NewLine} " +
              $"{myData.ClosedNotesPerDay} {Environment.NewLine} " +
              $"{myData.Data1}{Environment.NewLine} " +
              $"{myData.Data2} Monthly - {myData.Data3.TokFormat()}  {Environment.NewLine}" +
              $"  need {365 - totalTxn} in {365 - days} days [Shortage: {days - totalTxn}] {Environment.NewLine} " +
              $"{DateHelper.DaysToMonth(" Days Left", DateTime.Today, new DateTime(2019, 1, 24))}";


            //label1.Text = $"{totalTxn} notes in {days} days {Environment.NewLine} " +
            //    $"{DateHelper.DaysToMonth("Running Days", new DateTime(2018, 1, 25), DateTime.Today)} {Environment.NewLine} " +
            //    $"{Math.Round(totalTxn / days, 2)} note(s) per day {Environment.NewLine} " +
            //    $"{Math.Round(closedTxn / days, 2)} closed note(s) per day {Environment.NewLine} " +
            //    $"{closedMonthlyCustomers.Sum(s => s.Interest).ToMoney()}(C) [{closedMonthlyCustomers.Sum(s => s.LoanAmount).ToMoney()}] + {activeMonthlyCustomers.Sum(s => s.Interest).ToMoney()}(A) [{activeMonthlyCustomers.Sum(s => s.Balance).ToMoney()}] = {monthlyCustomers.Sum(s => s.Interest).ToMoney()} out of ({monthlyCustomers.Sum(s => s.LoanAmount).ToMoney()}).{Environment.NewLine} " +
            //    $"{Math.Round(allGivenAmount / days).TokFormat()} Rs. per day ({((Math.Round(allGivenAmount / days) / 10) * 30).TokFormat()} per month) {Environment.NewLine}" +
            //    $"  need {365 - totalTxn} in {365 - days} days [Shortage: {days - totalTxn}] {Environment.NewLine} " +
            //    $"{DateHelper.DaysToMonth(" Days Left", DateTime.Today, new DateTime(2019, 1, 24))}";
        }

        private void AdjustColumnOrder()
        {
            dataGridView1.Columns["CollectionAmt"].DisplayIndex = 4;
            dataGridView1.Columns["ReturnDay"].DisplayIndex = 4;
            dataGridView1.Columns["ReturnType"].DisplayIndex = 5;
            dataGridView1.Columns["CollectionSpotId"].DisplayIndex = 6;
            dataGridView1.Columns["AdjustedAmount"].DisplayIndex = 3;
            dataGridView1.Columns["AmountGivenDate"].DisplayIndex = 8;
            dataGridView1.Columns["ClosedDate"].DisplayIndex = 9;

            dataGridView1.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            dataGridView1.Columns["ClosedDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            dataGridView1.Columns["Name"].Width = 250;
            dataGridView1.Columns["AdjustedAmount"].Width = 50;
            dataGridView1.Columns["LoanAmount"].Width = 50;
            dataGridView1.Columns["Interest"].Width = 50;
            dataGridView1.Columns["MonthlyInterest"].Width = 50;
            dataGridView1.Columns["AmountGivenDate"].Width = 80;
            dataGridView1.Columns["ClosedDate"].Width = 80;
            dataGridView1.Columns["IsActive"].Width = 50;
            dataGridView1.Columns["CustomerId"].Width = 50;
            dataGridView1.Columns["NeedInvestigation"].Width = 50;

            //dataGridView1.Columns["ClosedDate"].HeaderText. = 9;
        }

        private void SetColumnVisibility(bool show = false)
        {
            dataGridView1.Columns["ModifiedDate"].Visible = false;
            dataGridView1.Columns["PhoneNumber"].Visible = false;
            dataGridView1.Columns["IsMerged"].Visible = false;
            dataGridView1.Columns["MergedDate"].Visible = false;
            dataGridView1.Columns["IdAndName"].Visible = false;
            dataGridView1.Columns["IsForceClosed"].Visible = false;
            dataGridView1.Columns["NameAndSeqId"].Visible = false;
            dataGridView1.Columns["NameAndId"].Visible = false;
            dataGridView1.Columns["ModifiedDate"].Visible = false;
            dataGridView1.Columns["MergeFromCusSeqNumber"].Visible = false;
            dataGridView1.Columns["TamilName"].Visible = false;
            dataGridView1.Columns["CollectionSpotId"].Visible = show;
            dataGridView1.Columns["ReturnDay"].Visible = show;
            //dataGridView1.Columns["ReturnType"].Visible = show;
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedRows = (sender as DataGridView).SelectedRows;

            if (selectedRows.Count != 1) return;

            var selectedCustomer = (selectedRows[0].DataBoundItem as Customer);

            var mainForm = (frmIndexForm)(((DataGridView)sender).Parent.Parent.Parent);

            mainForm.ShowForm<frmCustomerTransaction>(selectedCustomer);

        }

        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            NoteOptionChanged(sender);
        }

        private void rdbClosed_CheckedChanged(object sender, EventArgs e)
        {
            NoteOptionChanged(sender);

        }

        private void rdbActive_CheckedChanged(object sender, EventArgs e)
        {
            NoteOptionChanged(sender);
        }

        private void NoteOptionChanged(object sender)
        {
            List<Customer> searchedCustomer;

            if (rdbAll.Checked)
            {
                searchedCustomer = customers;
                GlobalValue.NoteOption = rdbAll.Tag.ToString();
            }
            else if (rdbClosed.Checked)
            {
                searchedCustomer = customers.Where(w => w.IsActive == false).OrderByDescending(o => o.ClosedDate).ToList();
                GlobalValue.NoteOption = rdbClosed.Tag.ToString();
            }
            else
            {
                searchedCustomer = customers.Where(w => w.IsActive == true).ToList();
                GlobalValue.NoteOption = rdbActive.Tag.ToString();
            }

            searchedCustomer = string.IsNullOrEmpty(txtSearch.Text) ? searchedCustomer : searchedCustomer.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            dataGridView1.DataSource = searchedCustomer;
            dataGridView1.ReadOnly = rdbClosed.Checked;
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {

            List<Customer> filteredData = null;

            if (rdbActive.Checked)
                filteredData = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower()) && w.IsActive == true).ToList(); // w => w.Interest > 0 &&
            else if (rdbClosed.Checked)
                filteredData = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower()) && w.IsActive == false).ToList(); //  w.Interest > 0 &&
            else //(rdbAll.Checked)
                filteredData = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();


            if (chkFriends.Checked)
                dataGridView1.DataSource = filteredData;
            else
                dataGridView1.DataSource = filteredData.Where(w => w.Interest > 0).ToList();



            GlobalValue.SearchText = txtSearch.Text;
        }



        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (sender as DataGridView);

            //if (grid.CurrentCell.ColumnIndex == 0) return;
            var rowIndex = grid.CurrentCell.RowIndex;

            var cus = grid.Rows[grid.CurrentCell.RowIndex].DataBoundItem as Customer;

            if (grid.CurrentCell.OwningColumn.Name == "ClosedDate" && FormGeneral.GetGridCellValue(grid, rowIndex, "ClosedDate") != null)
            {
                Customer.UpdateCustomerClosedDate(
                       new Customer()
                       {
                           CustomerId = cus.CustomerId,
                           CustomerSeqNumber = cus.CustomerSeqNumber,
                           ClosedDate = Convert.ToDateTime(FormGeneral.GetGridCellValue(grid, rowIndex, "ClosedDate"))
                       });
                return;

            }

            if (grid.CurrentCell.OwningColumn.Name == "Interest")
            {
                Customer.UpdateCustomerInterest(
                       new Customer()
                       {
                           CustomerId = cus.CustomerId,
                           CustomerSeqNumber = cus.CustomerSeqNumber,
                           Interest = FormGeneral.GetGridCellValue(grid, rowIndex, "Interest").ToInt32()
                       });
                return;

            }

            if (grid.CurrentCell.OwningColumn.Name == "LoanAmount")
            {
                Customer.UpdateCustomerLoan(
                       new Customer()
                       {
                           CustomerId = cus.CustomerId,
                           CustomerSeqNumber = cus.CustomerSeqNumber,
                           LoanAmount = FormGeneral.GetGridCellValue(grid, rowIndex, "LoanAmount").ToInt32()
                       });
                return;

            }

            if (grid.CurrentCell.OwningColumn.Name == "MonthlyInterest")
            {
                Customer.UpdateCustomerMonthlyInterest(
                       new Customer()
                       {
                           CustomerId = cus.CustomerId,
                           CustomerSeqNumber = cus.CustomerSeqNumber,
                           MonthlyInterest = FormGeneral.GetGridCellValue(grid, rowIndex, "MonthlyInterest").ToInt32()
                       });
                return;

            }

            if (grid.CurrentCell.OwningColumn.Name == "Name")
            {
                Customer.UpdateCustomerName(
                       new Customer()
                       {
                           CustomerId = cus.CustomerId,
                           CustomerSeqNumber = cus.CustomerSeqNumber,
                           Name = FormGeneral.GetGridCellValue(grid, rowIndex, "Name")
                       });
                return;

            }

            if (grid.CurrentCell.OwningColumn.Name == "AdjustedAmount")
            {
                var strAdjustmentAmount = FormGeneral.GetGridCellValue(grid, rowIndex, "AdjustedAmount");
                Customer.UpdateCustomerAdjustment(
                       new Customer()
                       {
                           CustomerId = cus.CustomerId,
                           CustomerSeqNumber = cus.CustomerSeqNumber,
                           AdjustedAmount = strAdjustmentAmount.ToInt32()
                       });
                return;

            }

            if (grid.CurrentCell.OwningColumn.Name == "ReturnType")
            {
                var returnType = FormGeneral.GetGridCellValue(grid, rowIndex, "ReturnType");

                Customer.UpdateCustomerReturnType(
                       new Customer()
                       {
                           CustomerId = cus.CustomerId,
                           CustomerSeqNumber = cus.CustomerSeqNumber,
                           ReturnType = returnType.ToEnum<ReturnTypeEnum>()
                       });
                return;

            }

            var strCollectedAmount = FormGeneral.GetGridCellValue(grid, rowIndex, "CollectionAmt");

            if (strCollectedAmount == null) return;

            var seqNo = cus.CustomerSeqNumber;
            var customerId = cus.CustomerId;
            var loanAmount = cus.LoanAmount;
            var valCollectedAmount = strCollectedAmount.ToInt32();

            if (string.IsNullOrEmpty(strCollectedAmount) == false)
            {
                var existingTxn = Transaction.GetTransactionForDate(new Transaction() { CustomerId = customerId, CustomerSequenceNo = seqNo, TxnDate = dateTimePicker1.Value });

                if (existingTxn != null && existingTxn.Count == 1)
                {

                    if (existingTxn.First().AmountReceived == valCollectedAmount)
                    {
                        return;
                    }

                    if (valCollectedAmount == 0 && DialogResult.Yes == MessageBox.Show($"Are you sure you want to delete an existing transactions for {cus.Name}?", "", MessageBoxButtons.YesNo))
                    {
                        Transaction.DeleteTransactionDetails(existingTxn.First());
                        cus.Interest -= existingTxn.First().AmountReceived;
                        Customer.UpdateCustomerInterest(cus);
                        return;
                    }

                    else if (DialogResult.Yes == MessageBox.Show($"Already have txn for {cus.Name}, Do you want to continue?", "", MessageBoxButtons.YesNo, MessageBoxIcon.Question))
                    {
                        Transaction.DeleteTransactionDetails(existingTxn.First());
                    }
                }


                var txn = new Transaction()
                {
                    AmountReceived = valCollectedAmount,
                    CustomerId = customerId,
                    CustomerSequenceNo = seqNo,
                    TransactionId = Transaction.GetNextTransactionId(),
                    Balance = (Transaction.GetBalance(cus) - valCollectedAmount),
                    TxnDate = dateTimePicker1.Value
                };


                if (cus.IsMonthly() && cus.LoanAmount != valCollectedAmount && txn.Balance > 0)
                {
                    txn.Balance = cus.LoanAmount;
                    cus.Interest += valCollectedAmount;
                    Customer.UpdateCustomerInterest(cus);
                }

                if (txn.Balance < 0)
                {
                    MessageBox.Show("Balance is less than 0. Please check your amount. Txn Aborted!", "", MessageBoxButtons.OK, icon: MessageBoxIcon.Stop);
                    LogHelper.WriteLog($"Balance is less than 0. Please check your amount. Txn Aborted!", txn.CustomerId, txn.CustomerSequenceNo);
                    return;
                }
                if (txn.Balance == 0)
                {
                    MessageBox.Show($"Good News, txn closed for [{txn.CustomerSequenceNo}]-[{cus.Name}]", "", MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
                    LogHelper.WriteLog($"Good News, This txn will be closed!", txn.CustomerId, txn.CustomerSequenceNo);

                    // Add new txn for extra amount.
                    //if(cus.AdjustedAmount != null && cus.AdjustedAmount > 0)
                    //{

                    //}

                    Customer.CloseCustomerTxn(cus, false, txn.TxnDate);
                }

                txn.IsClosed = (txn.Balance == 0);


                Transaction.AddDailyTransactions(txn);

                // Update txn Closed Date
                if (txn.IsClosed && txn.Balance == 0)
                {
                    // Update Closed Date
                    Customer.UpdateCustomerClosedDate(
                        new Customer()
                        {
                            CustomerId = txn.CustomerId,
                            CustomerSeqNumber = txn.CustomerSequenceNo,
                            ClosedDate = txn.TxnDate,
                        });
                }

                if (MessageBox.Show($"{cus.Name} - {txn.AmountReceived}. Do you want to continue with next customer?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    txtSearch.SelectAll();
                    txtSearch.Focus();
                }
                return;
            }


            //Customer.CorrectCustomerData(cus); // TODO: dont know the reason for this code here!!!

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SetCustomers();
            txtSearch.Focus();
            txtSearch.Text = string.Empty;
            rdbActive.Checked = true;
        }

        private void dataGridView1_MouseClick(object sender, MouseEventArgs e)
        {

            if (dataGridView1.SelectedRows.Count > 1) return;

            if (e.Button == MouseButtons.Right)
            {
                ContextMenuStrip strip = new ContextMenuStrip();
                int rowIndex = dataGridView1.HitTest(e.X, e.Y).RowIndex;

                var seqNo = FormGeneral.GetGridCellValue(dataGridView1, rowIndex, "CustomerSeqNumber");
                //var customerId = FormGeneral.GetGridCellValue(dataGridView1, rowIndex, "CustomerId");
                //var isActive = FormGeneral.GetGridCellValue(dataGridView1, rowIndex, "IsActive");

                var selectedCustomer = Customer.GetCustomerDetails(seqNo.ToInt32());

                strip.Tag = new Customer()
                {
                    CustomerSeqNumber = Convert.ToInt32(seqNo),
                    CustomerId = selectedCustomer.CustomerId,
                    IsActive = selectedCustomer.IsActive
                };

                var investigationText = selectedCustomer.NeedInvestigation ? "Make - No need of investigation" : "Make - Need Investigation";
                var EligibilityText = selectedCustomer.GivenEligibility ? "Make Not Eligible" : "Make Eligible";
                var noteWithUsText = selectedCustomer.NoteWithUs ? "Give Note To Customer" : "Take Note with us";
                if (rowIndex >= 0)
                {
                    strip.Items.Add("Delete Customer and Txn").Name = "All";
                    strip.Items.Add("Delete Customer only").Name = "Cus";
                    strip.Items.Add("Delete Txn only").Name = "Txn";
                    strip.Items.Add(investigationText).Name = "InvStatus";
                    strip.Items.Add(EligibilityText).Name = "ElgStatus";
                    strip.Items.Add(noteWithUsText).Name = "NoteStatus";
                    strip.Items.Add("Show Only This Customer").Name = "OnlyThisCus";
                }

                strip.Show(dataGridView1, new System.Drawing.Point(e.X, e.Y));

                strip.ItemClicked += Strip_ItemClicked;

            }

        }

        private void Strip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var cus = (Customer)((ContextMenuStrip)sender).Tag;

            if (e.ClickedItem.Name == "All")
            {
                Customer.DeleteCustomerDetails(cus.CustomerId, cus.CustomerSeqNumber);
                Transaction.DeleteTransactionDetails(cus.CustomerId, cus.CustomerSeqNumber);
            }
            else if (e.ClickedItem.Name == "Cus")
            {
                Customer.DeleteCustomerDetails(cus.CustomerId, cus.CustomerSeqNumber);
            }
            else if (e.ClickedItem.Name == "Txn")
            {
                Transaction.DeleteTransactionDetails(cus.CustomerId, cus.CustomerSeqNumber);
            }
            else if (e.ClickedItem.Name == "InvStatus")
            {
                Customer.UpdateCustomerInvestigation(cus.CustomerSeqNumber);
            }
            else if (e.ClickedItem.Name == "ElgStatus")
            {
                Customer.UpdateCustomerEligibility(cus.CustomerId);
            }
            else if (e.ClickedItem.Name == "NoteStatus")
            {
                Customer.UpdateCustomerNoteLocation(cus.CustomerSeqNumber);
            }
            else if (e.ClickedItem.Name == "OnlyThisCus")
            {
                var data = Customer.GetAllCustomer().Where(w => w.CustomerId == cus.CustomerId).ToList();

                var totalInputMoney = data.Sum(s => s.Interest);
                var totalOutputMoney = data.Sum(s => Transaction.GetBalance(s));

                var message = $"Income: {totalInputMoney} {Environment.NewLine}Balance: {totalOutputMoney} {Environment.NewLine} Profit: {totalInputMoney - totalOutputMoney} {Environment.NewLine}";
                dataGridView1.DataSource = data;

                MessageBox.Show(message);
            }

        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) return;
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["CollectionAmt"];
            dataGridView1.BeginEdit(true);
        }

        private void cmbFilters_SelectedIndexChanged(object sender, EventArgs e)
        {

            if (customers == null) return;
            var value = ((KeyValuePair<int, string>)cmbFilters.SelectedItem).Key;
            List<Customer> searchedCustomer;



            if (value == 1)
            {
                searchedCustomer = customers.OrderByDescending(o => o.LoanAmount).ToList();
            }
            else if (value == 2)
            {
                searchedCustomer = customers.OrderBy(o => o.CustomerSeqNumber).ToList();
            }
            else if (value == 3)
            {
                searchedCustomer = customers.OrderBy(o => o.CustomerId).ToList();
            }
            else if (value == 4)
            {
                searchedCustomer = customers.OrderBy(o => o.Name).ToList();
            }
            else if (value == 5)
            {
                searchedCustomer = customers.Where(w => w.ReturnDay == DateTime.Today.DayOfWeek).ToList();
            }
            else if (value == 6)
            {
                searchedCustomer = customers.Where(w => w.ReturnDay == DateTime.Today.AddDays(1).DayOfWeek).ToList();
            }
            else if (value == 7)
            {
                searchedCustomer = customers.OrderBy(o => o.ReturnDay).ToList();
            }
            else if (value == 8)
            {
                searchedCustomer = customers.OrderBy(o => o.ReturnType).ToList();
            }
            else if (value == 9)
            {
                searchedCustomer = customers.OrderByDescending(o => o.CollectionSpotId).ToList();
            }
            else if (value == 10)
            {
                searchedCustomer = customers.OrderByDescending(o => o.AdjustedAmount).ToList();
            }
            else if (value == 11)
            {
                searchedCustomer = customers.Where(w => w.GivenEligibility == false).ToList();
            }
            else if (value == 12)
            {
                searchedCustomer = customers.Where(w => w.NeedInvestigation == true).ToList();
            }
            else if (value == 13)
            {
                searchedCustomer = customers.Where(w => w.NoteWithUs == true).OrderBy(o => o.Name).ToList();
            }
            else
            {
                searchedCustomer = customers.Where(w => w.ReturnDay == DateTime.Today.AddDays(-1).DayOfWeek).ToList();
            }

            if (rdbActive.Checked)
            {
                searchedCustomer = searchedCustomer.Where(w => w.IsActive).ToList();
            }
            else if (rdbClosed.Checked)
            {
                searchedCustomer = searchedCustomer.Where(w => w.IsActive == false).ToList();
            }


            dataGridView1.DataSource = searchedCustomer;
            AdjustColumnOrder();

            GlobalValue.SortingByValue = cmbFilters.SelectedIndex;
        }

        private void chkAllColumns_CheckedChanged(object sender, EventArgs e)
        {
            SetColumnVisibility(chkAllColumns.Checked);

        }

        private void dataGridView1_DataSourceChanged(object sender, EventArgs e)
        {
            var data = dataGridView1.DataSource as List<Customer>;

            lblRowCount.Text = $"Row Count: {dataGridView1.Rows.Count.ToString()} (I: {data.Sum(s => s.Interest).ToMoneyFormat()} MI: {data.Sum(s => s.MonthlyInterest).TokFormat()} ADJ: {data.Sum(s => s.AdjustedAmount).ToMoney()} )";
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            GlobalValue.CollectionDate = dateTimePicker1.Value;
        }

        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var neededRow = (Customer)((sender as DataGridView).Rows[e.RowIndex]).DataBoundItem;

            var balance = Transaction.GetBalance(neededRow);

            dataGridView1.Rows[e.RowIndex].Cells["Name"].ToolTipText = balance.ToString();
        }

        private void cmbReturnTypes_SelectedIndexChanged(object sender, EventArgs e)
        {
            var data = customers.Where(w => w.IsActive && w.ReturnType == (ReturnTypeEnum)cmbReturnTypes.SelectedValue).ToList();

            if ((ReturnTypeEnum)cmbReturnTypes.SelectedItem == ReturnTypeEnum.Monthly)
            {
                dataGridView1.DataSource = data.OrderBy(o => o.AmountGivenDate.Value.Day).ToList();
            }
            else
            {
                dataGridView1.DataSource = data;
            }

            GlobalValue.ReturnTypeValue = cmbReturnTypes.SelectedIndex;

        }

        private void chkFriends_CheckedChanged(object sender, EventArgs e)
        {
            GlobalValue.FriendAlsoValue = chkFriends.Checked;
        }

        private void btnLatestCollection_Click(object sender, EventArgs e)
        {
            var collectionAmount = Transaction.GetDailyCollectionAmount(dateTimePicker1.Value);
            btnLatestCollection.Text = collectionAmount.ToMoneyFormat();
        }
    }
}
