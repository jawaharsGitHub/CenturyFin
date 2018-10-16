using Common;
using Common.ExtensionMethod;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmConfig : UserControl
    {
        public frmConfig()
        {
            InitializeComponent();
        }

        private void btnConfig_Click(object sender, System.EventArgs e)
        {
            AppConfiguration.AddOrUpdateAppSettings("interest", txtInterest.Text);

        }
    }
}
