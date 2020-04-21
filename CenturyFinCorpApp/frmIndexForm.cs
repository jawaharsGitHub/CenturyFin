using Common.ExtensionMethod;
using DataAccess.PrimaryTypes;
using System;
using System.Configuration;
using System.Drawing;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmIndexForm : Form
    {

        bool usingMenu = false;
        bool isAdded = false; // for child forms
        public static MenuStrip menuStrip;

        public frmIndexForm()
        {

            InitializeComponent();

            var customers = Customer.GetAllCustomer().OrderBy(o => o.AmountGivenDate).ToList();


            var activeTxn = customers.Count(c => c.IsActive == true);
            var closedTxn = customers.Count(c => c.IsActive == false);
            var totalTxn = activeTxn + closedTxn;

            var latestCxnDate = DailyCollectionDetail.GetLastCollectionDate();


            var diffDays = (DateTime.Today - latestCxnDate).Days;

            var diffStr = diffDays.ToString();
            if (diffDays == 1)
            {
                diffStr = "(Yesterday)";
            }
            else if (diffDays == 2)
            {
                diffStr = "(Two Days Ago)";
            }
            else //(diffDays >= 3)
            {
                diffStr = $"({diffDays} days ago)";
            }

            this.Text = $"JEYAM FINANACE Ltd. ({DateTime.Today.ToString("dddd, dd MMMM yyyy")}) Running ({activeTxn}) Closed ({closedTxn}) Total ({totalTxn}) - Last Cxn on {latestCxnDate.WithDateSuffix()} {diffStr}";
            this.AutoScrollOffset = new Point(0, 0);

            CreateMenu();

            usingMenu = Convert.ToBoolean(ConfigurationManager.AppSettings["usingMenu"]);

            if (usingMenu)
            {
                panel1.Visible = true;

            }
            else
            {
                menuStrip.Visible = false;
                panel1.Visible = false;
            }

            panel1.Width = 1300;
            panel1.Height = this.Height;

            ShowForm<frmCustomers>(); // initial form to be loaded
        }

        private void CreateMenu()
        {
            // Menu
            menuStrip = new MenuStrip
            {
                Location = new Point(0, 0),
                Name = "MenuStrip"
            };


            //Customer
            var mnuCustomer = new ToolStripMenuItem() { Name = "Customer", Text = "CUSTOMERS" };
            mnuCustomer.Click += (s, e) => ShowForm<frmCustomers>(); ;
            menuStrip.Items.Add(mnuCustomer);
            //Add customer
            var mnuAddCustomer = new ToolStripMenuItem() { Name = "AddCustomer", Text = "ADD-CUSTOMER" };
            menuStrip.Items.Add(mnuAddCustomer);
            mnuAddCustomer.Click += (s, e) => ShowForm<frmAddCustomer>(); ;
            //Daily Collection
            var mnuDailyCollection = new ToolStripMenuItem() { Name = "DailyColl", Text = "DAILY-COLLECTION" };
            menuStrip.Items.Add(mnuDailyCollection);
            mnuDailyCollection.Click += (s, e) => ShowForm<frmDailyEntry>(); ;
            //ShowInHand
            var mnuShowInHand = new ToolStripMenuItem() { Name = "InHand", Text = "INHAND-DETAILS" };
            menuStrip.Items.Add(mnuShowInHand);
            mnuShowInHand.Click += (s, e) => ShowForm<frmInHand>();

            /*
            //General Reports
            var mnuGeneralReport = new ToolStripMenuItem() { Name = "GenReport", Text = "GENERAL-REPORT" };
            menuStrip.Items.Add(mnuGeneralReport);
            mnuGeneralReport.Click += (s, e) => ShowForm<frmGeneralReport>();
            //Dynamic Reports
            var mnuReport = new ToolStripMenuItem() { Name = "DynReport", Text = "DYNAMIC-REPORT" };
            menuStrip.Items.Add(mnuReport);
            mnuReport.Click += (s, e) => ShowForm<frmDynamicReport>();
            //Data Check Report
            var mnuDataCheckRpt = new ToolStripMenuItem() { Name = "dataCheckReport", Text = "DATA-CHECK-REPORT" };
            menuStrip.Items.Add(mnuDataCheckRpt);
            mnuDataCheckRpt.Click += (s, e) => ShowForm<frmDataCheck>();
            //Credit Report
            var mnuCreditReport = new ToolStripMenuItem() { Name = "CreditReport", Text = "CREDIT-REPORT" };
            menuStrip.Items.Add(mnuCreditReport);
            mnuCreditReport.Click += (s, e) => ShowForm<frmCreditReport>();
            //Batches
            var mnuBatch = new ToolStripMenuItem() { Name = "batch", Text = "BATCH-JOBS" };
            menuStrip.Items.Add(mnuBatch);
            mnuBatch.Click += (s, e) => ShowForm<frmBatches>();
            //Outstanding
            var mnuOutstanding = new ToolStripMenuItem() { Name = "Outstanding", Text = "OUTSTANDING" };
            menuStrip.Items.Add(mnuOutstanding);
            mnuOutstanding.Click += (s, e) => ShowForm<frmOutstanding>();
            //Health
            var mnuHealth = new ToolStripMenuItem() { Name = "health", Text = "HEALTH" };
            menuStrip.Items.Add(mnuHealth);
            mnuHealth.Click += (s, e) => ShowForm<frmHealth>();
            //Politics
            var mnuConfigs = new ToolStripMenuItem() { Name = "config", Text = "CONFIGS" };
            menuStrip.Items.Add(mnuConfigs);
            mnuConfigs.Click += (s, e) => ShowForm<frmConfig>();
            //Petrol
            var mnuPetrol = new ToolStripMenuItem() { Name = "petrol", Text = "PETROL" };
            menuStrip.Items.Add(mnuPetrol);
            mnuPetrol.Click += (s, e) => ShowForm<frmPetrol>();

    */

            this.Controls.Add(menuStrip);
        }

        public void ShowForm(int transactionId)
        {
            var txn = Transaction.GetTransactionDetail(transactionId);
            var customer = Customer.GetCustomerDetails(txn);

            ShowForm<frmCustomerTransaction>(customer);
        }
        public void ShowForm<T>(Customer cus = null) where T : UserControl, new()
        {

            T ac;
            if (cus == null)
            {
                ac = new T();
            }
            else
            {
                var txns = Transaction.GetTransactionDetails(cus);

                if (txns == null)
                {
                    MessageBox.Show("May be you didn't run Closed Batch, Please run and try this!");
                    return;
                }

                ac = new frmCustomerTransaction(cus, this) as T;
            }

            if (isAdded && panel1.Controls.Count > 0)
            {
                panel1.Controls.RemoveAt(0);
            }


            isAdded = true;
            panel1.Controls.Add(ac);
            ac.Show();
        }

    }
}
