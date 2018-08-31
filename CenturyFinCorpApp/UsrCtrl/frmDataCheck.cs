using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using DataAccess.PrimaryTypes;
using Common;
using Common.ExtensionMethod;

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmDataCheck : UserControl
    {
        public frmDataCheck()
        {
            InitializeComponent();

            comboBox1.DataSource = GetOptions();

            CheckEachTxn();

        }

        public static List<KeyValuePair<int, string>> GetOptions()
        {
            var myKeyValuePair = new List<KeyValuePair<int, string>>()
               {
                   new KeyValuePair<int, string>(1, "Check MinBalance And AmountReceived"),
               };

            return myKeyValuePair;

        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            var value = ((KeyValuePair<int, string>)comboBox1.SelectedItem).Key;

            if (value == 1)
            {
                MinBalanceAndReceivedAmount();
            }

        }

        private void MinBalanceAndReceivedAmount()
        {
            var txn = Transaction.GetTransactionsNotGivenForFewDays();

            var details = (from c in Customer.GetAllCustomer()
                           select new
                           {
                               c.CustomerId,
                               c.CustomerSeqNumber,
                               c.Name,
                               c.LoanAmount,
                               txn = Transaction.GetTransactionDetails(c.CustomerId, c.CustomerSeqNumber, (c.IsActive == false))
                           });

            var data = (from t in details
                        select new
                        {
                            t.CustomerId,
                            t.CustomerSeqNumber,
                            t.Name,
                            t.LoanAmount,
                            TotalReceived = t.txn.Sum(s => s.AmountReceived),
                            MinBalance = t.txn.Min(m => m.Balance),
                            IsCorrect = ((t.LoanAmount - t.txn.Sum(s => s.AmountReceived)) == t.txn.Min(m => m.Balance))
                        }
                        );



            dataGridView1.DataSource = data.Where(w => w.IsCorrect == false).ToList(); ;

        }

        private void CheckEachTxn()
        {
            //var txn = Transaction.GetTransactionsNotGivenForFewDays();

            Transaction currentTxn;
            Transaction nextTxn;

            var details = (from c in Customer.GetAllCustomer()// .Where(w => w.CustomerSeqNumber == 1)
                           select new
                           {
                               c.CustomerId,
                               c.CustomerSeqNumber,
                               c.Name,
                               c.LoanAmount,
                               txn = Transaction.GetTransactionDetails(c.CustomerId, c.CustomerSeqNumber, (c.IsActive == false))
                           }).ToList();

            details.ForEach(f =>
            {

                if (f.txn.Count() == 0)
                {

                    LogHelper.WriteLog($"NO TXN - {f.Name}-{f.CustomerSeqNumber}");

                }
                else
                {

                    var firstItem = f.txn.First();

                    if(firstItem.AmountReceived > 0)
                    {
                        LogHelper.WriteLog($"ADD FIRST TXN {firstItem.TxnDate}-{firstItem.CustomerId}-{firstItem.CustomerSequenceNo}-{f.Name}");

                    }

                    if (firstItem.Balance != f.LoanAmount)
                    {
                        LogHelper.WriteLog($"LOANNOTCORRECT {firstItem.TxnDate}-{firstItem.CustomerId}-{firstItem.CustomerSequenceNo}-{f.Name}");

                    }

                    f.txn.ForEach(t =>
                    {

                        if (f.txn.Where(w => w.TxnDate.Date == t.TxnDate.Date).Count() > 1)
                        {
                            LogHelper.WriteLog($">1 {t.TxnDate}-{t.CustomerId}-{t.CustomerSequenceNo}-{f.Name}");

                        }

                        else if (f.txn.Where(w => w.TxnDate.Date == t.TxnDate.Date).Count() == 0)
                        {
                            LogHelper.WriteLog($"ZERO - {t.TxnDate}-{t.CustomerId}-{t.CustomerSequenceNo}-{f.Name}");

                        }

                        else if (f.txn.Where(w => w.TxnDate.Date == t.TxnDate.Date).Count() == 1)
                        {
                            nextTxn = f.txn.NextOf(t);

                            if (nextTxn != null && nextTxn.Balance != (t.Balance - nextTxn.AmountReceived))
                            {
                                LogHelper.WriteLog($"VERIFY - {t.TxnDate}-{t.CustomerId}-{t.CustomerSequenceNo}-{f.Name}");

                            }


                        }
                        else
                        {
                            LogHelper.WriteLog($"ELSE - {t.TxnDate}-{t.CustomerId}-{t.CustomerSequenceNo}-{f.Name}");

                        }






                    });
                }
            });




            // dataGridView1.DataSource = data.Where(w => w.IsCorrect == false).ToList(); ;

        }


    }
}
