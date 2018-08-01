using CenturyFinCorpApp;
using Common;
using DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmIndexForm : Form
    {

        bool usingMenu = false;
        bool isAdded = false; // for child forms

        public frmIndexForm()
        {

            InitializeComponent();

            //this.TopMost = true;
            this.AutoScrollOffset = new Point(0, 0);

            LoadAllData();


            // Menu
            var menuStrip = new MenuStrip();
            menuStrip.Location = new Point(0, 0);
            menuStrip.Name = "MenuStrip";
            //Customer
            var mnuCustomer = new ToolStripMenuItem() { Name = "Customer", Text = "Customers" };
            mnuCustomer.Click += (s, e) => ShowForm<frmCustomers>(); ;
            menuStrip.Items.Add(mnuCustomer);
            //Add customer
            var mnuAddCustomer = new ToolStripMenuItem() { Name = "AddCustomer", Text = "Add Customer" };
            menuStrip.Items.Add(mnuAddCustomer);
            mnuAddCustomer.Click += (s, e) => ShowForm<frmAddCustomer>(); ;
            //Daily Collection
            var mnuDailyCollection = new ToolStripMenuItem() { Name = "DailyColl", Text = "Daily Collection" };
            menuStrip.Items.Add(mnuDailyCollection);
            mnuDailyCollection.Click += (s, e) => ShowForm<frmDailyEntry>(); ;
            //ShowInHand
            var mnuShowInHand = new ToolStripMenuItem() { Name = "InHand", Text = "Shown In Hand" };
            menuStrip.Items.Add(mnuShowInHand);
            mnuShowInHand.Click += (s, e) => ShowForm<frmInHand>(); ;
            //Reports
            var mnuReport = new ToolStripMenuItem() { Name = "Report", Text = "Report" };
            menuStrip.Items.Add(mnuReport);
            mnuReport.Click += (s, e) => ShowForm<frmReport>();

            this.Controls.Add(menuStrip);

            usingMenu = Convert.ToBoolean(ConfigurationManager.AppSettings["usingMenu"]);

            if (usingMenu)
            {

                button1.Visible = false;
                btnCustomers.Visible = false;
                btnRefresh.Visible = false;
                btnClosedTxn.Visible = false;
                groupBox1.Visible = false;
                groupBox2.Visible = false;
                groupBox3.Visible = false;
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

        public void ShowForm(int transactionId)
        {
            var txn = Transaction.GetTransactionDetail(transactionId);
            var customer = Customer.GetCustomerDetails(txn.CustomerId, txn.CustomerSequenceNo);

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
                var txns = Transaction.GetTransactionDetails(cus.CustomerId, cus.CustomerSeqNumber, (cus.IsActive == false));

                if (txns == null)
                {
                    MessageBox.Show("May be you didn't run Closed Batch, Please run and try this!");
                    return;
                }

                ac = new frmCustomerTransaction(cus) as T;

                //ac.Parent = parentForm;
            }
            //ac = new T();
            //ac.TopLevel = false;

            if (isAdded && panel1.Controls.Count > 0)
            {
                panel1.Controls.RemoveAt(0);
            }


            isAdded = true;


            // very important property, pls dont remove it, then u have to restart ur system!!!
            panel1.Controls.Add(ac);
            if (usingMenu)
            {
                //ac.ControlBox = false;
                //ac.FormBorderStyle = FormBorderStyle.FixedSingle;
                //ac.ShowInTaskbar = false;
            }

            ac.Show();
        }

        private void LoadAllData()
        {
            GetNumberOfClients();
            GetNumberOfActiveClients();
            JawaInvestment();
            ComPanyInv();
            IntOnly();
            TotalAssets();
            InHandMoney();
            Profit();
            GetExpenditure();
            Outstanding();

            ClosedTxn();

            TotalNote();
        }

        private void TotalNote()
        {
            var noOfDistinctClients = GetNumberOfClients();
            var closedNotesCount = Directory.GetFiles(AppConfiguration.BackupFolderPath, "*.*", SearchOption.AllDirectories).Length;
            btnNote.Text = "Total Note: " + (noOfDistinctClients + closedNotesCount);
        }

        private void ClosedTxn()
        {
            btnClosedTxn.Text = $"Run Closed Txn ({Transaction.GetClosedTxn()})";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            frmDailyEntry ac = new frmDailyEntry();
            //ac.ShowDialog();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            frmCustomers ac = new frmCustomers();
            //ac.ShowDialog();
        }


        private void btnClosedTxn_Click(object sender, EventArgs e)
        {
            var json = File.ReadAllText(AppConfiguration.TransactionFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);

            if (list == null || list.Count == 0) return;

            var closedIds = list.Where(w => w.Balance == 0).ToList();

            foreach (var item in closedIds)
            {
                var closedTxn = new List<Transaction>();
                closedTxn.AddRange(list.Where(w => w.CustomerId == item.CustomerId && w.CustomerSequenceNo == item.CustomerSequenceNo));
                // Back up closed txn
                Transaction.AddClosedTransaction(closedTxn);

                // Delete Transactions data
                Transaction.DeleteTransactionDetails(item.CustomerId, item.CustomerSequenceNo);

                // Customer.UpdateCustomerDetails(new Customer() { CustomerId = item.CustomerId, CustomerSeqNumber = item.CustomerSequenceNo, IsActive = false });

            }


        }



        private void btnAdd_Click(object sender, EventArgs e)
        {
            //InHand.AddInHand(Convert.ToInt32(txtJawaInvestment.Text), fromJawahar: true);
        }

        private void btnAddExpen_Click(object sender, EventArgs e)
        {
            DataAccess.Expenditure.AddExpenditure(new DataAccess.Expenditure() { Amount = Convert.ToInt32(txtExpenditure.Text), Reason = txtReason.Text });
        }

        private void btnAllTransaction_Click(object sender, EventArgs e)
        {

        }


        private void GetNumberOfActiveClients()
        {
            var cus = Customer.GetAllCustomer() ?? new List<Customer>();

            var activeCustomers = cus.Where(w => w.IsActive).Count();

            btnActiveClients.Text = $"Active Customers: {Environment.NewLine}{activeCustomers}";

        }

        private int GetNumberOfClients()
        {
            var allCustomers = Customer.GetAllCustomer();

            var customerCount = allCustomers == null ? 0 : allCustomers.Select(s => s.CustomerId).Distinct().Count();
            btnNoClients.Text = $"All Customers: {Environment.NewLine}{customerCount}";
            return customerCount;


        }

        private void JawaInvestment()
        {
            var jawaInvestment = GetJawaInvestment();
            btnJawaInvestment.Text = $"Outgoing From Jawahar Investment: {Environment.NewLine}{jawaInvestment}";
        }

        private void ComPanyInv()
        {
            var jawaInvestment = (Investment.GetAllInvestmet() ?? new List<Investment>()).Where(w => w.InvestType == InvestmentFrom.Company).Sum(s => s.Amount);
            btnComPanyInv.Text = $"OutGoing From Company: {Environment.NewLine}{jawaInvestment}";
        }

        private void IntOnly()
        {
            var jawaInvestment = (Investment.GetAllInvestmet() ?? new List<Investment>()).Sum(s => s.Interest);
            btnIntOnly.Text = $"Interest Only Income: {Environment.NewLine}{jawaInvestment}";

        }

        private int TotalAssets()
        {

            var assets = (Transaction.GetAllOutstandingAmount() + InHandAndBank.GetAllhandMoney().InHandAmount) - GetAllExpenditure();
            btnTotalAssets.Text = $"Total Assets Amount: {Environment.NewLine}{assets }";
            return assets;

        }

        private void InHandMoney()
        {
            var jawaInvestment = InHandAndBank.GetAllhandMoney();
            btnInHand.Text = $"InHand Amount: {Environment.NewLine}{jawaInvestment.InHandAmount}";

        }

        private void Profit()
        {

            var profitOnly = (TotalAssets() - GetJawaInvestment() - GetExpenditure());
            btnProfit.Text = $"Profit Only: {Environment.NewLine}{profitOnly}";


        }

        private int GetAllExpenditure()
        {
            int exp = GetExpenditure();
            btnExpenditure.Text = $"Expenditure Only: {Environment.NewLine}{exp}";
            return exp;

        }

        private int GetJawaInvestment()
        {
            return 0;
            //return InHandAndBank.GetAllhandMoney().JawaharShare; //.GetAllInvestmet().Where(w => w.InvestType == InvestmentFrom.Jawahar).Sum(s => s.Amount); 

        }

        private int GetExpenditure()
        {
            return DataAccess.Expenditure.GetTotalExpenditure();

        }

        private void Outstanding()
        {
            int outStanding = Transaction.GetAllOutstandingAmount();
            btnOutstanding.Text = $"OutStanding Amount: {Environment.NewLine}{outStanding}";

        }

        private void btnRefresh_Click(object sender, EventArgs e)
        {
            LoadAllData();
        }
    }
}
