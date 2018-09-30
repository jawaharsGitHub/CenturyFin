using Common;
using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using System;
using System.Linq;
using System.Windows.Forms;


/*
 	Actual	50KM	40KM	35KM
Rs	1700	1740	1740	1740
KM	700 	1000	800 	700

 */
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
            var data = BaseClass.ReadFileAsObjects<Petrol>(Petrol.JsonFilePath).OrderBy(o => o.Date).ToList(); ;
            dgvPetrol.DataSource = data;

            var firstDate = data.First(f => f.Speedometer > 0);
            var lastDate = data.Last(f => f.Speedometer > 0);

            var totalRunKm = lastDate.Speedometer - firstDate.Speedometer;

            var runningDays = (lastDate.Date - firstDate.Date).TotalDays + 1;

            var avgPrice = data.Where(w => w.Date >= firstDate.Date).Average(a => a.Amount);
            var fullPrice = data.Where(w => w.Date >= firstDate.Date).Sum(a => a.Amount);


            lblAvgKm.Text = $"Average Km PerDay : {(totalRunKm / runningDays).ToInt32()} between {firstDate.Date.ToShortDateString()} and {lastDate.Date.ToShortDateString()} {Environment.NewLine}" +
                $"Running Kms : {totalRunKm} RunningDays : {runningDays}{Environment.NewLine}" +
                $"Amount Price Per Day : {avgPrice}{Environment.NewLine}" +
                $"Full Amount : {fullPrice}";


        }
    }
}
