using DataAccess;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class frmDailyEntry : Form
    {
        public frmDailyEntry()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var txn = Transaction.GetDailyCollectionDetails(dateTimePicker1.Value);

            var cus = from c in Customer.GetAllCustomer()
                      select new { c.CustomerId, c.Name };

            var result = (from t in txn
                         join c in cus
                         on t.CustomerId equals c.CustomerId
                         select new
                         {
                             t.TransactionId,
                             t.TxnDate,
                             c.Name,
                             t.AmountReceived,
                             t.Balance
                         }).Distinct();

            label1.Text = $"Total Collection is: {result.Sum(s => s.AmountReceived)}";
            dataGridView1.DataSource = result.ToList();

            //dateTimePicker1.Value = dateTimePicker1.Value.AddDays(1);
        }
    }
}
