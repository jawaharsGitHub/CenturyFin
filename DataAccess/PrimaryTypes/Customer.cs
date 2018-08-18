using Common;
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

    }

}
