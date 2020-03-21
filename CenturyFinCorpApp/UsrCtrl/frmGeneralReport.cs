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
        List<string> moverOverList = new List<string>();

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
                                       Count = newGroup.Count(),
                                       Names = newGroup.Select(s => s.NameAndSeqId)
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
                                        Count = newGroup.Count(),
                                        Names = newGroup.Select(s => s.NameAndSeqId)
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
                {
                    moveOverClosed += closedData;
                    moveOverInterest += f.Sum(s => s.TotalInterest);
                    var lst = f.SelectMany(s => s.Names);
                    moverOverList.AddRange(lst);

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
                MonthYear = "January 2018",
                ActualIncome = 0,
                CloseCount = 0
            });

            finalData.Insert(1, new IncomeReport()
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

            //var allTxns = Transaction.GetAlIncludeClosedTxns();

            //var allCus = Customer.GetAllCustomer();

            //var dddd = DateTime.Today.GetAllMonths();


            //dddd.ForEach(d =>
            //{
            //    int bal = 0;
            //    allCus.ForEach(ac =>
            //    {

            //        var ttt = allTxns.Where(w => 
            //        ac.CustomerSeqNumber == w.CustomerSequenceNo && 
            //        (ac.AmountGivenDate <= d.ld && ac.ClosedDate >= d.ld) &&
            //        (w.TxnDate <= d.ld)).OrderByDescending(o => o.TxnDate);

            //        if (ttt.Count() > 0)
            //        {
            //            bal += ttt.First().Balance;
            //        }
            //    });

            //    var ff = finalData.Where(w => w.MonthYear == d.fd.ToString("Y")).FirstOrDefault();

            //    if (ff != null)
            //        ff.InvAmount = bal.ToMoneyFormat();

            //    //finalData.Where(w => w.MonthYear == )

            //});



            // Years Expected and Actual Salary
            var actual = finalData.Sum(w => w.ActualIncome);
            var expected = finalData.Sum(w => w.ExpectedIncome);

            var salary = finalData.Sum(w => w.MonthlySalary);
            lblSalary.Text = $"Salary : {salary.ToMoneyFormat()}";
            lblSalary.Visible = considerSalary;

            var numberOfMonths = DateTime.Today.Subtract(new DateTime(2018, 1, 25)).Days / (365.25 / 12).ToInt32();

            // Monthly INT incomes.

            var monthlyTxns = Customer.GetAllCustomer().Where(w => w.IsMonthly()).ToList();

            var expectedMonthly = monthlyTxns.Where(w => w.IsActive == true).Sum(s => s.Interest);
            var actualMonthly = monthlyTxns.Where(w => w.IsActive == false).Sum(s => s.Interest);

            var total = (actual + expected);
            var totalMonthly = (actualMonthly + expectedMonthly);
            var actualProfit = actualMonthly + actual;
            var expectedProfit = expectedMonthly + expected;

            // PROFIT
            lblTotal.Text = $"Acutaul Profit: {actualMonthly.ToMoneyFormat()}(M) + {actual.ToMoneyFormat()}(D) = {actualProfit.ToMoneyFormat()}A (Per Month: { (actualProfit / numberOfMonths).ToMoneyFormat()}A){Environment.NewLine}" +
                $"Expected Profit: {expectedMonthly.ToMoneyFormat()}(M) + {expected.ToMoneyFormat()}(D) = {expectedProfit.ToMoneyFormat()}E (Per Month: { (expectedProfit / numberOfMonths).ToMoneyFormat()}E){Environment.NewLine}" +
            $"ALL Profit: {actualProfit.ToMoneyFormat()}(A) + {expectedProfit.ToMoneyFormat()}(E) = {(actualProfit + expectedProfit).ToMoneyFormat()}AE (Per Month: { ((actualMonthly + actual + expectedMonthly + expected) / numberOfMonths).ToMoneyFormat()}AE)";
            lblCloseCount.Text = $"Sum of Close Column Count should be {finalData.Sum(w => w.CloseCount)} {Environment.NewLine}  {closedDetailForCurrentMonth}";


            // LOSS
            var actualLoss = Report.GetActualLoss();
            var expectedLoss = Report.GetExpectedLoss();
            var allLoss = actualLoss + expectedLoss;

            var allProfit = actualProfit + expectedProfit;

            var totalIncomeWithActualLoss = (actualProfit + actualLoss);
            var totalIncomeWithAllLoss = (actualProfit + actualLoss + expectedLoss);

            var actualLossPerc = actualProfit.PercentageBtwNo(actualLoss);
            var expectedLossPerc = expectedProfit.PercentageBtwNo(expectedLoss);
            var allLossPerc = allProfit.PercentageBtwNo(allLoss);

            lblLoss.Text = $"Actual Loss: {actualLoss.ToMoneyFormat()} Vs {actualProfit.ToMoneyFormat()} ({actualLossPerc}%){Environment.NewLine}" +
                $"Expected Loss: {expectedLoss.ToMoneyFormat()} Vs {expectedProfit.ToMoneyFormat()}  ({expectedLossPerc}%){Environment.NewLine}" +
                $"All Loss: {allLoss.ToMoneyFormat()} Vs { allProfit.ToMoneyFormat()} ({allLossPerc}%)";


            // Profit Percentages.

            var DCus = Customer.GetAllCustomer().Where(w => w.ReturnType == ReturnTypeEnum.Daily).Select(s => s.Interest).Sum();
            var WCus = Customer.GetAllCustomer().Where(w => w.ReturnType == ReturnTypeEnum.Weekly).Select(s => s.Interest).Sum();
            var TMCus = Customer.GetAllCustomer().Where(w => w.ReturnType == ReturnTypeEnum.TenMonths).Select(s => s.Interest).Sum();
            var MCus = Customer.GetAllCustomer().Where(w => w.ReturnType == ReturnTypeEnum.Monthly).Select(s => s.Interest).Sum();
            var oCus = Customer.GetAllCustomer().Where(
                w => w.ReturnType != ReturnTypeEnum.Monthly &&
                w.ReturnType != ReturnTypeEnum.TenMonths &&
                w.ReturnType != ReturnTypeEnum.Weekly &&
                w.ReturnType != ReturnTypeEnum.Daily
                ).Select(s => s.Interest).Sum();

            var allInt = Customer.GetAllCustomer().Where(w => w.Interest > 0).Select(s => s.Interest).Sum();

            lblIntPerc.Text = $"Daily: {DCus.ToMoneyFormat()} Vs {allInt.ToMoneyFormat()} ({allInt.PercentageBtwNo(DCus)}%){Environment.NewLine}" +
                $"Weekly: {WCus.ToMoneyFormat()} Vs {allInt.ToMoneyFormat()} ({allInt.PercentageBtwNo(WCus)}%){Environment.NewLine}" +
                $"Ten Months: {WCus.ToMoneyFormat()} Vs {allInt.ToMoneyFormat()} ({allInt.PercentageBtwNo(TMCus)}%){Environment.NewLine}" +
                $"Monthly: {MCus.ToMoneyFormat()} Vs {allInt.ToMoneyFormat()} ({allInt.PercentageBtwNo(MCus)}%){Environment.NewLine}" +
                $"Others: {oCus.ToMoneyFormat()} Vs {allInt.ToMoneyFormat()} ({allInt.PercentageBtwNo(oCus)}%){Environment.NewLine}";





            if (comboBox1.Text.ToLower() != "all" && comboBox1.Text != "")
                filteredfinalData = finalData.Where(w => Convert.ToDateTime(w.MonthYear).Year.ToString() == comboBox1.Text).ToList();
            else
                filteredfinalData = finalData;



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

                             select new NotesPerMonth()
                             {
                                 Month = newGroup.Key,
                                 GivenCount = newGroup.Count(),
                                 LoanAmount = newGroup.Sum(s => s.LoanAmount).ToMoneyFormat(),
                                 GivenAmount = newGroup.Sum(s => (s.LoanAmount - s.Interest)).ToMoneyFormat(),
                                 FutureInterest = newGroup.Sum(s => s.Interest).ToMoneyFormat(),
                             }).Reverse().ToList();

            var closedcustomers = (from c in Customer.GetClosedCustomer()
                                       //orderby c.AmountGivenDate
                                   group c by c.ClosedDate.Value.ToString("Y") into newGroup
                                   select new NotesPerMonth()
                                   {
                                       Month = newGroup.Key,
                                       ClosedCount = $"{newGroup.Count()} ({newGroup.Sum(s => s.Interest)})",
                                   }).ToList();

            var resultData = (from c in customers
                              join cc in closedcustomers
                              on c.Month equals cc.Month
                              into newOuterJoinList
                              from item in newOuterJoinList.DefaultIfEmpty(null)
                              select new NotesPerMonth
                              {
                                  Month = c.Month,
                                  GivenCount = c.GivenCount,
                                  ClosedCount = (item == null) ? "0" : item.ClosedCount,
                                  LoanAmount = c.LoanAmount,
                                  GivenAmount = c.GivenAmount,
                                  FutureInterest = c.FutureInterest,
                              }).ToList();




            dgvNotePerMonth.DataSource = resultData;

            var totalLoanAmount = resultData.Sum(s => s.LoanAmount.ToIntMoney());
            var totalGivenAmount = resultData.Sum(s => s.GivenAmount.ToIntMoney());
            var difference = totalLoanAmount - totalGivenAmount;


            label6.Text = $"LA: {totalLoanAmount.ToMoneyFormat()} {Environment.NewLine}" +
                $"GA: {totalGivenAmount.ToMoneyFormat()} (~{difference.ToMoneyFormat()}) {Environment.NewLine}" +
                $"FI: {resultData.Sum(s => s.FutureInterest.ToIntMoney()).ToMoneyFormat()} {Environment.NewLine}" +
                $"C: {resultData.Sum(s => s.GivenCount)} {Environment.NewLine}";

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

        private void lblCloseCount_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            MessageBox.Show(String.Join(Environment.NewLine, moverOverList));

        }
    }
}
