using Common;
using DataAccess.PrimaryTypes;
using System.Windows.Forms;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmPetrol : UserControl
    {
        public frmPetrol()
        {
            InitializeComponent();
            LoadPetrolDetails();
        }

        private void LoadPetrolDetails()
        {
            var data = BaseClass.ReadFileAsObjects<Petrol>(Petrol.JsonFilePath);
            //data.Add(new Petrol());
            dgvPetrol.DataSource = data;
        }
    }
}
