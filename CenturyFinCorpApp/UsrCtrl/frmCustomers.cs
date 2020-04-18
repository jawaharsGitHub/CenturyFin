using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmCustomers : UserControl
    {
        private List<Customer> customers;
        public frmCustomers()
        {
            InitializeComponent();

            /*
            #region DuplicateNames

            string thoguthiName = "tvdallnames";

            var fpath = @"E:/" + thoguthiName;
            var text = File.ReadAllLines(fpath);
            var rrr = new List<string>();

            int gtOne = 0;
            int gtOneElse = 0;
            int gtOneTotal = 0;
            text.ToList().ForEach(f =>
            {
                var fullName = f.Split('.').ToList();

                if (fullName.Count > 1)
                {
                    rrr.Add(fullName[1]);
                    gtOne += 1;
                    gtOneTotal += 1;
                }
                else
                {
                    rrr.Add(fullName[0]);
                    gtOneElse += 1;
                    gtOneTotal += 1;
                }

            });

            var duplicateNames = (from p in rrr
                                group p by p into newPh
                                select new
                                {
                                    newPh.Key,
                                    Count = newPh.Count()
                                }).ToList();

            var resultPh = duplicateNames.Where(w => w.Count > 1).OrderByDescending(o => o.Count).ToList();

            var totalRec = resultPh.Sum(s => s.Count);


            StringBuilder resultP = new StringBuilder();
            int index = 0;

            resultPh.ForEach(rfe =>
            {
                index += 1;
                resultP.AppendLine($"{index}.{rfe.Key}({rfe.Count})");
            });

            resultP.Insert(0, $"Total Records need to check is {totalRec}");


            File.WriteAllText($"E:/{thoguthiName}-result.txt", resultP.ToString());
            
            #endregion
            */
            

            textBox1.Text = @"Daily = 0,
       Alternate = 1,
       Weekly = 2,
       BiWeekly = 3,
       BiMonthly = 4,
       Monthly = 5,
       GoldMonthly = 6,
       TenMonths = 7,
       NI = 8,
       None = 9
       Loss = 10
       Random = 11
       Others = 12";

            txtSearch.GotFocus += (s, e) =>
            {
                txtSearch.BackColor = Color.Yellow;
                txtSearch.ForeColor = Color.DarkRed;

            };
            txtSearch.LostFocus += (s, e) =>
            {
                txtSearch.BackColor = Color.Gray;
                txtSearch.ForeColor = Color.White;

            };


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


            GlobalValue.CollectionDate = DailyCollectionDetail.GetNexttCollectionDate();

            dateTimePicker1.Value = GlobalValue.CollectionDate.Value;

            RefreshClosed();


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
                   new KeyValuePair<int, string>(13, "Only Note With us"),
                   new KeyValuePair<int, string>(14, "Only Personal"),
                   new KeyValuePair<int, string>(15, "No Tamil Name"),
                   new KeyValuePair<int, string>(16, "Only Adj & To Be Closed."),
                   new KeyValuePair<int, string>(17, "By Balance %"),
                   new KeyValuePair<int, string>(18, "By Interest")

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

            var activeTxn = customers.Count(c => c.IsActive == true && c.Interest > 0 && c.IsNotMonthly());
            var NoInterestactiveTxn = customers.Count(c => c.IsActive == true && c.Interest == 0);
            var monthlyINtTxn = customers.Count(c => c.IsActive == true && c.IsMonthly());
            var closedTxn = customers.Count(c => c.IsActive == false);
            //var totalTxn = activeTxn + closedTxn + monthlyINtTxn + NoInterestactiveTxn;
            var totalTxn = customers.Count;
            var activeCUs = customers.Where(w => w.IsActive).Count();


            this.Text = $"WELCOME TO Running Notes: {activeTxn} Closed Notes: {closedTxn} Total Notes: {totalTxn}";

            rdbActive.Text = $"RUNNING ({activeCUs}) {Environment.NewLine}({activeTxn}R + {NoInterestactiveTxn}NI + {monthlyINtTxn}MI)";
            rdbClosed.Text = $"CLOSED ({closedTxn})";
            rdbAll.Text = $"ALL ({totalTxn})";

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

            //var extra = (days - totalTxn) >= 0 ? "Shortage" : "Extra";

            label1.Text = $"{myData.TotalNotes} {Environment.NewLine} " +
              $"{myData.DaysToMonth} {Environment.NewLine} " +
              $"{myData.NotesPerDay} {Environment.NewLine} " +
              $"{myData.ClosedNotesPerDay} {Environment.NewLine} " +
              $"{myData.Data1}{Environment.NewLine} " +
              $"{myData.Data2} Monthly - {myData.Data3.TokFormat()}  {Environment.NewLine}"
              //$"  need {365 - totalTxn} in {365 - days} days [{extra}: {Math.Abs(days - totalTxn)}] {Environment.NewLine} " +
              //$"{DateHelper.DaysToMonth(" Days Left", DateTime.Today, new DateTime(2019, 1, 24))}"
              ;

            dataGridView1.DataSource = customers;

        }
        private void AdjustColumnOrder()
        {
            dataGridView1.Columns["MonthlyInterest"].HeaderText = "Monthly Payment";

            dataGridView1.Columns["CollectionAmt"].DisplayIndex = 4;
            dataGridView1.Columns["ReturnDay"].DisplayIndex = 4;
            dataGridView1.Columns["ReturnType"].DisplayIndex = 5;
            dataGridView1.Columns["CollectionSpotId"].DisplayIndex = 6;
            dataGridView1.Columns["AdjustedAmount"].DisplayIndex = 3;
            dataGridView1.Columns["AmountGivenDate"].DisplayIndex = 8;
            dataGridView1.Columns["ClosedDate"].DisplayIndex = 9;

            dataGridView1.Columns["AmountGivenDate"].DefaultCellStyle.Format = Const.GridDateFormat;
            dataGridView1.Columns["ClosedDate"].DefaultCellStyle.Format = Const.GridDateFormat;

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

            //dataGridView1.Columns["BusType"].CellType = typeof(ComboBox);

        }
        private void SetColumnVisibility(bool show = false)
        {
            dataGridView1.Columns["ModifiedDate"].Visible = false;
            dataGridView1.Columns["PhoneNumber"].Visible = show;
            dataGridView1.Columns["IsMerged"].Visible = false;
            dataGridView1.Columns["MergedDate"].Visible = false;
            dataGridView1.Columns["IdAndName"].Visible = false;
            dataGridView1.Columns["IsForceClosed"].Visible = false;
            dataGridView1.Columns["NameAndSeqId"].Visible = false;
            dataGridView1.Columns["NameAndId"].Visible = false;
            dataGridView1.Columns["ModifiedDate"].Visible = false;
            dataGridView1.Columns["MergeFromCusSeqNumber"].Visible = false;
            dataGridView1.Columns["TamilName"].Visible = show;
            dataGridView1.Columns["InitialInterest"].Visible = false;
            dataGridView1.Columns["GivenEligibility"].Visible = false;
            dataGridView1.Columns["NoteWithUs"].Visible = false;
            dataGridView1.Columns["IsPersonal"].Visible = false;
            dataGridView1.Columns["NeedInvestigation"].Visible = show;
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

            var data = customers
                .OrderBy(o => o.ReturnType.ToInt32())
                .Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower()));

            if (rdbActive.Checked)
                filteredData = data.Where(w => w.IsActive == true).ToList();
            else if (rdbClosed.Checked)
                filteredData = data.Where(w => w.IsActive == false).ToList();
            else //(rdbAll.Checked)
                filteredData = data.ToList();

            if (chkFriends.Checked)
                dataGridView1.DataSource = filteredData.Where(w => w.Interest == 0).ToList();
            else
                dataGridView1.DataSource = filteredData;

            GlobalValue.SearchText = txtSearch.Text;
        }

        #region "Very sensitive - Grid cell edit functionalities..."


        bool IsEnterKey = false;
        int cellCurrentValue;

        private void txtSearch_Leave(object sender, EventArgs e)
        {
            if (dataGridView1.Rows.Count == 0) return;
            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["CollectionAmt"];
            cellCurrentValue = Transaction.GetLastTransactionAmount(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].DataBoundItem as Customer);
            dataGridView1.CurrentCell.Value = cellCurrentValue;
            dataGridView1.BeginEdit(true);
        }


        protected override bool ProcessCmdKey(ref System.Windows.Forms.Message msg, Keys keyData)
        {
            IsEnterKey = (keyData == Keys.Enter);
            return false;
        }

        private void dataGridView1_CellClick(object sender, DataGridViewCellEventArgs e)
        {
            GridEdit();
        }

        private void dataGridView1_KeyDown(object sender, KeyEventArgs e)
        {
            GridEdit();
        }

        private void dataGridView1_KeyUp(object sender, KeyEventArgs e)
        {
            GridEdit();
        }

        private void GridEdit()
        {
            if (dataGridView1.CurrentCell.OwningColumn.Name == "CollectionAmt")
            {
                cellCurrentValue = Transaction.GetLastTransactionAmount(dataGridView1.Rows[dataGridView1.CurrentCell.RowIndex].DataBoundItem as Customer);
                dataGridView1.CurrentCell.Value = cellCurrentValue;
                dataGridView1.BeginEdit(true);
            }
        }

        private void EditSuccess()
        {
            dataGridView1.CurrentCell.Style.BackColor = Color.LightGreen;
            dataGridView1.CurrentCell.Style.ForeColor = Color.White;
            this.dataGridView1.ClearSelection();
            IsEnterKey = false; // reset flag after ecery success edit.
        }

        private void EditCancel()
        {
            dataGridView1.CurrentCell.Style.BackColor = Color.Red;
            dataGridView1.CurrentCell.Style.ForeColor = Color.Yellow;
        }
        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            if (IsEnterKey == false)
            {
                EditCancel();
                return;
            }

            int existingTxnId = 0; // to  keep existing txn id.
            DataGridView grid = (sender as DataGridView);
            int rowIndex = grid.CurrentCell.RowIndex;
            string owningColumnName = grid.CurrentCell.OwningColumn.Name;
            string cellValue = FormGeneral.GetGridCellValue(grid, rowIndex, owningColumnName);
            Customer cus = grid.Rows[grid.CurrentCell.RowIndex].DataBoundItem as Customer;

            if (string.IsNullOrEmpty(cellValue) || (cellValue == "0" && Transaction.GetBalance(cus) == cus.LoanAmount))
            {
                EditCancel();
                return;
            }

            var updatedCustomer = new Customer()
            {
                CustomerId = cus.CustomerId,
                CustomerSeqNumber = cus.CustomerSeqNumber
            };

            #region "Edit mode"

            if (owningColumnName == "CollectionAmt")
            {
                int seqNo = cus.CustomerSeqNumber;
                int customerId = cus.CustomerId;
                int loanAmount = cus.LoanAmount;
                int valCollectedAmount = cellValue.ToInt32();

                List<Transaction> existingTxns = Transaction.GetTransactionForDate(
                    new Transaction()
                    {
                        CustomerId = customerId,
                        CustomerSequenceNo = seqNo,
                        TxnDate = dateTimePicker1.Value
                    }
                    );

                Transaction LastexistingTxn = null;

                if (existingTxns != null && existingTxns.Count > 0)
                    LastexistingTxn = existingTxns.OrderBy(o => o.TransactionId).Last();

                if (LastexistingTxn != null)
                {
                    if (LastexistingTxn.AmountReceived == valCollectedAmount)
                        return;

                    // DELETE TXN.
                    if (valCollectedAmount == 0 && DialogResult.Yes == MessageBox.Show($"Are you sure you want to delete an existing transactions for {cus.Name}?", "", MessageBoxButtons.YesNo))
                    {
                        Transaction.DeleteTransactionDetails(LastexistingTxn);
                        cus.Interest -= LastexistingTxn.AmountReceived;
                        Customer.UpdateCustomerInterest(cus);
                        return;
                    }
                    // REPLACE/UPDATE TXN.
                    else if (LastexistingTxn.AmountReceived != valCollectedAmount)
                    {
                        if (MessageBox.Show("You want to replace existing txns?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                        {
                            existingTxnId = LastexistingTxn.TransactionId;
                            Transaction.DeleteTransactionDetails(LastexistingTxn);
                        }
                        else
                        {
                            return;
                        }
                    }
                }

                Transaction txn = new Transaction()
                {
                    AmountReceived = valCollectedAmount,
                    CustomerId = customerId,
                    CustomerSequenceNo = seqNo,
                    TransactionId = existingTxnId > 0 ? existingTxnId : Transaction.GetNextTransactionId(),
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
                    //LogHelper.WriteLog($"Balance is less than 0. Please check your amount. Txn Aborted!", txn.CustomerId, txn.CustomerSequenceNo);
                    return;
                }

                if (txn.Balance == 0)
                {
                    MessageBox.Show($"Good News, txn closed for [{txn.CustomerSequenceNo}]-[{cus.Name}]", "", MessageBoxButtons.OK, icon: MessageBoxIcon.Exclamation);
                    //LogHelper.WriteLog($"Good News, This txn will be closed!", txn.CustomerId, txn.CustomerSequenceNo);
                    Customer.CloseCustomerTxn(cus, false, txn.TxnDate);
                }

                txn.IsClosed = (txn.Balance == 0);

                Transaction.AddDailyTransactions(txn);

                // UPDATE TXN CLOSED DATE
                if (txn.IsClosed && txn.Balance == 0)
                {
                    Customer.UpdateCustomerClosedDate(
                        new Customer()
                        {
                            CustomerId = txn.CustomerId,
                            CustomerSeqNumber = txn.CustomerSequenceNo,
                            ClosedDate = txn.TxnDate,
                        });
                }

                EditSuccess();

                if (MessageBox.Show($"{cus.Name} - {txn.AmountReceived}. Do you want to continue with next customer?", "", MessageBoxButtons.YesNo) == DialogResult.Yes)
                {
                    txtSearch.SelectAll();
                    txtSearch.Focus();
                }
                return;
            }

            else if (owningColumnName == "NeedInvestigation")
            {
                updatedCustomer.NeedInvestigation = Convert.ToBoolean(cellValue);
                Customer.UpdateCustomerNeedInvestigation(updatedCustomer);
            }

            else if (owningColumnName == "ClosedDate")
            {
                updatedCustomer.ClosedDate = Convert.ToDateTime(cellValue);
                Customer.UpdateCustomerClosedDate(updatedCustomer);
            }

            else if (owningColumnName == "Interest")
            {
                updatedCustomer.Interest = cellValue.ToInt32();
                Customer.UpdateCustomerInterest(updatedCustomer);
            }

            else if (owningColumnName == "LoanAmount")
            {
                updatedCustomer.LoanAmount = cellValue.ToInt32();
                Customer.UpdateCustomerLoan(updatedCustomer);
            }

            else if (owningColumnName == "MonthlyInterest")
            {
                updatedCustomer.MonthlyInterest = cellValue.ToInt32();
                Customer.UpdateCustomerMonthlyInterest(updatedCustomer);
            }

            else if (owningColumnName == "Name")
            {
                updatedCustomer.Name = cellValue;
                Customer.UpdateCustomerName(updatedCustomer);
            }

            else if (owningColumnName == "AdjustedAmount")
            {
                updatedCustomer.AdjustedAmount = cellValue.ToInt32();
                Customer.UpdateCustomerAdjustment(updatedCustomer);
            }

            else if (owningColumnName == "ReturnType")
            {
                updatedCustomer.ReturnType = cellValue.ToEnum<ReturnTypeEnum>();
                Customer.UpdateCustomerReturnType(updatedCustomer);
            }

            else if (owningColumnName == "InitialInterest")
            {
                updatedCustomer.InitialInterest = cellValue.ToInt32();
                Customer.UpdateInitialInterest(updatedCustomer);
            }

            else if (owningColumnName == "TamilName")
            {
                updatedCustomer.TamilName = cellValue;
                Customer.UpdateTamilName(updatedCustomer);
            }

            else if (owningColumnName == "PhoneNumber")
            {
                updatedCustomer.PhoneNumber = cellValue;
                Customer.UpdatePhoneNo(updatedCustomer);
            }

            else if (owningColumnName == "AmountGivenDate")
            {
                updatedCustomer.AmountGivenDate = Convert.ToDateTime(cellValue);
                Customer.UpdateAmountGivenDate(updatedCustomer);
            }
            else if (owningColumnName == "BusType")
            {
                updatedCustomer.BusType = cellValue.ToInt32();
                Customer.UpdateBusinessType(updatedCustomer);
            }

            EditSuccess();

            #endregion
        }

        #endregion "Very sensitive - Grid cell edit functionalities..."

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            // Set to deafulted values.

            txtSearch.Text = string.Empty;
            cmbFilters.SelectedIndex = 0;
            cmbReturnTypes.SelectedIndex = 0;
            chkFriends.Checked = false;

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
                var personalText = selectedCustomer.IsPersonal ? "Make This Public" : "Make This Personal";
                if (rowIndex >= 0)
                {
                    strip.Items.Add("Delete Customer and Txn").Name = "All";
                    strip.Items.Add("Delete Customer only").Name = "Cus";
                    strip.Items.Add("Delete Txn only").Name = "Txn";
                    strip.Items.Add(investigationText).Name = "InvStatus";
                    strip.Items.Add(EligibilityText).Name = "ElgStatus";
                    strip.Items.Add(noteWithUsText).Name = "NoteStatus";
                    strip.Items.Add("Show Only This Customer").Name = "OnlyThisCus";
                    strip.Items.Add(personalText).Name = "Personal";
                    strip.Items.Add("Sum").Name = "Sum";
                }

                strip.Show(dataGridView1, new System.Drawing.Point(e.X, e.Y));

                strip.ItemClicked += Strip_ItemClicked;

            }

        }
        private void Strip_ItemClicked(object sender, ToolStripItemClickedEventArgs e)
        {
            var cus = (Customer)((ContextMenuStrip)sender).Tag;


            if (e.ClickedItem.Name == "Cus")
            {
                Customer.DeleteCustomerDetails(cus.CustomerId, cus.CustomerSeqNumber);
                btnRefresh_Click(null, null);
            }

            else if (e.ClickedItem.Name == "Txn") Transaction.DeleteTransactionDetails(cus.CustomerId, cus.CustomerSeqNumber);

            else if (e.ClickedItem.Name == "InvStatus") Customer.ToggleCustomerInvestigation(cus.CustomerSeqNumber);

            else if (e.ClickedItem.Name == "ElgStatus") Customer.UpdateCustomerEligibility(cus.CustomerId);

            else if (e.ClickedItem.Name == "NoteStatus") Customer.UpdateCustomerNoteLocation(cus.CustomerSeqNumber);

            else if (e.ClickedItem.Name == "Personal") Customer.UpdateCustomerPersonalFlag(cus.CustomerSeqNumber);

            else if (e.ClickedItem.Name == "All")
            {
                Customer.DeleteCustomerDetails(cus.CustomerId, cus.CustomerSeqNumber);
                Transaction.DeleteTransactionDetails(cus.CustomerId, cus.CustomerSeqNumber);
                btnRefresh_Click(null, null);
            }

            else if (e.ClickedItem.Name == "OnlyThisCus")
            {
                var data = Customer.GetAllCustomer().Where(w => w.CustomerId == cus.CustomerId).OrderByDescending(o => o.IsActive).ToList();

                var totalInputMoney = data.Sum(s => s.Interest);
                var totalOutputMoney = data.Sum(s => Transaction.GetBalance(s));

                var message = $"Income: {totalInputMoney} {Environment.NewLine}Balance: {totalOutputMoney} {Environment.NewLine} Profit: {totalInputMoney - totalOutputMoney} {Environment.NewLine}";
                dataGridView1.DataSource = data;

                MessageBox.Show(message);
            }
            else if (e.ClickedItem.Name == "Sum")
            {
                int sum = 0;

                foreach (DataGridViewCell cell in dataGridView1.SelectedCells)
                    sum += cell.Value.ToInt32();

                MessageBox.Show(sum.ToString());
            }

        }
        private void frmCustomers_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();
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
                searchedCustomer = customers.Where(w => w.AdjustedAmount != null).OrderBy(o => o.AdjustedAmount).ToList();
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
            else if (value == 14)
            {
                searchedCustomer = customers.Where(w => w.IsPersonal == true).OrderByDescending(o => o.LoanAmount).ToList();
            }
            else if (value == 15)
            {
                searchedCustomer = customers.Where(w => string.IsNullOrEmpty(w.TamilName) == true && w.IsPersonal == false).OrderByDescending(o => o.LoanAmount).ToList();
            }
            else if (value == 16)
            {
                searchedCustomer = customers.Where(w => w.AdjustedAmount != null).OrderBy(o => o.AdjustedAmount).ToList();
                searchedCustomer = searchedCustomer.Where(w => Math.Abs(w.AdjustedAmount.ToInt32()) == Transaction.GetBalance(w)).OrderByDescending(o => o.LoanAmount).ToList();
            }
            else if (value == 17)
            {
                searchedCustomer = customers.OrderBy(o => ((Convert.ToDecimal(Transaction.GetBalance(o) / Convert.ToDecimal(o.LoanAmount))) * 100)).ToList();
            }
            else if (value == 18)
            {
                searchedCustomer = customers.OrderBy(o => o.Interest).ToList();
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
            txtSearch_TextChanged(null, null);

        }
        private void btnLatestCollection_Click(object sender, EventArgs e)
        {
            // FAST
            btnLatestCollection.Text = Transaction.GetDailyCollectionAmountQuickAndTemp(dateTimePicker1.Value);

            // SLOW
            //var collectionAmount = Transaction.GetDailyCollectionAmount(dateTimePicker1.Value);
            //btnLatestCollection.Text = collectionAmount.ToMoneyFormat();
        }

        private void btnClosedTxn_Click(object sender, EventArgs e)
        {
            var list = Transaction.GetAllTransactions();

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

        private void RefreshClosed()
        {
            btnClosedTxn.Text = $"Move Closed Txn({Transaction.GetClosedTxn()})";
        }

        private void dataGridView1_CellBeginEdit(object sender, DataGridViewCellCancelEventArgs e)
        {
            //int existingTxnId = 0; // to  keep existing txn id.
            //DataGridView grid = (sender as DataGridView);
            //int rowIndex = grid.CurrentCell.RowIndex;
            //string owningColumnName = grid.CurrentCell.OwningColumn.Name;
            //string cellValue = FormGeneral.GetGridCellValue(grid, rowIndex, owningColumnName);
            //Customer cus = grid.Rows[grid.CurrentCell.RowIndex].DataBoundItem as Customer;

            //if(owningColumnName == "BusType")
            //{
            //    ComboBox cb = new ComboBox();                
            //    var allBT = BusinessType.GetBusinessTypes();
            //    cb.DataSource = allBT;
            //    grid.CurrentCell. = cb;

            //}

        }
    }
}
