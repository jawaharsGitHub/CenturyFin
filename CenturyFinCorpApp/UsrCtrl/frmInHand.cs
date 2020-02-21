using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmInHand : UserControl
    {

        private DailyCollectionDetail dailyTxn;
        private static DateTime currentBalanceDate;

        bool haveData = false;

        public frmInHand()
        {
            InitializeComponent();

            GetDailyTxn(GlobalValue.CollectionDate.Value.AddDays(-1), true);
            GetDailyTxn(GlobalValue.CollectionDate.Value, true);


        }

        private void EnableEdit()
        {
            groupBox1.Enabled = txtComments.Enabled = btnAdd.Enabled = btnShow.Enabled = btnDelete.Enabled = true;

            if (btnEnable.Text == "Enable")
                btnEnable.Text = "Disable";
            else
            {
                GetDailyTxn(dateTimePicker1.Value, false);
                btnEnable.Text = "Enable";
            }
        }

        private void GetDailyTxn(DateTime date, bool isOnLoad)
        {
            dailyTxn = DailyCollectionDetail.GetDailyTxn(date, isOnLoad);

            var haveNoDailytnx = (dailyTxn == null);
            haveData = txtComments.Enabled = btnAdd.Enabled = btnShow.Enabled = btnDelete.Enabled = !haveNoDailytnx;

            btnEnable.Enabled = !haveData;

            groupBox1.Enabled = haveData;

            if (haveData == false)
            {
                txtOtherExpenditure.Text = "0";
                txtOtherInvestment.Text = "0";
                txtOutMoney.Text = "0";
                txtMamaExpenditure.Text = "0";

                txtInputMoney.Text = "0";
                txtOutusedMoney.Text = "0";
                txtInvsOutDiff.Text = "0";

                if (txtComments.Text.Length > txtComments.Text.LastIndexOf("$") + 1)
                    txtComments.Text = txtComments.Text.Remove(txtComments.Text.LastIndexOf("$") + 1);

            }

            if (haveNoDailytnx)
            {
                dailyTxn = DailyCollectionDetail.GetDailyTxn(date.AddDays(-1), isOnLoad);
                dateTimePicker1.Value = date;

                var collectionAmount = Transaction.GetDailyCollectionAmount(dateTimePicker1.Value);
                if (collectionAmount > 0)
                {
                    txtCollectionAmount.Text = collectionAmount.ToString();
                    var todayTxn = (from t in Transaction.GetDailyCollectionDetails_V0(dateTimePicker1.Value)
                                    join c in Customer.GetAllCustomer()
                                    on t.CustomerSequenceNo equals c.CustomerSeqNumber
                                    select new
                                    {
                                        c.Interest,
                                        c.LoanAmount,
                                        t.AmountReceived,
                                        t.Balance

                                    }).ToList();

                    // Topup details.
                    var topupCustomers = TopupCustomer.GetAllTopupCustomer().Where(w => w.ReturnType != ReturnTypeEnum.BiWeekly && w.AmountGivenDate.Value.Date == date).ToList();

                    txtGivenAmount.Text = (todayTxn.Where(w => w.AmountReceived == 0).Sum(s => s.Balance) + topupCustomers.Sum(s => s.LoanAmount)).ToString();
                    txtInterest.Text = (todayTxn.Where(w => w.AmountReceived == 0 && w.Balance > 0).Sum(s => s.Interest) + topupCustomers.Sum(s => s.Interest)).ToString();
                    txtClosed.Text = todayTxn.Where(w => w.Balance == 0).Count().ToString();
                    txtOpened.Text = todayTxn.Where(w => w.AmountReceived == 0).Count().ToString();

                    txtOtherExpenditure.Text = txtOtherInvestment.Text = txtOutMoney.Text = "0";

                }
                lblDate1.Text = lblDate2.Text = groupBox1.Text = $"{dateTimePicker1.Value.ToShortDateString()} NO DATA";
                btnAdd.Text = "ADD";
                return;
            }

            lblDate1.Text = lblDate2.Text = groupBox1.Text = $"Data For {dailyTxn.Date}";
            btnAdd.Text = "UPDATE";


            txtSentFromUSA.Text = dailyTxn.SentFromUSA.ToString();

            txtBankTxnOut.Text = dailyTxn.BankTxnOut.ToString();
            txtTakenFromBank.Text = dailyTxn.TakenFromBank.ToString();
            txtCollectionAmount.Text = dailyTxn.CollectionAmount.ToString();
            txtGivenAmount.Text = dailyTxn.GivenAmount.ToString();
            txtInterest.Text = dailyTxn.Interest.ToString();
            txtClosed.Text = dailyTxn.ClosedAccounts.ToString();
            txtOpened.Text = dailyTxn.OpenedAccounts.ToString();
            txtTmrNeeded.Text = dailyTxn.TomorrowNeed.ToString();

            txtOtherExpenditure.Text = Convert.ToString(dailyTxn.OtherExpenditire);
            txtOtherInvestment.Text = Convert.ToString(dailyTxn.OtherInvestment);

            btnYesterdayInHand.Text = dailyTxn.YesterdayAmountInHand.TokFormat();
            btnInCompany.Text = $"C: {(dailyTxn.MamaAccount + dailyTxn.ActualInHand).TokFormat()}";
            btnInHand.Text = "IH:" + dailyTxn.ActualInHand.TokFormat();
            btnMama.Text = "M:" + dailyTxn.MamaAccount.TokFormat();
            btnInvestment.Text = "INV:" + DailyCollectionDetail.GetActualInvestmentTxnDate(dateTimePicker1.Value).ActualMoneyInBusiness.ToMoneyFormat();

            lblCanGive.Text = $"Actually we can give - {(dailyTxn.TodayInHand / 4500) * 5000} {Environment.NewLine}with extra {(dailyTxn.TodayInHand % 4500)}";
            btnInBank.Text = dailyTxn.InBank.TokFormat();
            btnTmrWanted.Text = (dailyTxn.TomorrowDiff > 0) ? dailyTxn.TomorrowDiff.ToString() : $"0 --- (Have Extra {Math.Abs(Convert.ToInt32(dailyTxn.TomorrowDiff))} )";
            btnTmrWanted.BackColor = (dailyTxn.TomorrowDiff > 0) ? Color.Red : Color.Green;
            btnCanGive.Text = (dailyTxn.TodayInHand + dailyTxn.InBank).TokFormat();
            txtOutMoney.Text = dailyTxn.OutUsedMoney.ToString();


            // Collection SUmmary
            UpdateVerifyDetailsOnLoad();

            txtComments.Text = dailyTxn.Comments;

        }

        private void UpdateVerifyDetails()
        {
            txtInputMoney.Text = dailyTxn.InputMoney.ToString();
            txtOutusedMoney.Text = dailyTxn.OutGoingMoney.ToString();
            txtInvsOutDiff.Text = dailyTxn.Difference.ToString();
            txtExpectedInHand.Text = dailyTxn.ExpectedInHand.ToString();

            // will change later in 2nd phase.
            //txtActualInhand.Text = dailyTxn.ActualInHand.ToString();
            //txtMamaExpenditure.Text = dailyTxn.MamaExpenditure.ToString();
            //txtMamaInputMoney.Text = dailyTxn.MamaInputMoney.ToString();
            //txtMamaAccount.Text = dailyTxn.MamaAccount.ToString();
        }

        private void UpdateVerifyDetailsOnLoad()
        {
            txtInputMoney.Text = dailyTxn.InputMoney.ToString();
            txtOutusedMoney.Text = dailyTxn.OutGoingMoney.ToString();
            txtInvsOutDiff.Text = dailyTxn.Difference.ToString();
            txtExpectedInHand.Text = dailyTxn.ExpectedInHand.ToString();

            // will change later in 2nd phase.
            txtActualInhand.Text = dailyTxn.ActualInHand.ToString();
            txtMamaExpenditure.Text = dailyTxn.MamaExpenditure.ToString();
            txtMamaInputMoney.Text = dailyTxn.MamaInputMoney.ToString();
            txtMamaAccount.Text = dailyTxn.MamaAccount.ToString();
        }

        private void btnAddOrUpdate_Click(object sender, EventArgs e)
        {
            if (dailyTxn == null) dailyTxn = new DailyCollectionDetail();

            dailyTxn.Date = dateTimePicker1.Value.ToShortDateString();
            dailyTxn.YesterdayAmountInHand = dailyTxn.TodayInHand;
            dailyTxn.SentFromUSA = txtSentFromUSA.Text.ToDecimal();
            dailyTxn.BankTxnOut = txtBankTxnOut.Text.ToDecimal();
            dailyTxn.TakenFromBank = txtTakenFromBank.Text.ToInt32();
            dailyTxn.InBank = (dailyTxn.InBank + dailyTxn.SentFromUSA - dailyTxn.TakenFromBank - dailyTxn.BankTxnOut);

            dailyTxn.CollectionAmount = txtCollectionAmount.Text.ToInt32();
            dailyTxn.GivenAmount = txtGivenAmount.Text.ToInt32();
            dailyTxn.Interest = txtInterest.Text.ToInt32();
            dailyTxn.ClosedAccounts = txtClosed.Text.ToInt32();
            dailyTxn.OpenedAccounts = txtOpened.Text.ToInt32();
            dailyTxn.TomorrowNeed = txtTmrNeeded.Text.ToInt32();

            dailyTxn.OtherExpenditire = txtOtherExpenditure.Text.ToInt32();
            dailyTxn.OtherInvestment = txtOtherInvestment.Text.ToInt32();
            dailyTxn.OutUsedMoney = txtOutMoney.Text.ToInt32();

            dailyTxn.TodayInHand = (dailyTxn.YesterdayAmountInHand + dailyTxn.CollectionAmount + dailyTxn.TakenFromBank - dailyTxn.GivenAmount + dailyTxn.Interest + dailyTxn.OtherInvestment - dailyTxn.OtherExpenditire);

            dailyTxn.TomorrowDiff = (Convert.ToInt32(txtTmrNeeded.Text) - Convert.ToInt32((dailyTxn.TodayInHand + dailyTxn.InBank)));
            dailyTxn.Comments = txtComments.Text;
            dailyTxn.ActualMoneyInBusiness = (dailyTxn.ActualMoneyInBusiness + dailyTxn.OtherInvestment) - dailyTxn.OutUsedMoney;


            var totalInput = dailyTxn.ActualInHand + txtCollectionAmount.Text.ToInt32() + txtInterest.Text.ToInt32() + txtOtherInvestment.Text.ToInt32();
            var totalOut = txtGivenAmount.Text.ToInt32() + txtOtherExpenditure.Text.ToInt32();



            dailyTxn.InputMoney = totalInput;
            dailyTxn.OutGoingMoney = totalOut;
            dailyTxn.Difference = totalInput - totalOut;
            dailyTxn.ExpectedInHand = (dailyTxn.YesterdayAmountInHand.ToInt32() + totalInput) - totalOut;


            UpdateVerifyDetails();


            DailyCollectionDetail.AddOrUpdateDaily(dailyTxn);

            //currentBalanceDate = DailyCollectionDetail.GetLastCollectionDateDate();

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            GetDailyTxn(dateTimePicker1.Value, false);

        }

        private void txtGivenAmount_Leave(object sender, EventArgs e)
        {
            //var amt = Convert.ToDecimal(txtGivenAmount.Text);

            //txtInterest.Text = (amt / 10).ToString();

            //label19.Text = DecimalToWords(amt);
            //label20.Text = DecimalToWords(amt / 10);
        }

        public string DecimalToWords(decimal number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + DecimalToWords(Math.Abs(number));

            string words = "";

            int intPortion = (int)number;
            decimal fraction = (number - intPortion) * 100;
            int decPortion = (int)fraction;

            words = NumberToWords(intPortion);
            if (decPortion > 0)
            {
                words += " and ";
                words += NumberToWords(decPortion);
            }
            return words;
        }

        public static string NumberToWords(int number)
        {
            if (number == 0)
                return "zero";

            if (number < 0)
                return "minus " + NumberToWords(Math.Abs(number));

            string words = "";

            if ((number / 1000000) > 0)
            {
                words += NumberToWords(number / 1000000) + " million ";
                number %= 1000000;
            }

            if ((number / 1000) > 0)
            {
                words += NumberToWords(number / 1000) + " thousand ";
                number %= 1000;
            }

            if ((number / 100) > 0)
            {
                words += NumberToWords(number / 100) + " hundred ";
                number %= 100;
            }

            if (number > 0)
            {
                if (words != "")
                    words += "and ";

                var unitsMap = new[] { "zero", "one", "two", "three", "four", "five", "six", "seven", "eight", "nine", "ten", "eleven", "twelve", "thirteen", "fourteen", "fifteen", "sixteen", "seventeen", "eighteen", "nineteen" };
                var tensMap = new[] { "zero", "ten", "twenty", "thirty", "forty", "fifty", "sixty", "seventy", "eighty", "ninety" };

                if (number < 20)
                    words += unitsMap[number];
                else
                {
                    words += tensMap[number / 10];
                    if ((number % 10) > 0)
                        words += "-" + unitsMap[number % 10];
                }
            }

            return words;
        }


        private void btnNextDay_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
            GetDailyTxn(dateTimePicker1.Value, false);

        }

        private void btnPrevDay_Click(object sender, EventArgs e)
        {
            dateTimePicker1.Value = dateTimePicker1.Value.AddDays(-1);
            GetDailyTxn(dateTimePicker1.Value, false);

        }

        // verify button click
        private void button1_Click(object sender, EventArgs e)
        {
            var inputMoney = txtInputMoney.Text.ToInt32();
            var outUsedMoney = txtOutusedMoney.Text.ToInt32();
            var inVsOutDiff = txtInvsOutDiff.Text.ToInt32();
            var expectedInHand = txtActualInhand.Text.ToInt32() + txtMamaAccount.Text.ToInt32();  // txtExpectedInHand.Text.ToInt32();
            var actualInhand = inputMoney - outUsedMoney; //txtActualInhand.Text.ToInt32();
            var mamaAccount = (txtMamaAccount.Text.ToInt32() - txtMamaInputMoney.Text.ToInt32()) + txtMamaExpenditure.Text.ToInt32();


            dailyTxn.InputMoney = inputMoney;
            dailyTxn.OutUsedMoney = outUsedMoney;
            dailyTxn.Difference = inVsOutDiff;
            dailyTxn.ExpectedInHand = actualInhand + mamaAccount;
            dailyTxn.ActualInHand = actualInhand - txtMamaExpenditure.Text.ToInt32();
            dailyTxn.MamaAccount = mamaAccount;
            dailyTxn.MamaExpenditure = txtMamaExpenditure.Text.ToInt32();
            dailyTxn.MamaInputMoney = txtMamaInputMoney.Text.ToInt32();

            DailyCollectionDetail.UpdateVerifyDetails(dailyTxn);
            UpdateVerifyDetails();

            //var fileName = GetCxnFileName();

            //using (TextWriter tw = new StreamWriter(fileName))
            //{
            //    tw.WriteLine($"@Collection Summary for {dateTimePicker1.Value.ToShortDateString()}");
            //    tw.WriteLine($"------------------------------------------------");

            //    tw.WriteLine($"Total input Money = ActualInhand inhand yesterday[{dailyTxn.YesterdayAmountInHand.TokFormat()}] + collection [{txtCollectionAmount.Text}] + interest[{txtInterest.Text}] + other investment [{txtOtherInvestment.Text}] = [{inputMoney.TokFormat()}]");
            //    tw.WriteLine(Environment.NewLine);
            //    tw.WriteLine($"Total out used money = given amount [{txtGivenAmount.Text}] + other expenditure [{txtOtherExpenditure.Text}] = [{outUsedMoney.TokFormat()}]");
            //    tw.WriteLine(Environment.NewLine);
            //    tw.WriteLine($"Expected Inhand: {dailyTxn.ExpectedInHand.TokFormat()} Actual Inhand : {dailyTxn.ActualInHand.TokFormat()} MamaAccount: {dailyTxn.MamaAccount.TokFormat()} ");
            //}

            string s = txtComments.Text;
            int start = s.IndexOf("$");
            int end = s.LastIndexOf("$");
            string toBeReplaced = s.Substring(start + 1, end - start - 1);

            txtComments.Text = s.Replace(toBeReplaced, $"{Environment.NewLine}{dailyTxn.ExpectedInHand.TokFormat()} [In Company]{Environment.NewLine}{dailyTxn.ActualInHand.TokFormat()} [In Hand]{Environment.NewLine}{mamaAccount.TokFormat()} [In Mama]{Environment.NewLine}");
            btnAddOrUpdate_Click(null, null);

            GetDailyTxn(dateTimePicker1.Value, false);

            if (DialogResult.Yes == MessageBox.Show("You want email?", "Email?", MessageBoxButtons.YesNoCancel))
            {
                SendBalances();
            }
            //ReportRun();
            //Process.Start(fileName);
        }

        private void SendBalances()
        {
            try
            {

                //if (General.CheckForInternetConnection() == false)
                //{
                //    MessageBox.Show("No Internet Available, Please connect and try again!");
                //    return;
                //}

                BackgroundWorker bw = new BackgroundWorker();
                //this.Controls.Add(bw);
                bw.DoWork += (s, e) =>
                {

                    currentBalanceDate = DailyCollectionDetail.GetLastCollectionDate();
                    //ReportRun();
                    SendEmailForCrossCheck();
                    //var activeCus = Customer.GetAllActiveCustomer();
                    SendEmailForSendBalance();

                    //AppCommunication.SendBalanceEmail(allBalances, currentBalanceDate, activeCus.Count(), "Jeyam Finance Balance Report");
                    MessageBox.Show("Balance Report have been send to your email");
                };
                bw.RunWorkerAsync();



            }
            catch (Exception ex)
            {

                throw ex;
            }

        }

        private void Bw_DoWork(object sender, DoWorkEventArgs e)
        {
            throw new NotImplementedException();
        }

        private void CommitChanges()
        {
            try
            {

                if (General.CheckForInternetConnection() == false)
                {
                    MessageBox.Show("No Internet Available, Please connect and try again!");
                    return;
                }

                //GitHubClient.Commit();

                //MessageBox.Show("Balance Report have been send to your email");

            }
            catch (Exception)
            {

                throw;
            }

        }

        private string GetCxnFileName()
        {
            return General.GetDataFolder("CenturyFinCorpApp\\bin\\Debug", "DataAccess\\Data\\CollectionSummary\\") + $"CxnSum_{dateTimePicker1.Value.ToShortDateString()}.txt";
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DailyCollectionDetail.DeleteDailyCxnDetails(dateTimePicker1.Value);
            // Delete cxn data file.
            var fileName = GetCxnFileName();

            if (File.Exists(fileName))
            {
                File.Delete(fileName);
            }
            MessageBox.Show($"Deleted collection details for {dateTimePicker1.Value.ToShortDateString()}");
            GetDailyTxn(dateTimePicker1.Value, false);

        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            EnableEdit();

        }

        private void btnSendBalances_Click(object sender, EventArgs e)
        {
            SendBalances();
        }

        private void btnCheckReport_Click(object sender, EventArgs e)
        {
            //ReportRun();
        }

        private void ReportRun(bool isPersonal = false)
        {
            StringBuilder resultHtml = new StringBuilder();

            string htmlString = FileContentReader.ReportRunHtml;

            var cus = Customer.GetAllActiveCustomer();
            var txns = Transaction.GetDailyCollectionDetails_V0(currentBalanceDate);

            var data = (from c in cus
                        join t in txns
                        on c.CustomerSeqNumber equals t.CustomerSequenceNo into newData
                        from dept in newData.DefaultIfEmpty()
                            //where c.ReturnType == returnType
                            //&& c.IsPersonal == isPersonal
                        select new CollectionStatus
                        {
                            Name = string.IsNullOrEmpty(c.TamilName) ? c.Name : c.TamilName,
                            LoanAmount = c.LoanAmount,
                            Balance = Transaction.GetBalance(c),
                            TxnDate = (dept?.TxnDate),
                            TxnId = (dept?.TransactionId),
                            CxnAmount = c.InitialInterest,
                            ReturnType = c.ReturnType,
                            IsPersonal = c.IsPersonal,
                            //LastDate = (c.ReturnType == ReturnTypeEnum.Weekly) ? Transaction.GetTransactionSummaryForWeek(c.CustomerSeqNumber) : Transaction.GetLastTransactionDate(c),
                            LastDate = Transaction.GetLastTransactionDate(c),

                        })
                        .OrderByDescending(t => t.LastDate)
                        .ToList();

            AppCommunication.SendReportEmail(FormHTML(SourceHtmlString: htmlString, returnType: ReturnTypeEnum.Daily, data, isOnlyNotGiven: true, isPersonal: isPersonal));

            var values = Enum.GetValues(typeof(ReturnTypeEnum)).Cast<ReturnTypeEnum>().Reverse().ToList();

            values.ForEach(f =>
            {
                AppCommunication.SendReportEmail(FormHTML(SourceHtmlString: htmlString, returnType: f, data, isPersonal: isPersonal));
            });

        }

        private void SendEmailForCrossCheck()
        {
            var customersData = Customer.GetAllActiveCustomer();

            var values = Enum.GetValues(typeof(ReturnTypeEnum)).Cast<ReturnTypeEnum>().Reverse().ToList();

            FormHTMLForCrossCheck(customersData);
            //AppCommunication.SendBalanceEmail(htmlString, currentBalanceDate, customersData.Count);


            //values.ForEach(f =>
            //{
            // var htmlString = FormHTMLForCrossCheck(customersData);
            // AppCommunication.SendBalanceEmail(htmlString, currentBalanceDate, customersData.Count);
            //});

        }

        private EmailStructure FormHTML(string SourceHtmlString, ReturnTypeEnum returnType, List<CollectionStatus> data, bool isOnlyNotGiven = false, bool isPersonal = false)
        {

            List<CollectionStatus> localData = null;

            if (data.Count == 0) return null;

            StringBuilder rowData = new StringBuilder();

            if (isOnlyNotGiven)
            {
                localData = data.Where(w => w.IsToday == "N").ToList();
            }
            else
            {
                localData = data.Where(w => w.ReturnType == returnType && w.IsPersonal == isPersonal).ToList();
            }

            if (localData.Count() == 0) return null;

            localData.ForEach(f =>
            {
                rowData.Append($@"<tr><td>{f.Name}</td><td>{f.IsToday}</td><td>{f.LoanAmount}</td><td>{f.Balance}</td><td>{f.LastDate}</td></tr>");
            });


            var fn = $"{localData.Count()} {returnType.ToString()} For {currentBalanceDate.ToShortDateString()} {localData.Sum(D => D.Balance)} - {localData.Sum(D => D.Balance) / 100}";

            var dailyCheckHTML = SourceHtmlString.Replace("[data]", rowData.ToString()).Replace("[title]", fn);

            //return dailyCheckHTML;

            return new EmailStructure()
            {
                CollectionDate = currentBalanceDate,
                HtmlContent = dailyCheckHTML,
                Subject = isOnlyNotGiven ? "NOT GIVEN" : returnType.ToString()
            };

            //General.CreateHTML($"{fn}.htm", dailyCheckHTML);
        }

        private void FormHTMLForCrossCheck(List<Customer> customersData)
        {
            var data = customersData.OrderBy(o => o.Name).Select(ac =>
                        new
                        {
                            ac.CustomerSeqNumber,
                            ac.Name,
                            Ba = Transaction.GetBalanceAndLastDate(ac),
                            ac.LoanAmount,
                            ac.ReturnType
                        }).ToList();

            if (data.Count == 0) return;

            currentBalanceDate = DailyCollectionDetail.GetLastCollectionDate();
            var htmlString = FileContentReader.SendBalanceHtml;
            StringBuilder rowData = new StringBuilder();
            string returnTypeText = "";

            #region Monthly-Gold Check

            var onlyGoldMonthly = customersData.Where(w => w.ReturnType == ReturnTypeEnum.GoldMonthly);

            var goldMonthlyData = (onlyGoldMonthly
                    .Select((ac, i) =>
                    new
                    {
                        ac.CustomerSeqNumber,
                        ac.Name,
                        Ba = Transaction.GetTransactionGapsMonthly(ac),
                        ac.LoanAmount,
                        ac.ReturnType

                    }))
                    .OrderByDescending(o => o.Ba.MissedAmount)
                    .Where(w => w.Ba.MissedAmount > 0)
                    .Select((ac, i) =>
                   new
                   {
                       Sno = i + 1,
                       ac.CustomerSeqNumber,
                       ac.Name,
                       ac.Ba,
                       ac.LoanAmount,
                       ac.ReturnType

                   })
                    .ToList();

            goldMonthlyData.ForEach(f =>
            {
                rowData.Append($@"<tr><td>{f.Sno}</td><td>{f.CustomerSeqNumber}</td><td>{f.Name}</td><td>{f.LoanAmount}</td><td>{f.Ba.MissedMonthCount}/{f.Ba.MissedAmount} - {f.Ba.lastTxnDate}</td></tr>");
            });

            returnTypeText = "Gold-Monthly";

            // Issue: we need html not plain text to img.
            // HTMLhelper.HtmlToImg(rowData.ToString(), $"{Path.GetTempPath()}D-{currentBalanceDate.Plainddmmyyyy()}.jpg");

            AppCommunication.SendBalanceEmail(htmlString.Replace("[data]", rowData.ToString()).Replace("[title]", $"{returnTypeText} Check").Replace("[LastCol]", "MM/MA")
                , currentBalanceDate, $"{goldMonthlyData.Count()}/{onlyGoldMonthly.Count()}", $"{ returnTypeText} Check");
            rowData.Clear();

          

            #endregion "Monthly-Gold Check"

            #region Monthly Check

            var onlyMonthly = customersData.Where(w => w.ReturnType == ReturnTypeEnum.Monthly);

            var monthlyData = (onlyMonthly
                    .Select((ac, i) =>
                    new
                    {
                        ac.CustomerSeqNumber,
                        ac.Name,
                        Ba = Transaction.GetTransactionGapsMonthly(ac),
                        ac.LoanAmount,
                        ac.ReturnType

                    }))
                    .OrderByDescending(o => o.Ba.MissedAmount)
                    .Where(w => w.Ba.MissedAmount > 0)
                    .Select((ac, i) =>
                   new
                   {
                       Sno = i + 1,
                       ac.CustomerSeqNumber,
                       ac.Name,
                       ac.Ba,
                       ac.LoanAmount,
                       ac.ReturnType

                   })
                    .ToList();

            monthlyData.ForEach(f =>
            {
                rowData.Append($@"<tr><td>{f.Sno}</td><td>{f.CustomerSeqNumber}</td><td>{f.Name}</td><td>{f.LoanAmount}</td><td>{f.Ba.MissedMonthCount}/{f.Ba.MissedAmount} - {f.Ba.lastTxnDate}</td></tr>");
            });

            returnTypeText = "Monthly";

            AppCommunication.SendBalanceEmail(htmlString.Replace("[data]", rowData.ToString()).Replace("[title]", $"{returnTypeText} Check").Replace("[LastCol]", "MM/MA")
                , currentBalanceDate, $"{monthlyData.Count}/{onlyMonthly.Count()}", $"{ returnTypeText} Check");
            rowData.Clear();

            #endregion "Monthly Check"

            #region "TenMonths Check"

            var onlyTenMonths = customersData.Where(w => w.ReturnType == ReturnTypeEnum.TenMonths);

            var tenMonthsData = (onlyTenMonths
                    .Select((ac, i) =>
                    new
                    {
                        ac.CustomerSeqNumber,
                        ac.Name,
                        Ba = Transaction.GetTransactionGapsMonthly(ac),
                        ac.LoanAmount,
                        ac.ReturnType

                    }))
                    .OrderByDescending(o => o.Ba.MissedAmount)
                    .Where(w => w.Ba.MissedAmount > 0)
                    .Select((ac, i) =>
                   new
                   {
                       Sno = i + 1,
                       ac.CustomerSeqNumber,
                       ac.Name,
                       ac.Ba,
                       ac.LoanAmount,
                       ac.ReturnType

                   })
                    .ToList();

            tenMonthsData.ForEach(f =>
            {
                rowData.Append($@"<tr><td>{f.Sno}</td><td>{f.CustomerSeqNumber}</td><td>{f.Name}</td><td>{f.LoanAmount}</td><td>{f.Ba.MissedMonthCount}/{f.Ba.MissedAmount} - {f.Ba.lastTxnDate}</td></tr>");
            });

            returnTypeText = "TenMonths";

            AppCommunication.SendBalanceEmail(htmlString.Replace("[data]", rowData.ToString()).Replace("[title]", $"{returnTypeText} Check").Replace("[LastCol]", "MM/MA")
                , currentBalanceDate, $"{tenMonthsData.Count}/{onlyTenMonths.Count()}", $"{ returnTypeText} Check");
            rowData.Clear();

            #endregion "TenMonths Check"

            #region "Weekly Check"


            var onlyWeekly = customersData.Where(w => w.ReturnType == ReturnTypeEnum.Weekly);
            var weeklyData = (onlyWeekly
                    .Select((ac, i) =>
                    new
                    {
                        ac.CustomerSeqNumber,
                        ac.Name,
                        Ba = Transaction.GetTransactionGapsWeekly(ac),
                        ac.LoanAmount,
                        ac.ReturnType

                    }))
                    .OrderByDescending(o => o.Ba.WeekDiff)
                    .Where(w => w.Ba.WeekDiff > 0)
                    .Select((ac, i) =>
                   new
                   {
                       Sno = i + 1,
                       ac.CustomerSeqNumber,
                       ac.Name,
                       ac.Ba,
                       ac.LoanAmount,
                       ac.ReturnType

                   })
                    .ToList();

            weeklyData.ForEach(f =>
            {
                rowData.Append($@"<tr><td>{f.Sno}</td><td>{f.CustomerSeqNumber}</td><td>{f.Name}</td><td>{f.LoanAmount}</td><td>{f.Ba.DataForCol}</td></tr>");
            });

            returnTypeText = "Weekly";

            AppCommunication.SendBalanceEmail(htmlString.Replace("[data]", rowData.ToString()).Replace("[title]", $"{returnTypeText} Check").Replace("[LastCol]", "E Vs A-MW")
                , currentBalanceDate, $"{weeklyData.Count}/{onlyWeekly.Count()}", $"{ returnTypeText} Check");
            rowData.Clear();

            #endregion "Weekly Check"

            #region "Daily Check"

            var onlyDaily = data.Where(w => w.ReturnType == ReturnTypeEnum.Daily);

            var dailyData = onlyDaily.Where(w => w.Ba.Diff > 1)
                    .OrderByDescending(o => o.Ba.Diff).Select((ac, i) =>
                    new
                    {
                        Sno = i + 1,
                        ac.CustomerSeqNumber,
                        ac.Name,
                        ac.Ba,
                        ac.LoanAmount,
                        ac.ReturnType

                    })
                    .ToList();

            dailyData.ForEach(f =>
            {
                rowData.Append($@"<tr><td>{f.Sno}</td><td>{f.CustomerSeqNumber}</td><td>{f.Name}</td><td>{f.LoanAmount}</td><td>{f.Ba.DataForColumn}</td></tr>");
            });
            returnTypeText = "Daily";

            AppCommunication.SendBalanceEmail(htmlString.Replace("[data]", rowData.ToString()).Replace("[title]", $"{returnTypeText} Check").Replace("[LastCol]", "Txn Detail")
                , currentBalanceDate, $"{dailyData.Count}/{onlyDaily.Count()}", $"{ returnTypeText} Check");

            rowData.Clear();

            #endregion "Daily Check"


        }

        private void SendEmailForSendBalance()
        {
            var activeCus = Customer.GetAllActiveCustomer();
            var htmlString = FileContentReader.SendBalanceHtml;

            var data = activeCus.OrderBy(o => o.Name).Select((ac, i) =>
                        new
                        {
                            Sno = i + 1,
                            ac.CustomerSeqNumber,
                            ac.Name,
                            Ba = Transaction.GetBalanceAndLastDate(ac),
                            ac.LoanAmount
                        }).ToList();



            if (data.Count == 0) return;

            StringBuilder rowData = new StringBuilder();


            data.ForEach(f =>
            {
                rowData.Append($@"<tr><td>{f.Sno}</td><td>{f.CustomerSeqNumber}</td><td>{f.Name}</td><td>{f.LoanAmount}</td><td>{f.Ba.DataForColumn}</td></tr>");
            });

            var dailyCheckHTML = htmlString.Replace("[data]", rowData.ToString()).Replace("[title]", "Balance Report").Replace("[LastCol]", "Txn Detail");

            AppCommunication.SendBalanceEmail(dailyCheckHTML, currentBalanceDate, data.Count().ToString(), "JF Bal.Report");

        }

        private void btnCheckPrivateReport_Click(object sender, EventArgs e)
        {
            ReportRun(true);
        }

        private void btnNotGiven_Click(object sender, EventArgs e)
        {
            //ReportRun();
        }

        private void btnCheckin_Click(object sender, EventArgs e)
        {
            //GitHubClient.Commit();
        }
    }
}
