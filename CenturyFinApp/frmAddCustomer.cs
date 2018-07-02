using DataAccess;
using System;
using System.Linq;
using System.Windows.Forms;

namespace WindowsFormsApplication1
{
    public partial class frmAddCustomer : Form
    {
        public frmAddCustomer()
        {
            InitializeComponent();
        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (cmbInvestmentType.Text.Trim() == string.Empty)
            {
                MessageBox.Show("Please select Investment Type");
                return;
            }

            var invstType = (InvestmentFrom)Enum.Parse(typeof(InvestmentFrom), cmbInvestmentType.Text);


            var newCustomerId = Customer.GetNextCustomerId();
            var nextSeqNo = Customer.GetNextCustomerSeqNo();
            Customer cus = new Customer();
            
            if (chkExistingCustomer.Checked)
            {
                cus = (Customer)cmbExistingCustomer.SelectedItem;
                cus.CustomerSeqNumber = nextSeqNo;
                cus.IsExistingCustomer = true;
                newCustomerId = cus.CustomerId;

                //Update Active flag of existing customer.
                cus.IsActive = false;
                //Customer.UpdateCustomerDetails(cus);
            }
            else
            {
                cus.CustomerId = newCustomerId;
                cus.Name = txtName.Text;
                cus.PhoneNumber = txtPhone.Text;
                cus.CustomerSeqNumber = nextSeqNo;
            }

            cus.LoanAmount = Convert.ToInt32(txtLoan.Text);


            cus.Interest = Convert.ToInt32(txtInterest.Text);

            Customer.AddCustomer(cus);
            txtCustomerNo.Text = newCustomerId.ToString();

            // Add Investment

            Investment.AddInvestment(new Investment()
            {

                Amount = cus.LoanAmount,
                Interest = Convert.ToInt16(txtInterest.Text),
                CustomerId = cus.CustomerId,
                CustomerSequenceNo = cus.CustomerSeqNumber,
                InvestType = invstType

            });

            lblMessage.Text = $"Customer {cus.Name} Added Successfully";

            //Update InHand Money


            InHand.ReduceInHand(cus.LoanAmount);
            InHand.AddInHand(cus.Interest);

        }

        private void chkExistingCustomer_CheckedChanged(object sender, EventArgs e)
        {
            cmbExistingCustomer.Visible = (sender as CheckBox).Checked;

            if ((sender as CheckBox).Checked)
            {
                // load existing customer
                cmbExistingCustomer.DataSource = Customer.GetAllCustomer();
                cmbExistingCustomer.DisplayMember = "Name";
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
            var loanAmount = Convert.ToInt32(txtLoan.Text);

            var interest = (loanAmount / 100) * 8;
            txtInterest.Text = interest.ToString();

        }

        private void cmbExistingCustomer_TextChanged(object sender, EventArgs e)
        {

            //cmbExistingCustomer.DataSource = null;
            // load existing customer
            //cmbExistingCustomer.DataSource = Customer.GetAllCustomer().Where(w => w.Name.Contains(cmbExistingCustomer.Text)).ToList();
            //cmbExistingCustomer.DisplayMember = "Name";
            //cmbExistingCustomer.ValueMember = "CustomerId";
            //txtName.Enabled = txtPhone.Enabled = false;

            cmbExistingCustomer.DroppedDown = false;




        }
    }
}
