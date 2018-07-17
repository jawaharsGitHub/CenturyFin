﻿using DataAccess;
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
            customers = Customer.GetAllCustomer().OrderBy(o => o.AmountGivenDate).ToList();

            transactions = new List<Transaction>();

            var activeTxn = customers.Count(c => c.IsActive == true);
            var closedTxn = customers.Count(c => c.IsActive == false);
            var totalTxn = activeTxn + closedTxn;

            this.Text = $"Running Notes: {activeTxn} Closed Notes: {closedTxn} Total Notes: {totalTxn}";

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
                dataGridView1.DataSource = customers.Where(w => w.IsActive == false).ToList();
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

        private void btnSave_Click(object sender, EventArgs e)
        {
            Transaction.AddDailyTransactions(transactions);

            transactions.ForEach(t =>
            {
                if (t.IsClosed && t.Balance == 0)
                {
                    // Update Closed Date
                    Customer.UpdateCustomerClosedDate(
                        new Customer()
                        {
                            CustomerId = t.CustomerId,
                            CustomerSeqNumber = t.CustomerSequenceNo,
                            ClosedDate = t.TxnDate,
                        });
                }
            });

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
                    Balance = (Transaction.GetBalance(Convert.ToInt32(loanAmount), Convert.ToInt32(seqNo), Convert.ToInt32(customerId)) - Convert.ToInt16(collectedAmount)),
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
                transactions.Add(txn);
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

        private void chkOrderByStartDate_CheckedChanged(object sender, EventArgs e)
        {

            if (chkOrderByStartDate.Checked)
                dataGridView1.DataSource = customers.Where(w => w.ClosedDate != null && w.ClosedDate != DateTime.MinValue).OrderBy(o => o.ClosedDate).ToList();
            else
                dataGridView1.DataSource = customers;


        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            customers = Customer.GetAllCustomer().OrderBy(o => o.AmountGivenDate).ToList();
            this.dataGridView1.DataSource = customers;
        }
    }
}