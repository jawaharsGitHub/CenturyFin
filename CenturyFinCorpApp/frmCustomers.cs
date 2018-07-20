using DataAccess;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class frmCustomers : Form
    {
        private List<Customer> customers;
        private List<Transaction> transactions;

        public frmCustomers()
        {
            InitializeComponent();
            // Get the table from the data set
            SetCustomers();

            transactions = new List<Transaction>();

            

            if (customers == null) return;

            //Create New DataGridViewTextBoxColumn
            DataGridViewTextBoxColumn textboxColumn = new DataGridViewTextBoxColumn();
            textboxColumn.HeaderText = "Collection Amount";
            textboxColumn.Name = "CollectionAmt";

            dataGridView1.DataSource = customers;


            //Add TextBoxColumn dynamically to DataGridView

            dataGridView1.Columns.Add(textboxColumn);
            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);

            txtSearch.Focus();
            AdjustColumnOrder();

        }

        private void SetCustomers()
        {
            customers = Customer.GetAllCustomer().OrderBy(o => o.AmountGivenDate).ToList();

            var activeTxn = customers.Count(c => c.IsActive == true);
            var closedTxn = customers.Count(c => c.IsActive == false);
            var totalTxn = activeTxn + closedTxn;

            this.Text = $"Running Notes: {activeTxn} Closed Notes: {closedTxn} Total Notes: {totalTxn}";
        }

        private void AdjustColumnOrder()
        {            
            dataGridView1.Columns["CollectionAmt"].DisplayIndex = 3;
            dataGridView1.Columns["ModifiedDate"].Visible = false;
            dataGridView1.Columns["PhoneNumber"].Visible = false;
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedRows = (sender as DataGridView).SelectedRows;

            if (selectedRows.Count != 1) return;

            var selectedCustomer = (selectedRows[0].DataBoundItem as Customer);

            frmCustomerTransaction cd = new frmCustomerTransaction(selectedCustomer.CustomerSeqNumber, selectedCustomer.CustomerId, selectedCustomer.LoanAmount, selectedCustomer.Name, (selectedCustomer.IsActive == false));

            cd.ShowDialog();

        }

        private void rdbAll_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbAll.Checked)
            {
                dataGridView1.DataSource = customers;
            }
        }

        private void rdbClosed_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbClosed.Checked)
            {
                dataGridView1.DataSource = customers.Where(w => w.IsActive == false).OrderBy(o => o.ClosedDate).ToList();
            }
        }

        private void rdbActive_CheckedChanged(object sender, EventArgs e)
        {
            if (rdbActive.Checked)
            {
                dataGridView1.DataSource = customers.Where(w => w.IsActive == true).ToList();
            }
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (rdbActive.Checked)
                dataGridView1.DataSource = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower()) && w.IsActive == true).ToList();
            if (rdbClosed.Checked)
                dataGridView1.DataSource = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower()) && w.IsActive == false).ToList();
            if (rdbAll.Checked)
                dataGridView1.DataSource = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
        }

        private void btnAddCustomer_Click(object sender, EventArgs e)
        {
            frmAddCustomer ac = new frmAddCustomer();
            ac.ShowDialog();

        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (sender as DataGridView);
            var rowIndex = grid.CurrentCell.RowIndex;

            var seqNo = GetGridCellValue(grid, rowIndex, "CustomerSeqNumber");
            var customerId = GetGridCellValue(grid, rowIndex, "CustomerId");
            var loanAmount = GetGridCellValue(grid, rowIndex, "LoanAmount");
            var collectedAmount = GetGridCellValue(grid, rowIndex, "CollectionAmt");

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
                }

                txn.IsClosed = (txn.Balance <= 0);


                // Add new Txn.
                //transactions.Add(txn);
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


                return;
            }


            var amountGivenDate = GetGridCellValue(grid, rowIndex, "AmountGivenDate");
            var closedDate = GetGridCellValue(grid, rowIndex, "ClosedDate");
            var interest = GetGridCellValue(grid, rowIndex, "Interest");

            var name = GetGridCellValue(grid, rowIndex, "Name");

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

        private string GetGridCellValue(DataGridView grid, int rowIndex, string columnName)
        {
            var cellValue = Convert.ToString(grid.Rows[grid.CurrentCell.RowIndex].Cells[columnName].Value);
            return (cellValue == string.Empty) ? null : cellValue;

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            SetCustomers();
            rdbAll.Checked = true;
        }
    }
}
