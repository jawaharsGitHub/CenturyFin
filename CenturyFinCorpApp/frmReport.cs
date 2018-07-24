﻿using Common;
using DataAccess;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            ShowFulAssetMoney();
        }

        private void ShowOutstandingMoney()
        {
            outstandingMoney = Transaction.GetAllOutstandingAmount();
            lblOutStanding.Text = outstandingMoney.ToMoney();
        }

        private void ShowFulAssetMoney()
        {
            var inHandAndBank = InHandAndBank.GetAllhandMoney();
            lblTotalAsset.Text = (outstandingMoney + inHandAndBank.InHandAmount + inHandAndBank.InBank).ToMoney();
        }


        private void btnClosedTxn_Click(object sender, System.EventArgs e)
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

        private void btnRefresh_Click(object sender, System.EventArgs e)
        {
            btnClosedTxn.Text = $"Run Closed Txn ({Transaction.GetClosedTxn()})";
        }
    }
}
