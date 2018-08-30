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

namespace CenturyFinCorpApp.UsrCtrl
{
    public partial class frmDataCheck : UserControl
    {
        public frmDataCheck()
        {
            InitializeComponent();

            comboBox1.DataSource = GetOptions();

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
    }
}
