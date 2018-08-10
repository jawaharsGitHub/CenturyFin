﻿using DataAccess.PrimaryTypes;
using System;
using System.Drawing;
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
            GetDailyTxn(DateTime.Today.AddDays(-1), true);

        }

        private void GetDailyTxn(DateTime date, bool isOnLoad)
        {
            dailyTxn = DailyCollectionDetail.GetDailyTxn(date, isOnLoad);
            if (dailyTxn == null)
            {
                MessageBox.Show("No record found");
                return;
            }

            txtSanthanam.Text = dailyTxn.SanthanamUncle.ToString();
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

            btnYesterdayInHand.Text = dailyTxn.YesterdayAmountInHand.ToString();
            btnTodayInHand.Text = dailyTxn.TodayInHand.ToString();
            btnInBank.Text = dailyTxn.InBank.ToString();
            btnTmrWanted.Text = (dailyTxn.TomorrowDiff > 0) ? dailyTxn.TomorrowDiff.ToString() : $"0 --- (Have Extra {Math.Abs(Convert.ToInt32(dailyTxn.TomorrowDiff))} )";
            btnTmrWanted.BackColor = (dailyTxn.TomorrowDiff > 0) ? Color.Red : Color.Green;
            btnCanGive.Text = (dailyTxn.TodayInHand + dailyTxn.InBank).ToString();

            lblDate.Text = dailyTxn.Date;

            txtComments.Text = dailyTxn.Comments;

            btnCollection.Text = Convert.ToString(Transaction.GetDailyCollectionDetails(DateTime.Today).Sum(s => s.AmountReceived));
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            dailyTxn.Date = dateTimePicker1.Value.ToShortDateString();
            dailyTxn.SanthanamUncle = Convert.ToInt32(txtSanthanam.Text);
            dailyTxn.YesterdayAmountInHand = dailyTxn.TodayInHand;
            dailyTxn.SentFromUSA = Convert.ToDecimal(txtSentFromUSA.Text);
            dailyTxn.BankTxnOut = Convert.ToDecimal(txtBankTxnOut.Text);
            dailyTxn.TakenFromBank = Convert.ToInt32(txtTakenFromBank.Text);
            dailyTxn.InBank = (dailyTxn.InBank + dailyTxn.SentFromUSA - dailyTxn.TakenFromBank - dailyTxn.BankTxnOut);

            dailyTxn.CollectionAmount = Convert.ToInt32(txtCollectionAmount.Text);
            dailyTxn.GivenAmount = Convert.ToInt32(txtGivenAmount.Text);
            dailyTxn.Interest = Convert.ToInt32(txtInterest.Text);
            dailyTxn.ClosedAccounts = Convert.ToInt32(txtClosed.Text);
            dailyTxn.OpenedAccounts = Convert.ToInt32(txtOpened.Text);
            dailyTxn.TomorrowNeed = Convert.ToInt32(txtTmrNeeded.Text);

            dailyTxn.OtherExpenditire = Convert.ToInt32(txtOtherExpenditure.Text);
            dailyTxn.OtherInvestment = Convert.ToInt32(txtOtherInvestment.Text);

            dailyTxn.TodayInHand = (dailyTxn.YesterdayAmountInHand + dailyTxn.CollectionAmount + dailyTxn.TakenFromBank - dailyTxn.GivenAmount + dailyTxn.Interest + dailyTxn.OtherInvestment - dailyTxn.OtherExpenditire);

            dailyTxn.TomorrowDiff = (Convert.ToInt32(txtTmrNeeded.Text) - Convert.ToInt32((dailyTxn.TodayInHand + dailyTxn.InBank)));
            dailyTxn.Comments = txtComments.Text;


            DailyCollectionDetail.AddDaily(dailyTxn);

            // Update In Hand and In Bank amount.
            var inhand = new InHandAndBank()
            {
                Date = dateTimePicker1.Value.ToShortDateString(),
                InBank = dailyTxn.InBank.Value,
                InHandAmount = dailyTxn.TodayInHand.Value
            };

            InHandAndBank.AddInHand(inhand, dailyTxn.TakenFromBank);

        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            GetDailyTxn(dateTimePicker1.Value, false);

        }
    }
}
