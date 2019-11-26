using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
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
        List<IncomeReport> filteredfinalData;
        int varActualClose;

        public frmGeneralReport()
        {
            InitializeComponent();
            chkAddSalary.Checked = true; // will callCalculateIncome(true);
            CustomerGrowth();

            ShowRemaingDays();

            GetCustomerCount();

            ShowOutstandingMoney();
            ShowTotalAssetMoney();

            comboBox1.Text = DateTime.Today.Year.ToString();
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
                        closedDetailForCurrentMonth.Append($" {Environment.NewLine}Expected Close(EC): {closedData.ToString()} [{totalInt.TokFormat()}] {Environment.NewLine}Carry Fwd Close(CFC): {moveOverClosed.ToString()} [{moveOverInterest.TokFormat()}] = {(varActualClose + closedData + moveOverClosed).TokFormat()}");
                    }
                    else
                    {
                        closedDetailForCurrentMonth.Append($" Actual Close(AC): \t\t\t\t {closedData} [{totalInt.TokFormat()}]");

                        varActualClose = closedData;
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


            // Years Expected and Actual Salary
            var actual = finalData.Sum(w => w.ActualIncome);
            var expected = finalData.Sum(w => w.ExpectedIncome);
            var salary = finalData.Sum(w => w.MonthlySalary);


            var numberOfMonths = DateTime.Today.Subtract(new DateTime(2018, 1, 25)).Days / (365.25 / 12).ToInt32();

            // Monthly INT incomes.

            var monthlyTxns = Customer.GetAllCustomer().Where(w => w.IsMonthly()).ToList();

            var expectedMonthly = monthlyTxns.Where(w => w.IsActive == true).Sum(s => s.Interest);
            var actualMonthly = monthlyTxns.Where(w => w.IsActive == false).Sum(s => s.Interest);

            var total = (actual + expected);
            var totalMonthly = (actualMonthly + expectedMonthly);

            

            // Monthly
            lblExpected.Text = $"1. Actual(M) : {actualMonthly.ToMoneyFormat()} (Per Month: { (actualMonthly / numberOfMonths).ToMoneyFormat()}){Environment.NewLine}" +
               $"2. Expected(M) : {expectedMonthly.ToMoneyFormat()} (Per Month: { (expectedMonthly / numberOfMonths).ToMoneyFormat()})";

            // Daily
            lblActual.Text = $"3. Actual(D) : {actual.ToMoneyFormat()} (Per Month: { (actual / numberOfMonths).ToMoneyFormat()}){Environment.NewLine}" +
                $"4. Expected(D) : {expected.ToMoneyFormat()} (Per Month: { (expected / numberOfMonths).ToMoneyFormat()})";


            lblTotal.Text = $"5. TOTAL(Actual) : {actualMonthly.ToMoneyFormat()}(M) + {actual.ToMoneyFormat()}(D) = {(actualMonthly + actual).ToMoneyFormat()} (Per Month: { ((actualMonthly + actual) / numberOfMonths).ToMoneyFormat()}){Environment.NewLine}" +
                $"6. TOTAL(Expected) : {expectedMonthly.ToMoneyFormat()}(M) + {expected.ToMoneyFormat()}(D) = {(expectedMonthly + expected).ToMoneyFormat()} (Per Month: { ((expectedMonthly + expected) / numberOfMonths).ToMoneyFormat()}){Environment.NewLine}{Environment.NewLine}" +
            $"7. ALL TOTAL: {(actualMonthly + actual).ToMoneyFormat()} + {(expectedMonthly + expected).ToMoneyFormat()} = {(actualMonthly + actual + expectedMonthly + expected).ToMoneyFormat()} (Per Month: { ((actualMonthly + actual + expectedMonthly + expected) / numberOfMonths).ToMoneyFormat()})";
            lblCloseCount.Text = $"Sum of Close Column Count should be {finalData.Sum(w => w.CloseCount)} {Environment.NewLine}  {closedDetailForCurrentMonth}";

            lblSalary.Text = $"Salary : {salary.ToMoneyFormat()}";
            lblSalary.Visible = considerSalary;


            if (comboBox1.Text.ToLower() != "all" && comboBox1.Text != "")
            {
                filteredfinalData = finalData.Where(w => Convert.ToDateTime(w.MonthYear).Year.ToString() == comboBox1.Text).ToList();
            }
            else
            {
                filteredfinalData = finalData;
            }



            // Bindng the real/actual source finalData (will be updated using by ref by various data)
            dgvIncome.DataSource = (from s in filteredfinalData
                                    select new
                                    {
                                        s.MonthYear,
                                        ExpectedIncome = s.ExpectedIncome.ToMoneyFormat(),
                                        ActualIncome = s.ActualIncome.ToMoneyFormat(),
                                        MonthlySalary = s.MonthlySalary.ToMoneyFormat(),
                                        s.CloseCount
                                    }).Reverse().ToList();


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
                                 LoanAmount = newGroup.Sum(s => s.LoanAmount).ToMoneyFormat(),
                                 GivenAmount = newGroup.Sum(s => (s.LoanAmount - s.Interest)).ToMoneyFormat(),
                                 FutureInterest = newGroup.Sum(s => s.Interest).ToMoneyFormat(),
                             }).Reverse().ToList();


            dgvNotePerMonth.DataSource = customers;

            var totalLoanAmount = customers.Sum(s => s.LoanAmount.ToIntMoney());
            var totalGivenAmount = customers.Sum(s => s.GivenAmount.ToIntMoney());
            var difference = totalLoanAmount - totalGivenAmount;


            label6.Text = $"LA: {totalLoanAmount.ToMoneyFormat()} {Environment.NewLine}" +
                $"GA: {totalGivenAmount.ToMoneyFormat()} (~{difference.ToMoneyFormat()}) {Environment.NewLine}" +
                $"FI: {customers.Sum(s => s.FutureInterest.ToIntMoney()).ToMoneyFormat()} {Environment.NewLine}" +
                $"C: {customers.Sum(s => s.GivenCount)} {Environment.NewLine}";

        }

        private void ShowOutstandingMoney()
        {
            outstandingMoney = Transaction.GetAllOutstandingAmount();
            lblOutStanding.Text = outstandingMoney.includesProfit.ToMoneyFormat();
        }

        private void ShowTotalAssetMoney()
        {
            //var inHandAndBank = InHandAndBank.GetAllhandMoney();
            //var inHandMoney = DailyCollectionDetail.GetActualInvestmentTxnDate();

            var latestDailyCxn = DailyCollectionDetail.GetActualInvestmentTxnDate();

            lblTotalAsset.Visible = label4.Visible = false;
            lblTotalAsset.Text = $"test {(outstandingMoney.includesProfit + latestDailyCxn.ExpectedInHand).ToMoney()} (OS: {outstandingMoney.includesProfit.ToMoneyFormat()} IH: {latestDailyCxn.ActualInHand.ToMoney()} MAMA: {latestDailyCxn.MamaAccount.ToMoney()} Actual Outstanding: {outstandingMoney.actual.ToMoneyFormat()})";
            var monthlyCustomersBalance = (from c in Customer.GetAllCustomer().Where(w => w.IsActive && w.IsMonthly())
                                           select Transaction.GetBalance(c)).Sum();

            lblBizAsset.Text = $"{(outstandingMoney.includesProfit + latestDailyCxn.ExpectedInHand).ToMoney()} " +
                $"(OS: {outstandingMoney.includesProfit.ToMoneyFormat()} + IH: {latestDailyCxn.ActualInHand.ToMoney()} + MAMA: {latestDailyCxn.MamaAccount.ToMoney()})  {Environment.NewLine} " +
                $"Actual Outstanding: {outstandingMoney.actual.ToMoneyFormat()} {Environment.NewLine} " +
                $"INVESTMENT: Daily:~{(latestDailyCxn.ActualMoneyInBusiness - monthlyCustomersBalance).ToMoneyFormat()} + Monthly:{monthlyCustomersBalance.ToMoneyFormat()} = {latestDailyCxn.ActualMoneyInBusiness.ToMoneyFormat()}";
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
                filteredfinalData = finalData;
            }
            else
            {
                filteredfinalData = finalData.Where(w => Convert.ToDateTime(w.MonthYear).Year.ToString() == comboBox1.Text).ToList();
            }

            // Bindng the real/actual source finalData (will be updated using by ref by various data)
            dgvIncome.DataSource = (from s in filteredfinalData
                                    select new
                                    {
                                        s.MonthYear,
                                        ExpectedIncome = s.ExpectedIncome.ToMoneyFormat(),
                                        ActualIncome = s.ActualIncome.ToMoneyFormat(),
                                        MonthlySalary = s.MonthlySalary.ToMoneyFormat(),
                                        s.CloseCount
                                    }).Reverse().ToList();

        }

        private void dgvNotePerMonth_CellMouseEnter(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;

            var neededRow = (dynamic)(sender as DataGridView).DataSource;


            var sum = ((IEnumerable<dynamic>)neededRow).Sum(p => Convert.ToInt32(p.FutureInterest.Replace(",", "")));
            //var sum = result.Where(w => w.TransactionId <= neededRow.).Sum(s => s.AmountReceived);

            dgvNotePerMonth.Rows[e.RowIndex].Cells["FutureInterest"].ToolTipText = sum.ToString();

            var month = (((IEnumerable<dynamic>)neededRow).ToArray()[e.RowIndex]).Month;

            var givenCustomers = Transaction.GetGivenTxnForMonth(month);

        }
    }
}
