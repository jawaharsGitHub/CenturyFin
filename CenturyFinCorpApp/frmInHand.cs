using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
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

            btnYesterdayInHand.Text = dailyTxn.YesterdayAmountInHand.ToString();
            btnTodayInHand.Text = dailyTxn.TodayInHand.ToString();
            btnInBank.Text = dailyTxn.InBank.ToString();
            btnTmrWanted.Text = dailyTxn.TomorrowDiff.ToString();

            lblDate.Text = dailyTxn.Date;

            txtComments.Text = dailyTxn.Comments;
        }

        private void btnUpdate_Click(object sender, EventArgs e)
        {

            dailyTxn.Date = dateTimePicker1.Value.ToShortDateString();
            dailyTxn.SanthanamUncle = Convert.ToInt32(txtSanthanam.Text);
            dailyTxn.YesterdayAmountInHand = dailyTxn.TodayInHand;
            dailyTxn.SentFromUSA = Convert.ToInt32(txtSentFromUSA.Text);
            dailyTxn.BankTxnOut = Convert.ToInt32(txtBankTxnOut.Text);
            dailyTxn.TakenFromBank = Convert.ToInt32(txtTakenFromBank.Text);
            dailyTxn.InBank = (dailyTxn.InBank + dailyTxn.SentFromUSA - dailyTxn.TakenFromBank - dailyTxn.BankTxnOut);

            dailyTxn.CollectionAmount = Convert.ToInt32(txtCollectionAmount.Text);
            dailyTxn.GivenAmount = Convert.ToInt32(txtGivenAmount.Text);
            dailyTxn.Interest = Convert.ToInt32(txtInterest.Text);
            dailyTxn.ClosedAccounts = Convert.ToInt32(txtClosed.Text);
            dailyTxn.OpenedAccounts = Convert.ToInt32(txtOpened.Text);
            dailyTxn.TomorrowNeed = Convert.ToInt32(txtTmrNeeded.Text);
            dailyTxn.TodayInHand = (dailyTxn.YesterdayAmountInHand + dailyTxn.CollectionAmount + dailyTxn.TakenFromBank - dailyTxn.GivenAmount + dailyTxn.Interest);
            dailyTxn.TomorrowDiff = (Convert.ToInt32(txtTmrNeeded.Text) - dailyTxn.TodayInHand);
            dailyTxn.Comments = txtComments.Text;

            DailyCollectionDetail.AddDaily(dailyTxn);


        }

        private void btnShow_Click(object sender, EventArgs e)
        {
            GetDailyTxn(dateTimePicker1.Value, false);

        }
    }
}
