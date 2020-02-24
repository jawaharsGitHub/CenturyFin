using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;



using System.ComponentModel;
using System.Drawing;
using System.IO;

namespace CenturyFinCorpApp
{
    public partial class frmDailyEntry : UserControl
    {

        private int ActualCollection;
        private int ExpectedCollection;

        private List<CustomerDailyTxn> result;
        private List<ExtDailyTxn> CxnHistory;

        public frmDailyEntry()
        {
            InitializeComponent();


            dateTimePicker1.Value = GlobalValue.CollectionDate.Value;

            LoadDailyCollection(dateTimePicker1.Value, true);
            LoadAllHsitoryDailyCollections();

            //lblOutStanding.Text = Transaction.GetAllOutstandingAmount().ToMoney();
            AdjustColumnOrder();

            cmdFilter.DataSource = GetOptions();
            cmbFilter.DataSource = GetDataFilters();

            cmbAmountFilter.SelectedIndexChanged += CmbAmountFilter_SelectedIndexChanged;
        }

        private void CmbAmountFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            dataGridView1.DataSource = result.Where(w => w.AmountReceived == cmbAmountFilter.SelectedValue.ToInt32()).ToList();
        }

        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(3, "ALL"),
                   new KeyValuePair<int, string>(1, "Given Customer"),
                   new KeyValuePair<int, string>(2, "Closed Customer")
               };

            return myKeyValuePair;

        }



        private void LoadAllHsitoryDailyCollections()
        {
            var data = BaseClass.ReadFileAsObjects<DailyCollectionDetail>(AppConfiguration.DailyTxnFile);

            var cus = Customer.GetAllCustomer();

            CxnHistory = (from d in data
                          select
                          new ExtDailyTxn()
                          {
                              Date = Convert.ToDateTime(d.Date).ToString("dd-MM-yyyy dddd"),
                              CollectionAmount = d.CollectionAmount,
                              GivenAmount = d.GivenAmount,
                              // ExpectedCollectionAmount = LoadDailyCollection(Convert.ToDateTime(d.Date), true) // TODO: will use when we want it.
                              Closed = cus.Where(w => w.ClosedDate != null && w.ClosedDate.Value.Date == Convert.ToDateTime(d.Date).Date).Sum(s => s.Interest),
                              New = cus.Where(w => w.AmountGivenDate.Value.Date == Convert.ToDateTime(d.Date).Date).Sum(s => s.Interest)
                          }).OrderByDescending(o => Convert.ToDateTime(o.Date)).ToList();

            var max = CxnHistory.OrderBy(o => o.CollectionAmount).Last();
            var maxClosed = CxnHistory.OrderBy(o => o.Closed).Last();


            lblMax.Text = $"Max Cxn - {max.CollectionAmount}({maxClosed.Closed}) on {max.Date}";

            // Customer Collectin Average By Day.
            var averagePerDay = (from r in CxnHistory
                                 group r by Convert.ToDateTime(r.Date).DayOfWeek into newGroup
                                 select new
                                 {
                                     Day = newGroup.Key,
                                     CxnAvg = newGroup.Average(a => a.CollectionAmount).RoundMoneyOnly(),
                                     GivenAvg = newGroup.Average(a => a.GivenAmount).RoundMoneyOnly()

                                 }).OrderByDescending(o => o.CxnAvg).ToList();


            dgAvgPerDay.DataSource = averagePerDay;

            btnAvgPerDay.Text = averagePerDay.Average(a => a.CxnAvg).TokFormat();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDailyCollection(dateTimePicker1.Value);
        }

        private void AdjustColumnOrder()
        {
            dataGridView1.Columns["CustomerId"].Visible = false;
            dataGridView1.Columns["CustomerSeqId"].Visible = false;
            dataGridView1.Columns["Interest"].Visible = false;
            dataGridView1.Columns["CustomerName"].Visible = false;
            dataGridView1.Columns["TxnDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
        }


        private int? LoadDailyCollection(DateTime chooseDate, bool versionZero = false)
        {
            var txn = new List<Transaction>();

            if (versionZero)
            {
                txn = Transaction.GetDailyCollectionDetails_V0(chooseDate);
            }
            else
            {
                txn = Transaction.GetDailyCollectionDetails(chooseDate);
            }


            var cus = (from c in Customer.GetAllCustomer()
                       where c.AmountGivenDate.Value.Date <= chooseDate.Date && (c.ClosedDate == null || c.ClosedDate.Value.Date >= chooseDate.Date)
                       select c).ToList();


            if (txn != null)
            {
                result = (from t in txn
                          join c in cus
                          on t.CustomerSequenceNo equals c.CustomerSeqNumber
                          select new CustomerDailyTxn
                          {
                              TransactionId = t.TransactionId,
                              TxnDate = t.TxnDate,
                              CustomerName = c.Name,
                              Name = string.IsNullOrEmpty(c.TamilName) ? c.Name : c.TamilName,
                              Loan = c.LoanAmount,
                              CSId = c.CustomerSeqNumber,
                              AmountReceived = t.AmountReceived,
                              Balance = t.Balance,
                              CustomerId = c.CustomerId,
                              CustomerSeqId = c.CustomerSeqNumber,
                              Interest = c.Interest,
                              ReturnType = c.ReturnType
                          }).Distinct().ToList();


                cmbAmountFilter.DataSource = result.DistinctBy(d => d.AmountReceived).Select(s => s.AmountReceived).ToList();
            }

            var amountReceived = result.Where(w => w.AmountReceived > 0).Sum(s => s.AmountReceived);

            ActualCollection = amountReceived;
            ExpectedCollection = (cus.Where(w => w.AmountGivenDate.Value.Date != chooseDate.Date && w.IsNotMonthly()).Sum(s => s.LoanAmount) / 100);

            label1.Text = $"Total Collection is: {amountReceived.ToMoneyFormat()}";

            // Topup details.
            var topupCustomers = TopupCustomer.GetAllTopupCustomer().Where(w => w.AmountGivenDate.Value.Date == dateTimePicker1.Value.Date).ToList();

            var newText = "";

            if (topupCustomers.Count > 0)
            {
                newText = $"NEW:{result.Count(c => c.AmountReceived == 0 && c.Balance > 0)}  ({result.Where(w => w.AmountReceived == 0 && w.Balance > 0).Sum(s => s.Interest).ToMoneyFormat()}) +" +
                    $" {topupCustomers.Count()}Topup ({topupCustomers.Sum(s => s.Interest)}) = {(topupCustomers.Sum(s => s.Interest) + result.Where(w => w.AmountReceived == 0 && w.Balance > 0).Sum(s => s.Interest)).ToMoneyFormat()}";
            }
            else
            {
                newText = $"NEW:{result.Count(c => c.AmountReceived == 0 && c.Balance > 0)} ({result.Where(w => w.AmountReceived == 0 && w.Balance > 0).Sum(s => s.Interest).ToMoneyFormat()})";
            }

            label2.Text = $"{result.Count(c => c.AmountReceived > 0)} (Rs.{amountReceived.ToMoneyFormat()}) customers paid out of {cus.Count()} (Rs.{ExpectedCollection.ToMoneyFormat()}) {Environment.NewLine}" +
                $"CLOSED:{result.Count(c => c.Balance == 0)} ({result.Where(w => w.Balance == 0).Sum(s => s.Interest).ToMoneyFormat()}) " +
                newText;


            dataGridView1.DataSource = result;
            dataGridView1.Columns["AmountReceived"].ReadOnly = false;
            dataGridView1.Columns["Name"].DisplayIndex = 0;
            dataGridView1.Columns["AmountReceived"].DisplayIndex = 1;
            dataGridView1.Columns["Balance"].DisplayIndex = 2;

            lblCollSpot.Text = $"Went to {result.GroupBy(g => g.CSId).Count()} place to collect?";

            return ExpectedCollection;
        }

        private void LoadNotGivenCustomer()
        {
            var txn = Transaction.GetDailyCollectionDetails(dateTimePicker1.Value).Select(s => s.CustomerSequenceNo).ToList();
            var activeCustomers = Customer.GetAllCustomer().Where(w => w.IsActive == true).ToList();

            var cus = (from c in activeCustomers
                       where txn.Contains(c.CustomerSeqNumber) == false
                       select new { c.CustomerId, c.Name, c.IsActive, c.LoanAmount, c.Interest }).ToList();

            label2.Text = $"{cus.Count()} customers NOT PAID out of {activeCustomers.Count} (MISSING AMOUNT: Rs.{(cus.Sum(s => s.LoanAmount) / 100)})";

            dataGridView1.DataSource = cus;
        }

        private void button2_Click(object sender, EventArgs e)
        {
            frmInHand fd = new frmInHand();
            //fd.ShowDialog();
        }

        private void dataGridView1_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            var selectedRows = (sender as DataGridView).SelectedRows;

            if (selectedRows.Count != 1) return;

            var selectedCustomer = (selectedRows[0].DataBoundItem as dynamic);

            var mainForm = (frmIndexForm)(((DataGridView)sender).Parent.Parent.Parent); //new frmIndexForm(true);

            mainForm.ShowForm(selectedCustomer.TransactionId);

        }

        private void chkNotGivenCustomer_CheckedChanged(object sender, EventArgs e)
        {
            if (chkNotGivenCustomer.Checked)
            {
                // Load Not Given CUstomer;
                dataGridView1.DataSource = null;
                LoadNotGivenCustomer();
            }
            else
            {
                LoadDailyCollection(dateTimePicker1.Value);
            }
        }

        private void button3_Click(object sender, EventArgs e)
        {
            LoadDailyCollection(dateTimePicker1.Value, true);
        }

        private void dataGridView1_CellEndEdit(object sender, DataGridViewCellEventArgs e)
        {
            var grid = (sender as DataGridView);
            var rowIndex = grid.CurrentCell.RowIndex;

            var seqNo = FormGeneral.GetGridCellValue(grid, rowIndex, "CustomerSeqId");
            var customerId = FormGeneral.GetGridCellValue(grid, rowIndex, "CustomerId");
            var txnId = FormGeneral.GetGridCellValue(grid, rowIndex, "TransactionId");
            var amountReceived = FormGeneral.GetGridCellValue(grid, rowIndex, "AmountReceived");

            Transaction.UpdateTransactionDetails(new Transaction()
            {
                AmountReceived = amountReceived.ToInt32(),
                TransactionId = txnId.ToInt32(),
                CustomerId = customerId.ToInt32(),
                CustomerSequenceNo = seqNo.ToInt32()
            });

        }

        private void btnNext_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
            LoadDailyCollection(dateTimePicker1.Value, true);
        }

        private void btnPrev_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);
            LoadDailyCollection(dateTimePicker1.Value, true);
        }


        // Shows the sum of previous txns of the selected txns.
        private void dataGridView1_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var neededRow = (CustomerDailyTxn)((sender as DataGridView).Rows[e.RowIndex]).DataBoundItem;

            var sum = result.Where(w => w.TransactionId <= neededRow.TransactionId).Sum(s => s.AmountReceived);

            dataGridView1.Rows[e.RowIndex].Cells["AmountReceived"].ToolTipText = sum.ToString();
        }

        private void txtSearch_TextChanged(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtSearch.Text) == false)
            {
                var searchResult = (from r in result
                                    where r.CustomerName.ToLower().Contains(txtSearch.Text.ToLower())
                                    select r).ToList();

                dataGridView1.DataSource = searchResult;

            }
            else
            {
                dataGridView1.DataSource = result;
            }
        }

        private void cmdFilter_SelectedIndexChanged(object sender, EventArgs e)
        {
            int value = cmdFilter.SelectedValue.ToInt32();

            var filteredData = result;

            if (value == 1)
            {
                dataGridView1.DataSource = filteredData.Where(w => w.AmountReceived == 0 && w.Balance > 0).ToList();

            }
            else if (value == 2)
            {
                dataGridView1.DataSource = filteredData.Where(w => w.Balance == 0).ToList();

            }
            else //if (value == 3)
            {
                dataGridView1.DataSource = result;

            }


        }

        public static List<KeyValuePair<int, string>> GetDataFilters()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "Daily"),
                   new KeyValuePair<int, string>(2, "Weekly"),
                   new KeyValuePair<int, string>(3, "Monthly"),
                   new KeyValuePair<int, string>(4, "Yearly")
               };

            return myKeyValuePair;

        }

        private void cmbFilter_SelectedIndexChanged(object sender, EventArgs e)
        {

            int value = cmbFilter.SelectedValue.ToInt32();

            var filteredData = CxnHistory;

            if (value == 1)
            {
                dgvAllDailyCollection.DataSource = filteredData;

            }
            else if (value == 2)
            {
                var data = (from r in filteredData
                            group r by Convert.ToDateTime(r.Date).StartOfWeek(DayOfWeek.Monday) into newGroup
                            select new
                            {
                                Date = $"{newGroup.First().Date} to {newGroup.Last().Date}",
                                CxnCount = newGroup.Count(),
                                Closed = newGroup.Sum(s => s.Closed),
                                CollectionAmount = newGroup.Sum(s => s.CollectionAmount).ToMoney(),
                                GivenAmount = newGroup.Sum(s => s.GivenAmount).ToMoney(),
                                New = newGroup.Sum(s => s.New)

                            }).ToList();

                dgvAllDailyCollection.DataSource = data.ToList();

            }
            else if (value == 3)
            {
                var data = (from r in filteredData
                            group r by new { month = Convert.ToDateTime(r.Date).Month, year = Convert.ToDateTime(r.Date).Year } into newGroup
                            select new
                            {
                                Date = $"{newGroup.Key.month}/{newGroup.Key.year}",
                                CxnCount = newGroup.Count(),
                                Closed = newGroup.Sum(s => s.Closed),
                                CollectionAmount = newGroup.Sum(s => s.CollectionAmount).ToMoney(),
                                GivenAmount = newGroup.Sum(s => s.GivenAmount).ToMoney(),
                                New = newGroup.Sum(s => s.New)

                            }).ToList();

                dgvAllDailyCollection.DataSource = data.ToList();

            }

            else //if (value == 3)
            {
                var data = (from r in filteredData
                            group r by Convert.ToDateTime(r.Date).Year into newGroup
                            select new
                            {
                                Date = newGroup.Key,
                                CxnCount = newGroup.Count(),
                                Closed = newGroup.Sum(s => s.Closed),
                                CollectionAmount = newGroup.Sum(s => s.CollectionAmount).ToMoney(),
                                GivenAmount = newGroup.Sum(s => s.GivenAmount).ToMoney(),
                                New = newGroup.Sum(s => s.New)

                            }).ToList();

                dgvAllDailyCollection.DataSource = data.ToList();

            }

            dgvAllDailyCollection.Columns["New"].DefaultCellStyle.Format = "N0";

        }

        private void btnEmail_Click(object sender, EventArgs e)
        {
            // WhatsAppMessage.SendMsg();

            if (General.CheckForInternetConnection() == false)
            {
                MessageBox.Show("No Internet Available, Please connect and try again!");
                return;
            }

            BackgroundWorker bw = new BackgroundWorker();
            //this.Controls.Add(bw);

            /* 1.Transaction Image*/
            int height = dataGridView1.Height;
            dataGridView1.Height = (dataGridView1.RowCount * dataGridView1.RowTemplate.Height) + 100;
            Bitmap bitmapTxn = new Bitmap(this.dataGridView1.Width, this.dataGridView1.Height);
            dataGridView1.DrawToBitmap(bitmapTxn, new Rectangle(0, 0, this.dataGridView1.Width, this.dataGridView1.Height));
            //Resize DataGridView back to original height.
            dataGridView1.Height = height;

            /* 2.Name Image*/
            //Bitmap bitmapName = new Bitmap(this.btnCusName.Width, this.btnCusName.Height);
            //btnCusName.DrawToBitmap(bitmapName, new Rectangle(0, 0, this.btnCusName.Width, this.btnCusName.Height));

            bw.DoWork += (s, o) =>
            {

                /* 3.Merge 2 images*/
                Bitmap firstTxn = bitmapTxn;
                //Bitmap secondName = bitmapName;

                Bitmap result = new Bitmap(firstTxn.Width, firstTxn.Height + 30);
                Graphics g = Graphics.FromImage(result);
                g.DrawImageUnscaled(firstTxn, 0, 30);
                //g.DrawImageUnscaled(secondName, 0, 0);

                var txnFileName = $"{Path.GetTempPath()}Tuesday-Cxn.jpg";
                result.Save(txnFileName); // Save File
                AppCommunication.SendCustomerTxnEmail("Tuesday Cxn", DateTime.Today, txnFileName); // Email
                //File.Delete(txnFileName);
                MessageBox.Show("Txn Email Send!");

            };
            bw.RunWorkerAsync();
        }

        private void chkHide_CheckedChanged(object sender, EventArgs e)
        {

            dataGridView1.Columns[1].Visible = chkHide.Checked;
            dataGridView1.Columns[2].Visible = chkHide.Checked;
            dataGridView1.Columns[5].Visible = chkHide.Checked;
            dataGridView1.Columns[6].Visible = chkHide.Checked;

            dataGridView1.Columns[3].Visible = !chkHide.Checked;
            dataGridView1.Columns[4].Visible = !chkHide.Checked;
            dataGridView1.Columns[7].Visible = !chkHide.Checked;
            dataGridView1.Columns[11].Visible = !chkHide.Checked;

            dataGridView1.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;
        }
    }


}
