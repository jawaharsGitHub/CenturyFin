using Common;
using DataAccess.ExtendedTypes;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.PrimaryTypes
{
    public class Customer : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.CustomerFile;

        public int CustomerSeqNumber { get; set; }
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public string PhoneNumber { get; set; }
        public int LoanAmount { get; set; }
        public int Interest { get; set; }
        public bool IsExistingCustomer { get; set; }
        public bool IsActive { get; set; }
        public DateTime? AmountGivenDate { get; set; }
        public DateTime? ClosedDate { get; set; }
        public DateTime? ModifiedDate { get; set; }


        public static void AddCustomer(Customer newCustomer)
        {
            newCustomer.ModifiedDate = null;
            newCustomer.IsActive = true;
            newCustomer.ClosedDate = null;

            InsertSingleObjectToListJson(JsonFilePath, newCustomer);
        }

        public static void UpdateCustomerDetails(Customer updatedCustomer)
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

                var u = list.Where(c => c.CustomerId == updatedCustomer.CustomerId && c.CustomerSeqNumber == updatedCustomer.CustomerSeqNumber && c.IsActive == true).FirstOrDefault();
                u.IsActive = updatedCustomer.IsActive;
                u.ClosedDate = updatedCustomer.ClosedDate;

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CorrectCustomerData(Customer updatedCustomer)
        {

            try
            {
                if (updatedCustomer.ClosedDate == DateTime.MinValue)
                {
                    updatedCustomer.ClosedDate = null;
                }

                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

                var allCustomers = list.Where(c => c.CustomerId == updatedCustomer.CustomerId).ToList();

                if (allCustomers.FirstOrDefault().Name != updatedCustomer.Name)
                {
                    allCustomers.ForEach(un => un.Name = updatedCustomer.Name);
                }

                var u = list.Where(c => c.CustomerId == updatedCustomer.CustomerId && c.CustomerSeqNumber == updatedCustomer.CustomerSeqNumber).FirstOrDefault();

                u.AmountGivenDate = updatedCustomer.AmountGivenDate; // Done: need to update all fields later
                u.ClosedDate = updatedCustomer.ClosedDate;
                u.Interest = updatedCustomer.Interest;
                u.LoanAmount = updatedCustomer.LoanAmount;
                u.CustomerId = updatedCustomer.CustomerId;

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void UpdateCustomerClosedDate(Customer updatedCustomer)
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

                var u = list.Where(c => c.CustomerId == updatedCustomer.CustomerId && c.CustomerSeqNumber == updatedCustomer.CustomerSeqNumber).FirstOrDefault();
                u.ClosedDate = updatedCustomer.ClosedDate;

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Customer GetCustomerDetails(int cusid, int cusSeqNo)
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);
                return list.Where(c => c.CustomerId == cusid && c.CustomerSeqNumber == cusSeqNo).FirstOrDefault();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Customer> GetAllCustomer()
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteCustomerDetails(int customerId, int sequenceNo)
        {
            try
            {
                List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

                var itemToDelete = list.Where(c => c.CustomerId == customerId && c.CustomerSeqNumber == sequenceNo).FirstOrDefault();
                list.Remove(itemToDelete);

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetNextCustomerId()
        {
            List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

            if (list == null || list.Count == 0) return 1;

            return (list.Max(m => m.CustomerId) + 1);

        }

        public static int GetNextCustomerSeqNo()
        {
            List<Customer> list = ReadFileAsObjects<Customer>(JsonFilePath);

            if (list == null || list.Count == 0) return 1;
            return (list.Select(s => s.CustomerSeqNumber).Max() + 1);
        }

        public static CreditReport GetCreditScore(int _customerId, int _sequeneNo)
        {

            var cus = Customer.GetCustomerDetails(_customerId, _sequeneNo);

            var _isClosedTx = (cus.IsActive == false);

            var txns = Transaction.GetTransactionDetails(cus.CustomerId, cus.CustomerSeqNumber, _isClosedTx);

            if (txns == null || txns.Count == 0) return null;

            var dataDource = txns;


            //if (byBalance)
            //    dataGridView1.DataSource = dataDource.OrderBy(o => o.Balance).ToList();
            //else if (isDesc)
            //    dataGridView1.DataSource = dataDource.OrderByDescending(o => o.TxnDate).ThenBy(t => t.Balance).ToList();
            //else
            //    dataGridView1.DataSource = dataDource.OrderBy(o => o.TxnDate).ToList();

            var lastBalance = txns.Min(m => m.Balance);

            var startDate = dataDource.Select(s => s.TxnDate).Min();
            var lastDate = dataDource.Select(s => s.TxnDate).Max();
            var daysTaken = (lastBalance == 0) ? lastDate.Date.Subtract(startDate).Days + 2 : DateTime.Now.Date.Subtract(startDate).Days + 2;


            //lblStartDate.Text = $"Start Date: {startDate.Date.ToShortDateString()}";
            //lblLastDate.Text = $"Last Date: {lastDate.Date.ToShortDateString()}";

            var expected = (daysTaken * (cus.LoanAmount / 100)) > cus.LoanAmount ? -1 : (daysTaken * (cus.LoanAmount / 100));

            var _balance = _isClosedTx ? 0 : Transaction.GetBalance(cus.LoanAmount, cus.CustomerSeqNumber, cus.CustomerId);
            var actual = cus.LoanAmount - _balance;

            var dayTaken = daysTaken;
            var _expected = expected;
            var _actual = actual;


            //lblNoOfDays.Text = $"Days taken to Return {DaysTaken} (Expected {expected} ACTUAL {actual})";

            //if (expected == -1)
            //    lblNoOfDays.ForeColor = System.Drawing.Color.IndianRed;
            //else if (actual < expected)
            //    lblNoOfDays.ForeColor = System.Drawing.Color.Red;
            //else if (actual > expected)
            //    lblNoOfDays.ForeColor = System.Drawing.Color.Green;
            //else if (actual == expected)
            //    lblNoOfDays.ForeColor = System.Drawing.Color.Honeydew;


            var interestRate = (cus.LoanAmount / cus.Interest) == 12 ? 8.69 : 11.11;


            var percGainPerMonth = Math.Round(((interestRate / daysTaken) * 30), 2); // 8.89% is % of 800 for 9200 for one month.

            //var percentage = percGainPerMonth;
            var interestPerMonth = (percGainPerMonth / 100) * (cus.LoanAmount - cus.Interest);

            //lblPercentageGain.Text = $"{percGainPerMonth.ToString()}% Per Month({(percGainPerMonth / 100) * (cus.LoanAmount - cus.Interest)} Rs/Month)";

            //dateTimePicker1.Value = lastDate.AddDays(1);

            //dataGridView1.Columns["CustomerId"].Visible = false;
            //dataGridView1.Columns["IsClosed"].Visible = false;
            //dataGridView1.Columns["TxnUpdatedDate"].Visible = false;
            //dataGridView1.Columns["CustomerSequenceNo"].Visible = false;

            return new CreditReport
            {
                CustomerId = cus.CustomerId,
                Name = cus.Name,
                InterestRate = interestRate,
                PercGainPerMonth = percGainPerMonth,
                InterestPerMonth = interestPerMonth,
                DaysTaken = daysTaken
            };


        }

    }

}
