using Common;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Configuration;
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

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["usingMenu"]) == true)
                btnAddCustomer.Visible = false;
            else
                btnAddCustomer.Visible = true;

            // Get the table from the data set
            SetCustomers();

            if (customers == null) return;

            //Create New DataGridViewTextBoxColumn
            DataGridViewTextBoxColumn textboxColumn = new DataGridViewTextBoxColumn();
            textboxColumn.HeaderText = "Collection Amount";
            textboxColumn.Name = "CollectionAmt";

            dataGridView1.DataSource = customers;

            //Add TextBoxColumn dynamically to DataGridView
            dataGridView1.Columns.Add(textboxColumn);
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            AdjustColumnOrder();


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


        }

        private void SetCustomers()
        {
            customers = Customer.GetAllCustomer().OrderBy(o => o.AmountGivenDate).ToList();

            // For Get the data i need.
            //StringBuilder sb = new StringBuilder();
            //StringBuilder sb2 = new StringBuilder();

            //var c1 = Customer.GetAllCustomer().OrderBy(o => o.AmountGivenDate);

            //foreach (var item in c1)
            //{
            //    sb.AppendLine(item.AmountGivenDate.ToString());

            //}

            //var c2 = Customer.GetAllCustomer().Where(w => w.ClosedDate != null).OrderBy(o => o.ClosedDate);

            //foreach (var item in c2)
            //{
            //    sb2.AppendLine(item.ClosedDate.ToString());

            //}


            var activeTxn = customers.Count(c => c.IsActive == true);
            var closedTxn = customers.Count(c => c.IsActive == false);
            var totalTxn = activeTxn + closedTxn;

            this.Text = $"WELCOME TO Running Notes: {activeTxn} Closed Notes: {closedTxn} Total Notes: {totalTxn}";

            rdbActive.Text = $"RUNNING NOTES ({activeTxn})";
            rdbClosed.Text = $"CLOSED NOTES ({closedTxn})";
            rdbAll.Text = $"ALL NOTES ({totalTxn})";

            // Customer day and count ratio.
            var days = (DateTime.Today - new DateTime(2018, 1, 25)).TotalDays;

            label1.Text = $"{totalTxn} customers in {days} days {Environment.NewLine} {Math.Round(totalTxn/days, 1)} customer(s) per day";
        }

        private void AdjustColumnOrder()
        {
            dataGridView1.Columns["CollectionAmt"].DisplayIndex = 3;
            dataGridView1.Columns["ModifiedDate"].Visible = false;
            dataGridView1.Columns["PhoneNumber"].Visible = false;
            dataGridView1.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            dataGridView1.Columns["ClosedDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            dataGridView1.Columns["Name"].Width = 250;
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedRows = (sender as DataGridView).SelectedRows;

            if (selectedRows.Count != 1) return;

            var selectedCustomer = (selectedRows[0].DataBoundItem as Customer);

            var mainForm = (frmIndexForm)(((DataGridView)sender).Parent.Parent.Parent); //new frmIndexForm(true);

            mainForm.ShowForm<frmCustomerTransaction>(selectedCustomer);

            //frmCustomerTransaction cd = new frmCustomerTransaction(selectedCustomer));

            //cd.ShowDialog();

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
                searchedCustomer = customers.Where(w => w.IsActive == false).OrderBy(o => o.ClosedDate).ToList();
                GlobalValue.NoteOption = rdbClosed.Tag.ToString();
            }
            else
            {
                searchedCustomer = customers.Where(w => w.IsActive == true).ToList();
                GlobalValue.NoteOption = rdbActive.Tag.ToString();
            }

            //var result = from

            searchedCustomer = string.IsNullOrEmpty(txtSearch.Text) ? searchedCustomer : searchedCustomer.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            //var query = (from c in searchedCustomer
            //             join t in transactions on c.CustomerId equals t.CustomerId
            //             select new {
            //                 c,
            //                 t.Balance
            //             }).ToList();

            dataGridView1.DataSource = searchedCustomer;



        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (rdbActive.Checked)
                dataGridView1.DataSource = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower()) && w.IsActive == true).ToList();
            if (rdbClosed.Checked)
                dataGridView1.DataSource = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower()) && w.IsActive == false).ToList();
            if (rdbAll.Checked)
                dataGridView1.DataSource = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();

            GlobalValue.SearchText = txtSearch.Text;
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            //frmAddCustomer ac = new frmAddCustomer();
            //ac.ShowDialog();

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (sender as DataGridView);
            var rowIndex = grid.CurrentCell.RowIndex;

            var seqNo = FormGeneral.GetGridCellValue(grid, rowIndex, "CustomerSeqNumber");
            var customerId = FormGeneral.GetGridCellValue(grid, rowIndex, "CustomerId");
            var loanAmount = FormGeneral.GetGridCellValue(grid, rowIndex, "LoanAmount");
            var collectedAmount = FormGeneral.GetGridCellValue(grid, rowIndex, "CollectionAmt");

            if (string.IsNullOrEmpty(collectedAmount) == false)
            {
                var txn = new Transaction()
                {
                    AmountReceived = Convert.ToInt32(collectedAmount),
                    CustomerId = Convert.ToInt32(customerId),
                    CustomerSequenceNo = Convert.ToInt32(seqNo),
                    TransactionId = Transaction.GetNextTransactionId(),
                    Balance = (Transaction.GetBalance(Convert.ToInt32(loanAmount), Convert.ToInt32(seqNo), Convert.ToInt32(customerId)) - Convert.ToInt16(collectedAmount)), // TODO: Balance is not updaed correctly eg: 71-104 - 19th july txn.
                    TxnDate = dateTimePicker1.Value
                };

                if (txn.Balance < 0)
                {
                    MessageBox.Show("Balance is less than 0. Please check your amount. Txn Aborted!");
                    return;
                }
                if (txn.Balance == 0)
                {
                    MessageBox.Show("Good News, This txn will be closed!");
                    Customer.UpdateCustomerDetails(new Customer() { CustomerId = txn.CustomerId, CustomerSeqNumber = txn.CustomerSequenceNo, IsActive = false, ClosedDate = txn.TxnDate });
                }

                txn.IsClosed = (txn.Balance <= 0);


                // Add new Txn.
                //transactions.Add(txn);
                var existingTxn = Transaction.GetTransactionForDate(txn.CustomerId, txn.CustomerSequenceNo, txn.TxnDate);
                if (existingTxn == null || existingTxn.Count == 0)
                {
                    Transaction.AddDailyTransactions(txn);
                }
                else
                {
                    txn.TransactionId = existingTxn.First().TransactionId;
                    Transaction.UpdateTransactionDetails(txn);
                }


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


                return;
            }



            var amountGivenDate = FormGeneral.GetGridCellValue(grid, rowIndex, "AmountGivenDate");
            var closedDate = FormGeneral.GetGridCellValue(grid, rowIndex, "ClosedDate");
            var interest = FormGeneral.GetGridCellValue(grid, rowIndex, "Interest");
            var name = FormGeneral.GetGridCellValue(grid, rowIndex, "Name");

            // Update Customer Created Date.

            Customer.CorrectCustomerData(
                    new Customer()
                    {
                        CustomerId = Convert.ToInt32(customerId),
                        CustomerSeqNumber = Convert.ToInt32(seqNo),
                        AmountGivenDate = Convert.ToDateTime(amountGivenDate),
                        ClosedDate = Convert.ToDateTime(closedDate),
                        Interest = Convert.ToInt32(interest),
                        LoanAmount = Convert.ToInt32(loanAmount),
                        Name = name
                    });
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
                var customerId = FormGeneral.GetGridCellValue(dataGridView1, rowIndex, "CustomerId");
                var isActive = FormGeneral.GetGridCellValue(dataGridView1, rowIndex, "IsActive");

                strip.Tag = new Customer() { CustomerSeqNumber = Convert.ToInt32(seqNo), CustomerId = Convert.ToInt32(customerId), IsActive = Convert.ToBoolean(isActive) };

                if (rowIndex >= 0)
                {
                    strip.Items.Add("Delete Customer and Txn").Name = "All";
                    strip.Items.Add("Delete Customer only").Name = "Cus";
                    strip.Items.Add("Delete Txn only").Name = "Txn";
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
        }

        private void frmCustomers_Load(object sender, EventArgs e)
        {
            txtSearch.Focus();
        }

        private void txtSearch_Leave(object sender, EventArgs e)
        {

            dataGridView1.CurrentCell = dataGridView1.Rows[0].Cells["CollectionAmt"];
            dataGridView1.BeginEdit(true);
        }
    }
}
