﻿using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using System;
using System.Diagnostics;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmInHand : UserControl
    {

        private DailyCollectionDetail dailyTxn;

        public frmInHand()
        {
            InitializeComponent();

            GetDailyTxn(GlobalValue.CollectionDate.Value.AddDays(-1), true);
            GetDailyTxn(GlobalValue.CollectionDate.Value, true);

        }

        private void EnableEdit()
        {
            groupBox1.Enabled = txtComments.Enabled = true;
        }

        private void GetDailyTxn(DateTime date, bool isOnLoad)
        {
            dailyTxn = DailyCollectionDetail.GetDailyTxn(date, isOnLoad);

            var haveNoDailytnx = (dailyTxn == null);
            groupBox1.Enabled = txtComments.Enabled = !haveNoDailytnx;

            if (haveNoDailytnx)
            {
                dailyTxn = DailyCollectionDetail.GetDailyTxn(date.AddDays(-1), isOnLoad);
                dateTimePicker1.Value = date;

                var collectionAmount = Transaction.GetDailyCollectionDetails_V0(dateTimePicker1.Value).Where(w => w.AmountReceived > 0).Sum(s => s.AmountReceived);
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
                    txtInterest.Text = (todayTxn.Where(w => w.AmountReceived == 0 &&  w.Balance > 0).Sum(s => s.Interest) + topupCustomers.Sum(s => s.Interest)).ToString();
                    txtClosed.Text = todayTxn.Where(w => w.Balance == 0).Count().ToString();
                    txtOpened.Text = todayTxn.Where(w => w.AmountReceived == 0).Count().ToString();

                    txtOtherExpenditure.Text = txtOtherInvestment.Text = txtOutMoney.Text = "0";

                }
                //btnEnable.Text = Convert.ToString(Transaction.GetDailyCollectionDetails_V0(dateTimePicker1.Value).Sum(s => s.AmountReceived));
                lblDate1.Text = lblDate2.Text = $"{dateTimePicker1.Value.ToShortDateString()} NOT FOUND";
                btnAdd.Text = "ADD";
                return;
            }

            lblDate1.Text = lblDate2.Text = $"Data For {dailyTxn.Date}";
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
            btnMama.Text = "M:" +dailyTxn.MamaAccount.TokFormat();
            btnInvestment.Text = "INV:"  + DailyCollectionDetail.GetActualInvestmentTxnDate(dateTimePicker1.Value).ActualMoneyInBusiness.ToMoneyFormat();

            lblCanGive.Text = $"Actually we can give - {(dailyTxn.TodayInHand / 4500) * 5000} {Environment.NewLine}with extra {(dailyTxn.TodayInHand % 4500)}";
            btnInBank.Text = dailyTxn.InBank.TokFormat();
            btnTmrWanted.Text = (dailyTxn.TomorrowDiff > 0) ? dailyTxn.TomorrowDiff.ToString() : $"0 --- (Have Extra {Math.Abs(Convert.ToInt32(dailyTxn.TomorrowDiff))} )";
            btnTmrWanted.BackColor = (dailyTxn.TomorrowDiff > 0) ? Color.Red : Color.Green;
            btnCanGive.Text = (dailyTxn.TodayInHand + dailyTxn.InBank).TokFormat();
            txtOutMoney.Text = dailyTxn.OutUsedMoney.ToString();


            // Collection SUmmary
            UpdateVerifyDetails();

            txtComments.Text = dailyTxn.Comments;

            //btnEnable.Text = Convert.ToString(Transaction.GetDailyCollectionDetails_V0(dateTimePicker1.Value).Sum(s => s.AmountReceived));
        }

        private void UpdateVerifyDetails()
        {
            txtInputMoney.Text = dailyTxn.InputMoney.ToString();
            txtOutusedMoney.Text = dailyTxn.OutGoingMoney.ToString();
            txtInvsOutDiff.Text = dailyTxn.Difference.ToString();
            txtExpectedInHand.Text = dailyTxn.ExpectedInHand.ToString();
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

            dailyTxn.TodayInHand =   (dailyTxn.YesterdayAmountInHand + dailyTxn.CollectionAmount + dailyTxn.TakenFromBank - dailyTxn.GivenAmount + dailyTxn.Interest + dailyTxn.OtherInvestment - dailyTxn.OtherExpenditire);

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

            GetDailyTxn(dateTimePicker1.Value, false);



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
            dailyTxn.ActualInHand = actualInhand;
            dailyTxn.MamaAccount = mamaAccount;
            dailyTxn.MamaExpenditure = txtMamaExpenditure.Text.ToInt32();
            dailyTxn.MamaInputMoney = txtMamaInputMoney.Text.ToInt32();

            DailyCollectionDetail.UpdateVerifyDetails(dailyTxn);
            UpdateVerifyDetails();

            var fileName = General.GetDataFolder("CenturyFinCorpApp\\bin\\Debug", "DataAccess\\Data\\CollectionSummary\\") + $"CxnSum_{dateTimePicker1.Value.ToShortDateString()}.txt";

            using (TextWriter tw = new StreamWriter(fileName))
            {
                tw.WriteLine($"@Collection Summary for {dateTimePicker1.Value.ToShortDateString()}");
                tw.WriteLine($"------------------------------------------------");

                tw.WriteLine($"Total input Money = ActualInhand inhand yesterday[{dailyTxn.ActualInHand.TokFormat()}] + collection [{txtCollectionAmount.Text}] + interest[{txtInterest.Text}] + other investment [{txtOtherInvestment.Text}] = [{inputMoney.TokFormat()}]");
                tw.WriteLine(Environment.NewLine);
                tw.WriteLine($"Total out used money = given amount [{txtGivenAmount.Text}] + other expenditure [{txtOtherExpenditure.Text}] = [{outUsedMoney.TokFormat()}]");
                tw.WriteLine(Environment.NewLine);
                tw.WriteLine($"Expected Inhand: {dailyTxn.ExpectedInHand.TokFormat()} Actual Inhand : {dailyTxn.ActualInHand.TokFormat()} MamaAccount: {dailyTxn.MamaAccount.TokFormat()} ");
            }

            string s = txtComments.Text;
            int start = s.IndexOf("$");
            int end = s.LastIndexOf("$");
            string toBeReplaced = s.Substring(start + 1, end - start - 1);
            //s = s.Replace(result, "your replacement value");

            txtComments.Text = s.Replace(toBeReplaced, $"{Environment.NewLine}{dailyTxn.ExpectedInHand.TokFormat()} [In Company]{Environment.NewLine}{dailyTxn.ActualInHand.TokFormat()} [In Hand]{Environment.NewLine}{mamaAccount.TokFormat()} [In Mama]{Environment.NewLine}");
            btnAddOrUpdate_Click(null, null);

            Process.Start(fileName);
        }

        private void btnDelete_Click(object sender, EventArgs e)
        {
            DailyCollectionDetail.DeleteDailyCxnDetails(dateTimePicker1.Value);
            MessageBox.Show($"Deleted collection details for {dateTimePicker1.Value.ToShortDateString()}");
            GetDailyTxn(dateTimePicker1.Value, false);

        }

        private void btnEnable_Click(object sender, EventArgs e)
        {
            EnableEdit();
        }
    }
}
