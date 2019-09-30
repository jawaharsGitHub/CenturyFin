using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmDynamicReport : UserControl
    {

        public frmDynamicReport()
        {
            InitializeComponent();

            comboBox1.DataSource = GetOptions();


            RefreshClosed();

        }

        private void RefreshClosed()
        {
            btnClosedTxn.Text = $"Run Closed Txn ({Transaction.GetClosedTxn()})";
        }

        private void ToBeClosedSoon()
        {
            var txn = Transaction.GetTransactionsToBeClosedSoon(100);

            var closedData = txn.Where(w => w.DaysToClose <= DateHelper.RemaingDaysOfMonth);

            lblSeverity.Text = $"Expected Income for this Month - {closedData.Count()} ({closedData.Sum(s => s.Interest).TokFormat()})";

            dgReports.DataSource = txn;
            dgReports.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
        }

        private void NotGivenForFewDays()
        {
            var txn = Transaction.GetTransactionsNotGivenForFewDays();

            //GetReturnTypeGroupedData();
            // NEED INVESTIGATION
            //CustomerStatusReport(txn);

            ProfitReport();

            dgReports.DataSource = txn;
            dgReports.Columns["AmountGivenDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
            dgReports.Columns["LastTxnDate"].DefaultCellStyle.Format = "dd'/'MM'/'yyyy";
        }

        private void ProfitReport()
        {
            var fileName = "ProfitReport.txt";

            var data = (from c in Customer.GetAllCustomer()
                        group c by c.CustomerId into newGroup
                        select new
                        {
                            newGroup.First().Name,
                            Income = newGroup.Sum(s => s.Interest),
                            Balance = newGroup.Sum(s => Transaction.GetBalance(s)),
                            Profit = newGroup.Sum(s => s.Interest) - newGroup.Sum(s => Transaction.GetBalance(s))
                        }).OrderByDescending(o => o.Profit).ToList();

            using (TextWriter tw = new StreamWriter(fileName))
            {
                data.ForEach(f =>
                        {
                            tw.WriteLine($"{f.Name} - Income: {f.Income} Balance: {f.Balance} Profit: {f.Profit}");
                        });
            }


            Process.Start(fileName);


        }

        private void GetReturnTypeGroupedData()
        {
            var data = (from cus in Customer.GetAllActiveCustomer()
                        group cus by cus.ReturnType into newGroup
                        select newGroup).ToList();


            var fileName = "GroupedByReturnType.txt";

            using (TextWriter tw = new StreamWriter(fileName))
            {

                data.ForEach(d =>
                {
                    tw.WriteLine(d.Key);
                    tw.WriteLine($"------------------------------------------------");

                    d.ToList().ForEach(f =>
                    {
                        tw.WriteLine(f.Name);
                    });

                });

            }

            Process.Start(fileName);
        }

        private void CustomerStatusReport(List<DynamicReportNotGivenDays> txn)
        {
            var needInvestigationData = (from w in txn
                                         where w.NotGivenFor >= 15 &&
                                         w.IsNotMonthly() &&
                                         w.Interest != 0 &&
                                         w.NeedInvestigation == true
                                         orderby w.Balance descending
                                         orderby w.CustomerId

                                         select w).ToList();
            var needInvestigationCount = needInvestigationData.Count();
            var needInvestigationAmount = needInvestigationData.Sum(s => s.Balance);

            var needInvestigationAmountWithoutInterest = needInvestigationData.Sum(s => s.Balance) - needInvestigationData.Sum(s => s.Interest);

            // VERY RISK
            var veryRiskData = (from w in txn
                                where w.NotGivenFor >= 15 &&
                                         w.IsNotMonthly() &&
                                         w.Interest != 0 &&
                                         w.NeedInvestigation == false
                                orderby w.Balance descending
                                select w).ToList();
            var veryRiskCount = veryRiskData.Count();
            var veryRiskAmount = veryRiskData.Sum(s => s.Balance);
            var veryRiskAmountWithoutInterest = veryRiskData.Sum(s => s.Balance) - veryRiskData.Sum(s => s.Interest);

            // RISK
            var riskData = (from w in txn
                            where w.NotGivenFor <= 14 &&
                                     w.NotGivenFor > 7 &&
                                     w.IsNotMonthly() &&
                                     w.Interest != 0 &&
                                     w.NeedInvestigation == false

                            orderby w.Balance descending
                            select w).ToList();


            var riskCount = riskData.Count();
            var riskAmount = riskData.Sum(s => s.Balance);
            var riskAmountWithoutInterest = riskData.Sum(s => s.Balance) - riskData.Sum(s => s.Interest);

            // NO INTEREST - FRIEND'S 
            var noInterestData = (from w in txn
                                  where w.Interest == 0
                                  orderby w.Balance descending
                                  select w).ToList();


            var noIntCount = noInterestData.Count();
            var noIntAmount = noInterestData.Sum(s => s.Balance);


            // MONTHLY
            var monthlytData = (from w in txn
                                where w.IsMonthly()
                                orderby w.AmountGivenDate.Value.Day
                                select w).ToList();


            var monthlyCount = monthlytData.Count();
            var monthlyAmount = monthlytData.Sum(s => s.Balance);

            lblSeverity.Text = $"Need Investigation - {needInvestigationCount}({needInvestigationAmount.TokFormat()} Vs {needInvestigationAmountWithoutInterest.TokFormat()}) {Environment.NewLine}" +
                $"Very Risk - {veryRiskCount}({veryRiskAmount.TokFormat()}) {Environment.NewLine}" +
                $"Risk - {riskCount}({riskAmount.TokFormat()})";


            var fileName = "RiskAnalysis.txt";

            using (TextWriter tw = new StreamWriter(fileName))
            {

                tw.WriteLine($"{needInvestigationAmount.TokFormat()} Vs {needInvestigationAmountWithoutInterest.TokFormat()} @ NEED INVESTIGATION!!!!!!!!! ");
                tw.WriteLine($"--------------------------------------- ");
                foreach (var s in needInvestigationData)
                    tw.WriteLine($"{s.CustomerId} {s.Name} - {s.Balance} Vs {s.Balance - s.Interest}({s.NotGivenFor} days)");

                tw.WriteLine($"------------------------------------------------");

                tw.WriteLine($"{veryRiskAmount.TokFormat()} Vs {veryRiskAmountWithoutInterest.TokFormat()}@ VERY RISK!!!!!!!!! ");
                tw.WriteLine($"--------------------------------------- ");
                foreach (var s in veryRiskData)
                    tw.WriteLine($"{s.Name} - {s.Balance} Vs {s.Balance - s.Interest} ({s.NotGivenFor} days)");

                tw.WriteLine($"------------------------------------------------");

                tw.WriteLine($"{riskAmount.TokFormat()} Vs {riskAmountWithoutInterest.TokFormat()}@ RISK!!!");
                tw.WriteLine($"--------------------------------------- ");
                foreach (var s in riskData)
                    tw.WriteLine($"{s.Name} - {s.Balance} Vs {s.Balance - s.Interest}({s.NotGivenFor} days)");

                tw.WriteLine($"------------------------------------------------");

                tw.WriteLine($"{noIntAmount.TokFormat()} @ Friends!!!");
                tw.WriteLine($"--------------------------------------- ");
                foreach (var s in noInterestData)
                    tw.WriteLine($"{s.Name} - {s.Balance} ({s.NotGivenFor} days)");

                tw.WriteLine($"------------------------------------------------");


                tw.WriteLine($"{monthlyAmount.TokFormat()} @ Monthly!!!");
                tw.WriteLine($"--------------------------------------- ");
                foreach (var s in monthlytData)
                    tw.WriteLine($"Loan: {s.LoanAmount} Int: {s.MonthlyInterest} Due: {s.AmountGivenDate.Value.Day} Last Paid On: {s.LastTxnDate.Date.ToString("dd MMMM yyyy")} - {s.Name}");

                tw.WriteLine($"------------------------------------------------");


                var allCustomerData = (from cus in Customer.GetAllCustomer()
                                       group cus by cus.CustomerId into newGroup
                                       select newGroup).ToList();

                foreach (var allCUs in allCustomerData)
                {
                    tw.WriteLine($"{allCUs.Key} ({allCUs.Count()} - CLOSED: {allCUs.Where(w => w.IsActive == false).Sum(s => s.Interest)}) ACTIVE: {allCUs.Where(w => w.IsActive).Sum(s => s.Interest)}");
                    tw.WriteLine($"-------------------------------");

                    foreach (var s in allCUs)
                        tw.WriteLine($"{s.Name}-Amount: {s.LoanAmount} - Interest: {s.Interest} - IsActive: {s.IsActive}");
                }

                tw.WriteLine($"------------------------------------------------");



            }


            Process.Start(fileName);
        }

        private void XCustomer()
        {
            // get all active customers
            var activeCus = Customer.GetAllCustomer().Where(w => w.IsActive).ToList();

            var xCus = Customer.GetAllCustomer().Where(w => activeCus.Select(s => s.CustomerId).Contains(w.CustomerId) == false && w.IsActive == false).ToList();

            dgReports.DataSource = xCus.Select(s => new { s.Name, s.CustomerId }).Distinct().ToList();


        }




        private void btnClosedTxn_Click(object sender, System.EventArgs e)
        {

            var json = File.ReadAllText(AppConfiguration.TransactionFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);

            if (list == null || list.Count == 0) return;

            var closedIds = list.Where(w => w.Balance == 0).ToList();

            foreach (var item in closedIds)
            {
                var closedTxn = new List<Transaction>();
                closedTxn.AddRange(list.Where(w => w.CustomerId == item.CustomerId && w.CustomerSequenceNo == item.CustomerSequenceNo));
                // Back up closed txn
                Transaction.AddClosedTransaction(closedTxn);

                // Delete Transactions data
                Transaction.DeleteTransactionDetails(item.CustomerId, item.CustomerSequenceNo);
            }

            RefreshClosed();

        }

        private void btnRefresh_Click(object sender, System.EventArgs e)
        {
            btnClosedTxn.Text = $"Run Closed Txn ({Transaction.GetClosedTxn()})";
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;

            if (value == 1)
            {
                NotGivenForFewDays();
            }
            else if (value == 2)
            {
                ToBeClosedSoon();
            }
            else if (value == 3)
            {
                XCustomer();
            }
            else if (value == 4)
            {
                CustomerCollectionSpot();
            }
            else if (value == 5)
            {
                AmountGroups();
            }
            else if (value == 6)
            {
                InterestGroups();
            }
            else if (value == 7)
            {
                OverdueReports();
            }

        }

        private void OverdueReports()
        {
            var overdueDays = (from c in Customer.GetAllCustomer()
                               where c.IsActive && (DateTime.Today.Date - c.AmountGivenDate.Value.Date).TotalDays > 100
                               select new
                               {
                                   c.Name,
                                   c.CustomerSeqNumber,
                                   (DateTime.Today.Date - c.AmountGivenDate.Value.Date).TotalDays,
                                   Balance = Transaction.GetBalance(c),
                                   c.LoanAmount

                               }).OrderByDescending(O => O.TotalDays).ToList();

            dgReports.DataSource = overdueDays;
        }

        private void InterestGroups()
        {
            //var cus = Customer.GetAllCustomer().Where(w => w.IsActive == false).ToList();
            var cus = Customer.GetAllCustomer().ToList();

            var groupsByInterest = (from c in cus
                                    group c by c.CustomerId into newGroup
                                    select new InterestGroup
                                    {
                                        CustomerId = newGroup.Key,
                                        Name = newGroup.First().Name,
                                        TotalInterest = newGroup.Sum(s => s.Interest),
                                        TotalBalance = Customer.GetAllActiveCustomer().Where(w => w.CustomerId == newGroup.Key).Sum(s => Transaction.GetBalance(s)),
                                        Count = newGroup.Count()
                                    }).OrderByDescending(o => o.TotalInterest).ToList();

            dgReports.DataSource = groupsByInterest;

        }

        private void AmountGroups()
        {
            var cus = Customer.GetAllCustomer().ToList();

            var groupsByAmount = cus.GroupBy(c => c.LoanAmount)
                                .Select(g => new
                                {
                                    Amount = g.Key,
                                    ActiveCount = g.Where(c => c.IsActive == true).Count(),
                                    ActiveTotal = g.Where(c => c.IsActive == true).Sum(s => s.LoanAmount),
                                    ClosedCount = g.Where(c => c.IsActive == false).Count(),
                                    ClosedTotal = g.Where(c => c.IsActive == false).Sum(s => s.LoanAmount),
                                    AllCount = g.Count(),
                                    AllTotal = g.Sum(s => s.LoanAmount),
                                })
                                .OrderByDescending(o => o.AllTotal).ToList();

            dgReports.DataSource = groupsByAmount;

        }

        private void CustomerCollectionSpot()
        {

            var cus = Customer.GetAllCustomer().Where(w => w.IsActive).ToList();

            // get all active customers
            var activeCus = (from c in cus
                             group c by c.CollectionSpotId into newGroup
                             select new
                             {
                                 Spot = cus.Where(w => w.CustomerId == newGroup.Key).First().Name,
                                 Count = newGroup.Count(),
                                 Amount = newGroup.Sum(s => (s.LoanAmount / 100)),
                                 Customers = string.Join($", {Environment.NewLine}", newGroup.Select(i => $"{i.CustomerId}-{i.Name}"))
                             }).OrderByDescending(o => o.Count);

            dgReports.DataSource = activeCus.Where(w => w.Count > 1).ToList();
            var singleCount = activeCus.Where(w => w.Count == 1);
            var multipleCount = activeCus.Where(w => w.Count > 1);

            lblDetails.Text = $"Single Collection Spot {singleCount.Count()} ({singleCount.Sum(s => s.Amount)}) {Environment.NewLine} " +
                $"Multi Collection Spot {multipleCount.Count()} ({multipleCount.Sum(s => s.Amount)}) {Environment.NewLine}" +
                $"Total Spots {singleCount.Count() + multipleCount.Count()} ({singleCount.Sum(s => s.Amount) + multipleCount.Sum(s => s.Amount)})";


        }


        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "NOT GIVEN FOR FEW DAYS"),
                   new KeyValuePair<int, string>(2, "TO BE CLOSED SOON"),
                   new KeyValuePair<int, string>(3, "X-CUSTOMER"),
                   new KeyValuePair<int, string>(4, "CUSTOMER-COLLECTION SPOT"),
                   new KeyValuePair<int, string>(5, "AMOUNT-GROUPS"),
                   new KeyValuePair<int, string>(6, "INTEREST-GROUPS"),
                   new KeyValuePair<int, string>(7, "OVERDUE-CUSTOMER")
               };

            return myKeyValuePair;

        }

        private void dgReports_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
        {
            var value = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;

            if (value == 1)
            {
                if (this.dgReports.Columns["NotGivenFor"] == null) return;
                if (e.RowIndex >= 0 && e.ColumnIndex == this.dgReports.Columns["NotGivenFor"].Index)
                {
                    if (e.Value != null)
                    {
                        int dayNotGiven = Convert.ToInt32(e.Value);
                        if (dayNotGiven >= 15)
                        {
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Orange;
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (dayNotGiven >= 7)
                        {
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Black;
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                        }
                    }
                }

            }

            else if (value == 2)
            {
                if (this.dgReports.Columns["NeedToClose"] == null) return;
                if (e.RowIndex >= 0 && e.ColumnIndex == this.dgReports.Columns["NeedToClose"].Index)
                {
                    if (e.Value != null)
                    {
                        int needToClose = Convert.ToInt32(e.Value);
                        if (needToClose == 0)
                        {
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Black;
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.ForeColor = Color.White;
                        }
                        else if (needToClose <= 7)
                        {
                            dgReports.Rows[e.RowIndex].DefaultCellStyle.BackColor = Color.Orange;
                        }
                    }
                }
            }
        }
    }
}
