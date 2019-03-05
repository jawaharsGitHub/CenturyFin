using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management.Automation;
using System.Text;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmGeneralReport : UserControl
    {
        (int actual, int includesProfit) outstandingMoney;
        List<IncomeReport> finalData;

        public frmGeneralReport()
        {
            InitializeComponent();
            chkAddSalary.Checked = true; // will callCalculateIncome(true);
            CustomerGrowth();

            ShowRemaingDays();

            GetCustomerCount();

            ShowOutstandingMoney();
            ShowTotalAssetMoney();
        }

        private void GetCustomerCount()
        {
            var count = Customer.GetAllCustomer().Select(s => s.CustomerId).Distinct().Count();
            btnCustomerCount.Text = $"Customer Count - {count.ToString()}";

        }

        private void ShowRemaingDays()
        {

            var data = DateHelper.GetRemaingDaysToNextCycle();

            label1.Text = $"Remaining Days to start next month ({data.NextMonthName}) cycle: {data.NoOfDays}";
            label2.Text = $"Remaining {DateHelper.RemaingDaysOfMonth} Days to go in this month:";
        }

        private void CalculateIncome(bool considerSalary = false)
        {
            finalData = new List<IncomeReport>();

            // Closed Account
            var closedCustomers = (from c in Customer.GetAllCustomer()
                                   where c.IsActive == false
                                   group c by Convert.ToDateTime(c.ClosedDate).ToShortDateString() into newGroup
                                   select new
                                   {
                                       ClosedDate = newGroup.Key,
                                       TotalInterest = newGroup.Sum(s => s.Interest),
                                       IsExpectedIncome = false,
                                       //newGroup,
                                       Count = newGroup.Count()
                                   }).OrderBy(o => Convert.ToDateTime(o.ClosedDate)).ToList();

            // Running Account (Expected Income)
            var runningCustomers = (from c in Customer.GetAllCustomer()
                                    where c.IsActive == true
                                    group c by Convert.ToDateTime(c.AmountGivenDate).ToShortDateString() into newGroup
                                    select new
                                    {
                                        ClosedDate = (Convert.ToDateTime(newGroup.Key).AddDays(100)).ToShortDateString(), // TODO: it should be group by given data + 100 days not by key(July 2018)
                                        TotalInterest = newGroup.Sum(s => s.Interest),
                                        IsExpectedIncome = true,
                                        //newGroup,
                                        Count = newGroup.Count()
                                    }).OrderBy(o => Convert.ToDateTime(o.ClosedDate)).ToList();


            // Merge Both result
            closedCustomers.AddRange(runningCustomers);

            // group by closed date and income type
            var data = (from x in closedCustomers
                        group x by new { ClosedMonth = Convert.ToDateTime(x.ClosedDate).ToString("Y"), IsExpectedIncome = x.IsExpectedIncome } into newGroup
                        select newGroup).ToList();


            StringBuilder closedDetailForCurrentMonth = new StringBuilder();
            closedDetailForCurrentMonth.Append($"{Environment.NewLine}For {DateTime.Now.ToString("MMMM")}");

            var moveOverClosed = 0;
            var moveOverInterest = 0;

            var ddd = 0;
            data.ForEach(f =>
            {

                IncomeReport existData = null;

                f.ToList().ForEach(d =>
                {
                    existData = finalData.Where(w => w.MonthYear == Convert.ToDateTime(d.ClosedDate).ToString("Y")).FirstOrDefault();

                    if (existData == null) // Not exist
                    {
                        existData = new IncomeReport() { MonthYear = Convert.ToDateTime(d.ClosedDate).ToString("Y") };
                        finalData.Add(existData);
                    }

                    if (d.IsExpectedIncome)
                    {
                        existData.ExpectedIncome += d.TotalInterest;
                        ddd += d.Count;

                    }
                    else
                    {
                        existData.ActualIncome += d.TotalInterest;
                        ddd += d.Count;
                    }


                });

                var closedData = f.Sum(s => s.Count);


                if (f.Key.IsExpectedIncome && Convert.ToDateTime(f.Key.ClosedMonth).ToString("yyyyMM").ToInt32() < DateTime.Today.ToString("yyyyMM").ToInt32())
                //(DateTime.Today.Month > Convert.ToDateTime(f.Key.ClosedMonth).Month && DateTime.Today.Year >= Convert.ToDateTime(f.Key.ClosedMonth).Year))
                {
                    moveOverClosed += closedData;
                    moveOverInterest += f.Sum(s => s.TotalInterest);
                }
                else
                {
                    existData.CloseCount += closedData;
                }


                if (DateTime.Today.Month == Convert.ToDateTime(f.Key.ClosedMonth).Month && DateTime.Today.Year == Convert.ToDateTime(f.Key.ClosedMonth).Year)
                {
                    var totalInt = f.Sum(s => s.TotalInterest);
                    if (f.Key.IsExpectedIncome)
                    {
                        //var 
                        closedDetailForCurrentMonth.Append($" {Environment.NewLine}Expected Close: {closedData.ToString()} [{totalInt}] {Environment.NewLine} carry fwd close: {moveOverClosed.ToString()} [{moveOverInterest}] = {closedData + moveOverClosed}");
                    }
                    else
                    {
                        //var details = f.Select(s => s.newGroup).ToList();
                        //var cus = new List<Customer>();

                        //details.ForEach(fe =>
                        //{
                        //    cus.AddRange(fe.Select(s => s).ToList());
                        //});



                        closedDetailForCurrentMonth.Append($" Actual Close: {closedData} [{totalInt}]");
                    }

                    existData.CloseCount += moveOverClosed;
                }


            });

            finalData.Insert(0, new IncomeReport()
            {
                MonthYear = "February 2018",
                ActualIncome = 0,
                CloseCount = 0
            });

            // Move past month expected to current month expected.
            var pastMonthExpectedIncome = (from pm in finalData
                                           where
                                           //DateTime.Today.AddDays(-1).ToString("yyyyMM");
                                           //(Convert.ToDateTime(pm.MonthYear).Month < DateTime.Now.Month && 
                                           //Convert.ToDateTime(pm.MonthYear).Year < DateTime.Now.Year
                                           Convert.ToDateTime(pm.MonthYear).ToString("yyyyMM").ToInt32() < DateTime.Today.ToString("yyyyMM").ToInt32()
                                           select pm);


            // Done: Need generic fix for this calculation!!!!
            var currentMonthExpectedIncome = (from d in finalData
                                              where
                                              d.MonthYear == DateTime.Today.ToString("Y")
                                              select d).FirstOrDefault();

            if (currentMonthExpectedIncome != null) currentMonthExpectedIncome.ExpectedIncome += pastMonthExpectedIncome.Sum(s => s.ExpectedIncome);

            pastMonthExpectedIncome.ToList().ForEach(f => f.ExpectedIncome = 0);

            // Consider if salary also!
            if (considerSalary)
            {
                finalData.ForEach(fd =>
                {
                    if (Convert.ToDateTime(fd.MonthYear).Month == 2 && Convert.ToDateTime(fd.MonthYear).Year == 2018)
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
            var salary = finalData.Sum(w => w.MonthlySalary);
            var total = (actual + expected);

            var numberOfMonths = DateTime.Today.Subtract(new DateTime(2018, 1, 25)).Days / (365.25 / 12).ToInt32();

            lblActual.Text = $"Actual :  {actual.ToMoney()} (Per Month: { (actual / numberOfMonths).ToMoney()})";
            lblExpected.Text = $"Ëxpected : {expected.ToMoney()} (Per Month: { (expected / numberOfMonths).ToMoney()})";
            lblTotal.Text = $"TOTAL : {total.ToMoney()} (Per Month: { (total / numberOfMonths).ToMoney()})";
            lblCloseCount.Text = $"Close Count should be {finalData.Sum(w => w.CloseCount)}  {closedDetailForCurrentMonth}";

            lblSalary.Text = $"Salary : {salary}";
            lblSalary.Visible = considerSalary;


        }

        private void chkAddSalary_CheckedChanged(object sender, EventArgs e)
        {
            CalculateIncome(chkAddSalary.Checked);
        }

        private void CustomerGrowth()
        {
            var customers = (from c in Customer.GetAllCustomer()
                             orderby c.AmountGivenDate
                             group c by c.AmountGivenDate.Value.ToString("Y") into newGroup

                             select new
                             {
                                 Month = newGroup.Key,
                                 GivenCount = newGroup.Count(),
                                 LoanAmount = newGroup.Sum(s => s.LoanAmount),
                                 GivenAmount = newGroup.Sum(s => (s.LoanAmount - s.Interest)),
                                 FutureInterest = newGroup.Sum(s => s.Interest),
                             }).ToList();


            dgvNotePerMonth.DataSource = customers;

            var totalLoanAmount = customers.Sum(s => s.LoanAmount);
            var totalGivenAmount = customers.Sum(s => s.GivenAmount);
            var difference = totalLoanAmount - totalGivenAmount;


            label6.Text = $"LA: {totalLoanAmount.ToMoney()} {Environment.NewLine}" +
                $"GA: {totalGivenAmount.ToMoney()} (~{difference.ToMoney()}) {Environment.NewLine}" +
                $"FI: {customers.Sum(s => s.FutureInterest).ToMoney()} {Environment.NewLine}" +
                $"C: {customers.Sum(s => s.GivenCount)} {Environment.NewLine}";

        }

        private void ShowOutstandingMoney()
        {
            outstandingMoney = Transaction.GetAllOutstandingAmount();
            lblOutStanding.Text = outstandingMoney.includesProfit.ToMoney();
        }

        private void ShowTotalAssetMoney()
        {
            var inHandAndBank = InHandAndBank.GetAllhandMoney();
            lblTotalAsset.Visible = label4.Visible = false;
            lblTotalAsset.Text = $"{(outstandingMoney.includesProfit + inHandAndBank.InHandAmount + inHandAndBank.InBank).ToMoney()} (OS: {outstandingMoney.includesProfit.ToMoney()} IH: {inHandAndBank.InHandAmount.ToMoney()} IB: {inHandAndBank.InBank.ToMoney()} Actual Outstanding: {outstandingMoney.actual.ToMoney()})";
            var monthlyCustomersBalance = (from c in Customer.GetAllCustomer().Where(w => w.IsActive && w.ReturnType == DataAccess.ExtendedTypes.ReturnTypeEnum.Monthly)
                                           select Transaction.GetBalance(c)).Sum();

            var fullInvestment = DailyCollectionDetail.GetActualInvestmentTxnDate();

            lblBizAsset.Text = $"{(outstandingMoney.includesProfit + inHandAndBank.InHandAmount).ToMoney()} " +
                $"(OS: {outstandingMoney.includesProfit.ToMoney()} + IH: {inHandAndBank.InHandAmount.ToMoney()})  {Environment.NewLine} " +
                $"Actual Outstanding: {outstandingMoney.actual.ToMoney()} {Environment.NewLine} " +
                $"INVESTMENT: Daily:~{(fullInvestment - monthlyCustomersBalance).ToMoney()} + Monthly:{monthlyCustomersBalance.ToMoney()} = {fullInvestment.ToMoney()}";
        }

        private void dgvIncome_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            if (this.dgvIncome.Columns["MonthYear"] == null) return;
            if (e.RowIndex >= 0 && e.ColumnIndex == this.dgvIncome.Columns["MonthYear"].Index)
            {
                if (e.Value != null && Convert.ToDateTime(e.Value).ToString("Y") == DateTime.Now.ToString("Y"))
                {

                    dgvIncome.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Orange;
                    dgvIncome.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;

                }
            }
        }

        private void btnCommit_Click(object sender, EventArgs e)
        {
            string directory = ""; // directory of the git repository

            using (PowerShell powershell = PowerShell.Create())
            {
                // this changes from the user folder that PowerShell starts up with to your git repository
                powershell.AddScript(String.Format(@"cd {0}", directory));

                powershell.AddScript(@"git init");
                powershell.AddScript(@"git add *");
                powershell.AddScript(@"git commit -m 'git commit from PowerShell in C# - TESTING'");
                powershell.AddScript(@"git push");

                Collection<PSObject> results = powershell.Invoke();
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (comboBox1.Text.Trim().ToLower() == "all")
            {
                dgvIncome.DataSource = finalData;
            }
            else
            {
                dgvIncome.DataSource = finalData.Where(w => Convert.ToDateTime(w.MonthYear).Year.ToString() == comboBox1.Text).ToList();
            }

        }
    }
}
