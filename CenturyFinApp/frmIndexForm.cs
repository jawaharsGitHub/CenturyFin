﻿using Common;
using DataAccess;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class frmIndexForm : Form
    {

        public frmIndexForm()
        {
            InitializeComponent();
            LoadAllData();



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
            frmAddCustomer ac = new frmAddCustomer();
            ac.ShowDialog();
        }

        private void btnCustomers_Click(object sender, EventArgs e)
        {
            frmCustomers ac = new frmCustomers();
            ac.ShowDialog();
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

                Customer.UpdateCustomerDetails(new Customer() { CustomerId = item.CustomerId, CustomerSeqNumber = item.CustomerSequenceNo, IsActive = false });

            }


        }

        

        private void btnAdd_Click(object sender, EventArgs e)
        {
            InHand.AddInHand(Convert.ToInt32(txtJawaInvestment.Text), fromJawahar : true);
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

            var assets = (Transaction.GetAllOutstandingAmount() + InHand.GetAllhandMoney().InHandAmount) - GetAllExpenditure();
            btnTotalAssets.Text = $"Total Assets Amount: {Environment.NewLine}{assets }";
            return assets;

        }

        private void InHandMoney()
        {
            var jawaInvestment = InHand.GetAllhandMoney();
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
            return InHand.GetAllhandMoney().JawaharShare; //.GetAllInvestmet().Where(w => w.InvestType == InvestmentFrom.Jawahar).Sum(s => s.Amount); 

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