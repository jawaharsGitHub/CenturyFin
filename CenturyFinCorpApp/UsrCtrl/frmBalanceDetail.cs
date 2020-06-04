using DataAccess.ExtendedTypes;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmBalanceDetail : UserControl
    {
        public frmBalanceDetail()
        {
            InitializeComponent();

            dataGridView1.DataSource = BalanceDetail.GetAll();


        }
    }
}
