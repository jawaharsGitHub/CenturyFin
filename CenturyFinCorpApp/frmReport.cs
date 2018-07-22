using DataAccess;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmReport : UserControl
    {
        int outstandingMoney = 0;
        public frmReport()
        {
            InitializeComponent();

            ShowOutstandingMoney();
        }

        private void ShowOutstandingMoney()
        {
            outstandingMoney = Transaction.GetAllOutstandingAmount();
            lblOutStanding.Text = outstandingMoney.ToMoney();
        }

        private void ShowFulAssetMoney()
        {
            //lblOutStanding.Text = outstandingMoney;
        }
    }
}
