﻿using Common;
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
            LoadBusinessType();
            this.cmbBusinessType.SelectedIndexChanged += new System.EventHandler(this.cmbBusinessType_SelectedIndexChanged);
            dateTimePicker1.Value = GlobalValue.CollectionDate.Value;

            cmbExistingCustomer.DropDownStyle = ComboBoxStyle.DropDown;
            cmbExistingCustomer.AutoCompleteSource = AutoCompleteSource.ListItems;
            cmbExistingCustomer.AutoCompleteMode = AutoCompleteMode.Suggest;

            //btnAdd.UpdateDefaultButton();


        }

        private void btnAdd_Click(object sender, EventArgs e)
        {

            if (ReturnTypeEnum.None == (ReturnTypeEnum)cmbReturnType.SelectedItem)
            {
                MessageBox.Show("Please Select Return Type");
                cmbReturnType.Focus();
                return;
            }

            if (ReturnTypeEnum.Weekly == (ReturnTypeEnum)cmbReturnType.SelectedItem)
            {
                if (cmbReturnDay.SelectedIndex == 0)
                {
                    MessageBox.Show("Please Select Return Day for weekly type!");
                    cmbReturnDay.Focus();
                    return;
                }
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

                if(string.IsNullOrEmpty(txtTamilName.Text))
                {
                    MessageBox.Show("தமிழ் பெயர் கட்டாயம்!");
                    txtTamilName.Focus();
                    return;
                }
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
                cus.TamilName = txtTamilName.Text;
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

            if (cus.ReturnType == ReturnTypeEnum.TenMonths)
            {
                cus.MonthlyInterest = cus.LoanAmount / 10; ;
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

            var nthTimes = Customer.GetAllCustomer().Where(w => w.CustomerId == cus.CustomerId).Count();

            lblMessage.Text = $"Customer {cus.Name} ({nthTimes}{General.GetDaySuffix(nthTimes)} times) Added Successfully.";
            lblNoteCount.Text = cus.CustomerSeqNumber.ToString();

        }

        private void EnableTxtBoxes(bool isEnable)
        {
            txtName.Enabled = txtPhone.Enabled = txtTamilName.Enabled = isEnable;
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
                EnableTxtBoxes(false);

            }
            else
            {
                EnableTxtBoxes(true);
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
            var allDays = Enum.GetValues(typeof(DayOfWeek)).Cast<DayOfWeek>().ToList();

            allDays.ForEach(f => { cmbReturnDay.Items.Add(f); });

            cmbReturnDay.Items.Insert(0, "--Select--");

            cmbCollectionSpot.DataSource = Customer.GetAllUniqueCustomers();
            cmbCollectionSpot.ValueMember = "CustomerId";
            cmbCollectionSpot.DisplayMember = "Name";


            cmbReturnType.SelectedItem = ReturnTypeEnum.None;
            cmbReturnDay.SelectedItem = DayOfWeek.Sunday;
            cmbCollectionSpot.SelectedValue = 0;
        }

        private void LoadBusinessType()
        {
            cmbBusinessType.DataSource = BusinessType.GetBusinessTypes();
            cmbBusinessType.ValueMember = "Id";
            cmbBusinessType.DisplayMember = "IdAndName";
            cmbBusinessType.SelectedValue = 0;

            cmbBusTypeToAdd.DataSource = BusinessType.GetBusinessTypes();
            cmbBusTypeToAdd.ValueMember = "Id";
            cmbBusTypeToAdd.DisplayMember = "IdAndName";
            cmbBusTypeToAdd.SelectedValue = 0;
        }

        private void cmbExistingCustomer_SelectedIndexChanged(object sender, EventArgs e)
        {
            var selectedCustomer = cmbExistingCustomer.SelectedItem as Customer;

            if (selectedCustomer.GivenEligibility == false)
            {
                MessageBox.Show($"{selectedCustomer.Name} is not eligible for loan. Sorry!!!");
                this.Enabled = false;
            }

            cmbReturnType.SelectedItem = selectedCustomer.ReturnType;

        }

        private void cmbReturnType_SelectedIndexChanged(object sender, EventArgs e)
        {
            ReturnTypeOrDayChanged();
        }

        private void dateTimePicker1_ValueChanged(object sender, EventArgs e)
        {
            ReturnTypeOrDayChanged();
        }


        private void ReturnTypeOrDayChanged()
        {
            if ((ReturnTypeEnum)cmbReturnType.SelectedItem == ReturnTypeEnum.Weekly)
            {
                cmbReturnDay.SelectedItem = dateTimePicker1.Value.DayOfWeek;
            }

            if ((ReturnTypeEnum)cmbReturnType.SelectedItem == ReturnTypeEnum.TenMonths)
            {
                txtInterest.Text = txtLoan.Text.PercentageOfStr(20);
            }

        }

        private void btnBTadd_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtBusType.Text))
            {
                MessageBox.Show("please enter BT.");
                return;
            }

            var result = BusinessType.AddBusinessType(new BusinessType() { Name = txtBusType.Text });
            MessageBox.Show(result);
            txtBusType.SelectAll();
            txtBusType.Focus();
            //LoadBusinessType();

        }

        private void btnBTedit_Click(object sender, EventArgs e)
        {
            if (cmbBusinessType.SelectedItem == null)
            {
                MessageBox.Show("Please select BT before edit");
                return;
            }

            var result = BusinessType.UpdateBusinessType(new BusinessType()
            {
                Id = cmbBusinessType.SelectedValue.ToInt32(),
                Name = txtBusType.Text
            });

            MessageBox.Show(result);
            //LoadBusinessType();
        }

        private void btnBTdelete_Click(object sender, EventArgs e)
        {
            if (cmbBusinessType.SelectedItem == null)
            {
                MessageBox.Show("Please select BT before delete");
                return;
            }


            var bt = (cmbBusinessType.SelectedItem as BusinessType);
            if (DialogResult.Yes == MessageBox.Show($"Are you sure you want to delet Bt - {bt.Name}?", "", MessageBoxButtons.YesNo))
            {
                if (BusinessType.DeleteBusinessType(bt.Id))
                    MessageBox.Show($"BT - {bt.Name} deleted success!");
                else
                    MessageBox.Show("error!");
            }

            //LoadBusinessType();

        }

        private void cmbBusinessType_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (cmbBusinessType.SelectedItem != null)
            {
                txtBusType.Text = (cmbBusinessType.SelectedItem as BusinessType).Name;
            }

        }
    }
}
