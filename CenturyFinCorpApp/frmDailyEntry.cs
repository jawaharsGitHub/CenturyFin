using DataAccess;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmDailyEntry : UserControl
    {
        public frmDailyEntry()
        {
            InitializeComponent();

            if (Convert.ToBoolean(ConfigurationManager.AppSettings["usingMenu"]) == true)
                button2.Visible = false;
            else
                button2.Visible = true;

            chkAddSalary.Checked = true; // will callCalculateIncome(true);
            LoadDailyCollection();
            lblOutStanding.Text = Transaction.GetAllOutstandingAmount().ToMoney();
            //CalculateIncome(true);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            LoadDailyCollection();
        }

        private void LoadDailyCollection()
        {
            var txn = Transaction.GetDailyCollectionDetails(dateTimePicker1.Value);

            var cus = from c in Customer.GetAllCustomer()
                      select new { c.CustomerId, c.Name, c.IsActive, c.Interest, c.LoanAmount };

            var result = (from t in txn
                          join c in cus
                          on t.CustomerId equals c.CustomerId
                          select new
                          {
                              t.TransactionId,
                              t.TxnDate,
                              c.Name,
                              t.AmountReceived,
                              t.Balance
                          }).Distinct();

            var amountReceived = result.Sum(s => s.AmountReceived);


            result = result.Where(w => w.AmountReceived != 0).ToList();

            label1.Text = $"Total Collection is: {amountReceived}";
            label2.Text = $"{result.Count()} (Rs.{amountReceived}) customers paid out of {cus.Count(c => c.IsActive)} (Rs.{(cus.Where(w => w.IsActive).Sum(s => s.LoanAmount) / 100)})";

            dataGridView1.DataSource = result;
        }

        private void CalculateIncome(bool considerSalary = false)
        {

            // Closed Account
            var closedCustomers = (from c in Customer.GetAllCustomer()
                                   where c.IsActive == false
                                   group c by Convert.ToDateTime(c.ClosedDate).ToShortDateString() into newGroup
                                   select new
                                   {
                                       ClosedDate = newGroup.Key,
                                       TotalInterest = newGroup.Sum(s => s.Interest),
                                       IsExpectedIncome = false
                                   }).OrderBy(o => Convert.ToDateTime(o.ClosedDate)).ToList();

            // Running Account (Expected Income)
            var runningCustomers = (from c in Customer.GetAllCustomer()
                                    where c.IsActive == true
                                    group c by Convert.ToDateTime(c.AmountGivenDate).ToShortDateString() into newGroup
                                    select new
                                    {
                                        ClosedDate = (Convert.ToDateTime(newGroup.Key).AddDays(100)).ToShortDateString(), // TODO: it should be group by given data + 100 days not by key(July 2018)
                                        TotalInterest = newGroup.Sum(s => s.Interest),
                                        IsExpectedIncome = true
                                    }).OrderBy(o => Convert.ToDateTime(o.ClosedDate)).ToList();

            // Merge Both result
            closedCustomers.AddRange(runningCustomers);

            // group by eclosed date and income type
            var data = (from x in closedCustomers
                        group x by new { ClosedMonth = Convert.ToDateTime(x.ClosedDate).ToString("Y"), IsExpectedIncome = x.IsExpectedIncome } into newGroup
                        select newGroup).ToList();

            var finalData = new List<IncomeReport>();


            data.ForEach(f =>
            {

                f.ToList().ForEach(d =>
                {
                    var existData = finalData.Where(w => w.Month == Convert.ToDateTime(d.ClosedDate).ToString("Y")).FirstOrDefault();

                    if (existData == null) // Not exist
                    {
                        existData = new IncomeReport() { Month = Convert.ToDateTime(d.ClosedDate).ToString("Y") };
                        finalData.Add(existData);
                    }

                    if (d.IsExpectedIncome)
                        existData.ExpectedIncome += d.TotalInterest;
                    else
                        existData.ActualIncome += d.TotalInterest;


                });


            });

            finalData.Insert(0, new IncomeReport()
            {
                Month = "2018 Feb",
                ActualIncome = 0
            });

            // Move past month expected to current month expected.
            var pastMonthExpectedIncome = (from pm in finalData
                                           where Convert.ToDateTime(pm.Month).Month < DateTime.Now.Month
                                           select pm);


            var currentMonthExpectedIncome = finalData.Where(w => Convert.ToDateTime(w.Month).Month == DateTime.Now.Month).FirstOrDefault();
            if (currentMonthExpectedIncome != null) currentMonthExpectedIncome.ExpectedIncome += pastMonthExpectedIncome.Sum(s => s.ExpectedIncome);

            pastMonthExpectedIncome.ToList().ForEach(f => f.ExpectedIncome = 0);

            // Consider if salary also!
            if (considerSalary)
            {
                finalData.ForEach(fd =>
                {
                    if (Convert.ToDateTime(fd.Month).Month == 2)
                    {
                        fd.ActualIncome = (fd.ExpectedIncome - fd.MonthlySalary); // Always fr feb, actualincome is -salary
                    }
                    else if ((fd.ActualIncome > 0 && fd.ExpectedIncome > 0) || fd.ActualIncome > 0)
                    {
                        fd.ActualIncome = (fd.ActualIncome - fd.MonthlySalary);
                    }
                    else
                    {
                        fd.ExpectedIncome = (fd.ExpectedIncome - fd.MonthlySalary);
                    }

                });

            }
            // Bindng the real/actual source finalData (will be updated using by ref by various data)
            dgvIncome.DataSource = finalData;

            // Years Expected and Actual Salary

            var actual = finalData.Sum(w => w.ActualIncome);
            var expected = finalData.Sum(w => w.ExpectedIncome);
            var total = (actual + expected);

            lblActual.Text = $"Actual :  {actual.ToMoney()} (Per Month: { (actual / DateTime.Today.Month).ToMoney()})";
            lblExpected.Text = $"Ëxpected : {expected.ToMoney()} (Per Month: { (expected / DateTime.Today.Month).ToMoney()})";
            lblTotal.Text = $"TOTAL : {total.ToMoney()} (Per Month: { (total / DateTime.Today.Month).ToMoney()})";
        }

        private void chkAddSalary_CheckedChanged(object sender, EventArgs e)
        {
            CalculateIncome(chkAddSalary.Checked);
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
    }


}
