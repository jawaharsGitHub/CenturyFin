using Common;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ExtensionMethod;

namespace DataAccess.ExtendedTypes
{
    public class BalanceDetail : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.BalanceDetailFile;


        public int GrossAmount { get; set; }

        public int? ActualInHand { get; set; }

        public string Date { get; set; }

        public int? MamaAccount { get; set; }

        public int Salary { get;  set; }

        public int TillNowProfit { get; set; }

        public int FutureProfit { get; set; }

        public int AllProfit { get; set; }

        public int TillNowLoss { get; set; }

        public int FutureLoss { get; set; }

        public int AllLoss { get; set; }

        public int ActualLossPerc { get; set; }
        public int ExpectedLossPerc { get; set; }
        public int AllLossPerc { get; set; }

        public int DailyPerc { get; set; }
        public int WeeklyPerc { get; set; }
        public int TenMonthsPerc { get; set; }
        public int MonthlyPerc { get; set; }
        public int OthersPerc { get; set; }

        [JsonIgnore]
        public string Data { get; set; }

        [JsonIgnore]
        public string ProfitText { get; set; }

        [JsonIgnore]
        public string CloseCount { get; set; }

        [JsonIgnore]
        public string LossText { get; set; }

        [JsonIgnore]
        public string LossPercText { get; private set; }

        public void AddBalanceDetails(string date)
        {
            this.Date = date;

            if (GetBalanceDetail(date) != null)
                DeleteBalanceDetails(date);


            InsertSingleObjectToListJson(JsonFilePath, this);
        }

        public BalanceDetail GetBalanceDetail(string date)
        {
            try
            {
                var list = ReadFileAsObjects<BalanceDetail>(JsonFilePath);
                if (list == null) return null;
                return list.Where(c => c.Date == date).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<BalanceDetail> GetAll()
        {
            try
            {
                return ReadFileAsObjects<BalanceDetail>(JsonFilePath).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string GetLatestBalanceDetail()
        {
            try
            {
                var list = ReadFileAsObjects<BalanceDetail>(JsonFilePath);
                if (list == null) return null;
                var data = list.LastOrDefault();

                if (data != null)
                {
                    return GetEmailHeaderContent(data);
                }
                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        private static string GetEmailHeaderContent(BalanceDetail data)
        {
            return $"மாமாவிடம் கை-இருக்கும் பணம்: <b>{data.ActualInHand.ToMoney()}</b>" + "<br>" +
                    $"மாமா தரவேண்டிய பணம்: <b>{data.MamaAccount.ToMoney()}</b>" + "<br>" +
                    $"வெளியில் நிற்கும் பணம்: <b>{data.GrossAmount.ToMoneyFormat()}</b>" + "<br><br>";
        }

        public void DeleteBalanceDetails(string date)
        {
            try
            {
                var list = ReadFileAsObjects<BalanceDetail>(JsonFilePath);

                list.RemoveAll((c) => c.Date == date);

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public BalanceReport GetGenReportData(bool considerSalary = false)
        {
            List<IncomeReport> finalData;
            finalData = new List<IncomeReport>();
            List<string> moverOverList = new List<string>();
            int varActualClose = 0;

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


            // Years Expected and Actual Salary
            var actual = finalData.Sum(w => w.ActualIncome);
            var expected = finalData.Sum(w => w.ExpectedIncome);

            Salary = finalData.Sum(w => w.MonthlySalary);

            var numberOfMonths = DateTime.Today.Subtract(new DateTime(2018, 1, 25)).Days / (365.25 / 12).ToInt32();

            // Monthly INT incomes.

            var monthlyTxns = Customer.GetAllCustomer().Where(w => w.IsMonthly()).ToList();

            var expectedMonthly = monthlyTxns.Where(w => w.IsActive == true).Sum(s => s.Interest);
            var actualMonthly = monthlyTxns.Where(w => w.IsActive == false).Sum(s => s.Interest);

            var total = (actual + expected);
            var totalMonthly = (actualMonthly + expectedMonthly);
            var actualProfit = actualMonthly + actual;
            var expectedProfit = expectedMonthly + expected;

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

            // Total Report Data

            TillNowProfit = actualProfit;
            FutureProfit = expectedProfit;
            AllProfit = allProfit;

            TillNowLoss = actualLoss;
            FutureLoss = expectedLoss;
            AllLoss = allLoss;


            // PROFIT
            ProfitText = $"Acutaul Profit: {actualMonthly.ToMoneyFormat()}(M) + {actual.ToMoneyFormat()}(D) = {actualProfit.ToMoneyFormat()}A (Per Month: { (actualProfit / numberOfMonths).ToMoneyFormat()}A){Environment.NewLine}" +
                $"Expected Profit: {expectedMonthly.ToMoneyFormat()}(M) + {expected.ToMoneyFormat()}(D) = {expectedProfit.ToMoneyFormat()}E (Per Month: { (expectedProfit / numberOfMonths).ToMoneyFormat()}E){Environment.NewLine}" +
            $"ALL Profit: {actualProfit.ToMoneyFormat()}(A) + {expectedProfit.ToMoneyFormat()}(E) = {(actualProfit + expectedProfit).ToMoneyFormat()}AE (Per Month: { ((actualMonthly + actual + expectedMonthly + expected) / numberOfMonths).ToMoneyFormat()}AE)";

            //lblCloseCount.Text
            CloseCount = $"Sum of Close Column Count should be {finalData.Sum(w => w.CloseCount)} {Environment.NewLine}  {closedDetailForCurrentMonth}";




            LossText = $"Actual Loss: {actualLoss.ToMoneyFormat()} Vs {actualProfit.ToMoneyFormat()} ({actualLossPerc}%){Environment.NewLine}" +
                $"Expected Loss: {expectedLoss.ToMoneyFormat()} Vs {expectedProfit.ToMoneyFormat()}  ({expectedLossPerc}%){Environment.NewLine}" +
               $"All Loss: {allLoss.ToMoneyFormat()} Vs { allProfit.ToMoneyFormat()} ({allLossPerc}%)";

            ActualLossPerc = actualLossPerc;
            ExpectedLossPerc = expectedLossPerc;
            AllLossPerc = allLossPerc;



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

            DailyPerc = allInt.PercentageBtwNo(DCus);
            WeeklyPerc = allInt.PercentageBtwNo(WCus);
            TenMonthsPerc = allInt.PercentageBtwNo(TMCus);
            MonthlyPerc = allInt.PercentageBtwNo(MCus);
            OthersPerc = allInt.PercentageBtwNo(oCus);

            var allIntMoney = allInt.ToMoneyFormat();

            LossPercText = $"Daily: {DCus.ToMoneyFormat()} Vs {allIntMoney} ({DailyPerc}%){Environment.NewLine}" +
                $"Weekly: {WCus.ToMoneyFormat()} Vs {allIntMoney} ({WeeklyPerc}%){Environment.NewLine}" +
                $"Ten Months: {WCus.ToMoneyFormat()} Vs {allIntMoney} ({TenMonthsPerc}%){Environment.NewLine}" +
                $"Monthly: {MCus.ToMoneyFormat()} Vs {allIntMoney} ({MonthlyPerc}%){Environment.NewLine}" +
                $"Others: {oCus.ToMoneyFormat()} Vs {allIntMoney} ({OthersPerc}%){Environment.NewLine}";

            var latestDailyCxn = DailyCollectionDetail.GetActualInvestmentTxnDate();

            ActualInHand = latestDailyCxn.ActualInHand;
            MamaAccount = latestDailyCxn.MamaAccount;

            var outstandingMoney = Transaction.GetAllOutstandingAmount();
            GrossAmount = outstandingMoney.includesProfit;

            Data = GetEmailHeaderContent(this);

            return new BalanceReport() { incomeReports = finalData, BalanceDetail = this };


        }

    }

    public class BalanceReport
    {
        public BalanceDetail BalanceDetail { get; set; }

        public List<IncomeReport> incomeReports { get; set; }
    }
}
