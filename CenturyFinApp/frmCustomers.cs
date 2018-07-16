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

        public frmCustomers()
        {
            InitializeComponent();
            // Get the table from the data set
            customers = Customer.GetAllCustomer().OrderBy(o => o.AmountGivenDate).ToList();

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
            dataGridView1.DataSource = customers.Where(w => w.Name.ToLower().Contains(txtSearch.Text.ToLower())).ToList();
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            var txns = new List<Transaction>();

            foreach (DataGridViewRow row in dataGridView1.Rows)
            {

                txns.Add(new Transaction()
                {

                    //TxnCustomer = (row.DataBoundItem as Customer),
                    AmountReceived = Convert.ToInt32(row.Cells["CollectionAmt"].Value),
                    TxnDate = dateTimePicker1.Value

                });


            }

            //Transaction txn = new Transaction();

            Transaction.AddDailyTransactions(txns);
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
            var amountGivenDate = GetGridCellValue(grid, rowIndex, "AmountGivenDate");
            var closedDate = GetGridCellValue(grid, rowIndex, "ClosedDate");
            var interest = GetGridCellValue(grid, rowIndex, "Interest");
            var loanAmount = GetGridCellValue(grid, rowIndex, "LoanAmount");
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
            return Convert.ToString(grid.Rows[grid.CurrentCell.RowIndex].Cells[columnName].Value);

        }

        private void chkOrderByStartDate_CheckedChanged(object sender, EventArgs e)
        {

            if (chkOrderByStartDate.Checked)
                dataGridView1.DataSource = customers.OrderBy(o => o.ClosedDate).ToList();
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
