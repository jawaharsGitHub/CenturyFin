using DataAccess;
using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class CustomerTransaction : Form
    {
        int _customerId;
        int _sequeneNo;
        int _loan;
        int _balance;
        string _customerName;
        bool _isClosedTx = false;

        private Customer _customer;


        public CustomerTransaction()
        {
            InitializeComponent();
        }

        public CustomerTransaction(int sequenceNo, int customerId, int loan, string customerName, bool isClosedTx)
        {
            InitializeComponent();

            _customerId = customerId;
            _sequeneNo = sequenceNo;
            _loan = loan;
            _customerName = customerName;
            _isClosedTx = isClosedTx;

            _balance = _isClosedTx ? 0 : Transaction.GetBalance(_loan, _sequeneNo, _customerId);



            btnLoan.Text = _customer.LoanAmount.ToString();
            btnBalance.Text = _balance.ToString();
            this.Text = $"{_customer.Name} - CutomerId: {_customer.CustomerId} SequenceNo: {_customer.CustomerSeqNumber}";
            txtCollectionAmount.Text = (_customer.LoanAmount/ 100).ToString();

            InitializeListView();
            LoadTxn();
        }

        public Transaction AddTxn(Customer cus, DateTime txnDate)
        {
            var txn = new Transaction()
            {
                AmountReceived = Convert.ToInt16(txtCollectionAmount.Text),
                CustomerId = _customerId,
                CustomerSequenceNo = _sequeneNo,
                TransactionId = Transaction.GetNextTransactionId(),
                Balance = (Transaction.GetBalance(_loan, _sequeneNo, _customerId) - Convert.ToInt16(txtCollectionAmount.Text)),
                TxnDate = dateTimePicker1.Value

            };

            //var txn = new Transaction()
            //{
            //    TxnCustomer = cus,
            //    TransactionId = Transaction.GetNextTransactionId(),
            //    //Balance = (Transaction.GetBalance(_loan, _sequeneNo, _customerId) - Convert.ToInt16(txtCollectionAmount.Text)),
            //    TxnDate = dateTimePicker1.Value

            //};


            if (txn.Balance < 0)
            {
                MessageBox.Show("Please check that ur txn is overpaid. Txn Cancelled");
                return null;
            }

            Transaction.AddTransaction(txn);
            return txn;

        }

        private void btnAddTxn_Click(object sender, EventArgs e)
        {
            var txn = AddTxn(_customer, dateTimePicker1.Value);
            if (txn == null) return;
            //var txn = new Transaction()
            //{
            //    AmountReceived = Convert.ToInt16(txtCollectionAmount.Text),
            //    CustomerId = _customerId,
            //    CustomerSequenceNo = _sequeneNo,
            //    TransactionId = Transaction.GetNextTransactionId(),
            //    Balance = (Transaction.GetBalance(_loan, _sequeneNo, _customerId) - Convert.ToInt16(txtCollectionAmount.Text)),
            //    TxnDate = dateTimePicker1.Value

            //};

            //if (txn.Balance < 0)
            //{
            //    MessageBox.Show("Please check that ur txn is overpaid. Txn Cancelled");
            //    return;
            //}

            //Transaction.AddTransaction(txn);

            btnBalance.Text = txn.Balance.ToString();
            LoadTxn();
            if (txn.Balance == 0) MessageBox.Show("This Txn is completed Successfully!");

            lblMessage.Text = $"Txn  Added Successfully for {_customer.Name}";

            // Add InHand
            InHand.AddInHand(txn.AmountReceived);



        }


        private void InitializeListView()
        {

        }

        private void LoadTxn()
        {

            var txns = Transaction.GetTransactionDetails(_customerId, _sequeneNo, _isClosedTx);

            if (txns == null) return;


            var dataDource = from t in txns
                             
                                       select new
                                       {
                                           t.TransactionId,
                                           t.TxnDate,
                                           t.AmountReceived,
                                           t.Balance
                                       };

            dataGridView1.DataSource = dataDource.OrderByDescending(o => o.TxnDate).ToList();


            lblStartDate.Text = $"Start Date: {dataDource.Select(s => s.TxnDate).Min()}";
            lblLastDate.Text = $"Last Date: {dataDource.Select(s => s.TxnDate).Max()}";


            dataGridView1.AutoResizeColumns(DataGridViewAutoSizeColumnsMode.AllCellsExceptHeader);
        }
    }
}
