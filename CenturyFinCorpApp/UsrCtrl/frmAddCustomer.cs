using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using DataAccess.PrimaryTypes;
using System;
using System.Linq;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmAddCustomer : UserControl
    {
        public frmAddCustomer()
        {
            InitializeComponent();
            cmbExistingCustomer.Visible = false;
            LoadCustomerCollectionType();
            dateTimePicker1.Value = GlobalValue.CollectionDate.Value;

            cmbExistingCustomer.DropDownStyle = ComboBoxStyle.DropDown;
            cmbExistingCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbExistingCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;
            //cmbExistingCustomer.SelectedIndex = 0;

        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if(ReturnTypeEnum.None == (ReturnTypeEnum)cmbReturnType.SelectedItem)
            {
                MessageBox.Show("Please Select Return Type");
                return;
            }
            var nextIds = Customer.GetNextIds();
            var newCustomerId = nextIds.NewCustomerId;
            var nextSeqNo = nextIds.NewCustomerSeqId;
            Customer cus = new Customer();

            if (chkExistingCustomer.Checked)
            {
                cus = (Customer)cmbExistingCustomer.SelectedItem;
                cus.CustomerSeqNumber = nextSeqNo;
                cus.IsExistingCustomer = true;
                cus.AdjustedAmount = null;
                cus.InitialInterest = 0;
                newCustomerId = cus.CustomerId;
                cus.IsActive = false;   //Update Active flag of existing customer.
                cus.IsForceClosed = false;
                cus.MonthlyInterest = 0;
            }
            else
            {
                var isDuplicateName = Customer.IsDuplicateName(txtName.Text);

                if (isDuplicateName)
                {
                    var msg = $"Customer Name [{txtName.Text}] already exist, Please verify!";
                    MessageBox.Show(msg);
                    lblMessage.Text = msg;
                    return;
                }
                cus.CustomerId = newCustomerId;
                cus.Name = txtName.Text;
                cus.PhoneNumber = txtPhone.Text;
                cus.CustomerSeqNumber = nextSeqNo;
            }

            cus.LoanAmount = Convert.ToInt32(txtLoan.Text);
            cus.Interest = Convert.ToInt32(txtInterest.Text);
            cus.InitialInterest = Convert.ToInt32(txtInterest.Text);
            cus.AmountGivenDate = dateTimePicker1.Value;
            cus.ReturnType = (ReturnTypeEnum)cmbReturnType.SelectedItem;
            cus.ReturnDay = (DayOfWeek)cmbReturnDay.SelectedItem;
            cus.CollectionSpotId = cmbCollectionSpot.SelectedValue.ToInt32();

            if (cus.IsMonthly())
            {
                cus.MonthlyInterest = cus.Interest;
            }

            Customer.AddCustomer(cus);
            txtCustomerNo.Text = newCustomerId.ToString();

            // Add First(Default) Transaction.
            var txn = new Transaction()
            {
                AmountReceived = 0,
                CustomerId = cus.CustomerId,
                CustomerSequenceNo = cus.CustomerSeqNumber,
                TransactionId = Transaction.GetNextTransactionId(),
                Balance = cus.LoanAmount,
                TxnDate = dateTimePicker1.Value
            };

            Transaction.AddTransaction(txn);

            // Add Investment
            Investment.AddInvestment(new Investment()
            {
                Amount = cus.LoanAmount,
                Interest = Convert.ToInt16(txtInterest.Text),
                CustomerId = cus.CustomerId,
                CustomerSequenceNo = cus.CustomerSeqNumber
            });

            var nthTimes = Customer.GetAllCustomer().Where(w => w.CustomerId == cus.CustomerId).Count();

            lblMessage.Text = $"Customer {cus.Name} ({nthTimes}{General.GetDaySuffix(nthTimes)} times) Added Successfully.";
            lblNoteCount.Text = cus.CustomerSeqNumber.ToString();

        }

        private void chkExistingCustomer_CheckedChanged(object sender, EventArgs e)
        {
            cmbExistingCustomer.Visible = (sender as CheckBox).Checked;

            if ((sender as CheckBox).Checked)
            {

                // load existing customer
                cmbExistingCustomer.DataSource = Customer.GetAllCustomer(); // Customer.GetAllCustomer().(d => d.CustomerId).OrderBy(o => o.Name).ToList().Where(w => w.Name.StartsWith("Rab")).ToList();
                cmbExistingCustomer.DisplayMember = "NameAndId";
                cmbExistingCustomer.ValueMember = "CustomerId";
                txtName.Enabled = txtPhone.Enabled = false;

            }
            else
            {
                txtName.Enabled = txtPhone.Enabled = true;
            }
        }

        private void txtLoan_Leave(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtLoan.Text)) return;

            var loanAmount = Convert.ToInt32(txtLoan.Text);

            var interest = (loanAmount / 100) * 10;
            txtInterest.Text = interest.ToString();

        }

        private void cmbExistingCustomer_TextChanged(object sender, EventArgs e)
        {
            cmbExistingCustomer.DroppedDown = false;
        }

        private void LoadCustomerCollectionType()
        {
            cmbReturnType.DataSource = Enum.GetValues(typeof(ReturnTypeEnum));
            cmbReturnDay.DataSource = Enum.GetValues(typeof(DayOfWeek));

            cmbCollectionSpot.DataSource = Customer.GetAllUniqueCustomers();
            cmbCollectionSpot.ValueMember = "CustomerId";
            cmbCollectionSpot.DisplayMember = "Name";


            cmbReturnType.SelectedItem = ReturnTypeEnum.None;
            cmbReturnDay.SelectedItem = DayOfWeek.Sunday;
            cmbCollectionSpot.SelectedValue = 0;
        }

        private void cmbExistingCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCustomer = cmbExistingCustomer.SelectedItem as Customer;

            if (selectedCustomer.GivenEligibility == false)
            {
                MessageBox.Show($"{selectedCustomer.Name} is not eligible for loan. Sorry!!!");
                this.Enabled = false;
            }
        }
    }
}
