using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAccess
{
    public class Customer
    {


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



        // Add
        public static string AddObjectsToJson<T>(string json, List<T> objects)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();

            list.AddRange(objects);
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }




        public static void AddCustomer(Customer newCustomer)
        {
            //newCustomer.AmountGivenDate = DateTime.Today;
            newCustomer.ModifiedDate = null;
            newCustomer.IsActive = true;
            newCustomer.ClosedDate = null;
            List<Customer> customers = new List<Customer>() { newCustomer };

            // Get existing customers
            string baseJson = File.ReadAllText(AppConfiguration.CustomerFile);

            //Merge the customer
            string updatedJson = AddObjectsToJson(baseJson, customers);

            // Add into json
            File.WriteAllText(AppConfiguration.CustomerFile, updatedJson);

        }


        public static void UpdateCustomerDetails(Customer updatedCustomer)
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.CustomerFile);
                List<Customer> list = JsonConvert.DeserializeObject<List<Customer>>(json);

                var u = list.Where(c => c.CustomerId == updatedCustomer.CustomerId && c.CustomerSeqNumber == updatedCustomer.CustomerSeqNumber && c.IsActive == true).FirstOrDefault();
                u.IsActive = updatedCustomer.IsActive;
                u.ClosedDate = updatedCustomer.ClosedDate;

                string updatedCustomers = JsonConvert.SerializeObject(list, Formatting.Indented);


                File.WriteAllText(AppConfiguration.CustomerFile, updatedCustomers);

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
                var json = File.ReadAllText(AppConfiguration.CustomerFile);
                List<Customer> list = JsonConvert.DeserializeObject<List<Customer>>(json);

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

                string updatedCustomers = JsonConvert.SerializeObject(list, Formatting.Indented);


                File.WriteAllText(AppConfiguration.CustomerFile, updatedCustomers);

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
                var json = File.ReadAllText(AppConfiguration.CustomerFile);
                List<Customer> list = JsonConvert.DeserializeObject<List<Customer>>(json);

                var u = list.Where(c => c.CustomerId == updatedCustomer.CustomerId && c.CustomerSeqNumber == updatedCustomer.CustomerSeqNumber).FirstOrDefault();
                u.ClosedDate = updatedCustomer.ClosedDate;
               
                string updatedCustomers = JsonConvert.SerializeObject(list, Formatting.Indented);

                File.WriteAllText(AppConfiguration.CustomerFile, updatedCustomers);

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
                var json = File.ReadAllText(AppConfiguration.CustomerFile);
                List<Customer> list = JsonConvert.DeserializeObject<List<Customer>>(json);
                return list.Where(c => c.CustomerId == cusid && c.CustomerSeqNumber == cusSeqNo).FirstOrDefault();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static List<Customer> GetAllCustomer()
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.CustomerFile);
                List<Customer> list = JsonConvert.DeserializeObject<List<Customer>>(json);
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static void DeleteCustomerDetails(int customerId, int sequenceNo)
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.CustomerFile);
                List<Customer> list = JsonConvert.DeserializeObject<List<Customer>>(json);
                var itemToDelete = list.Where(c => c.CustomerId == customerId && c.CustomerSeqNumber == sequenceNo).FirstOrDefault();
                list.Remove(itemToDelete);
                string updatedCustomers = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(AppConfiguration.CustomerFile, updatedCustomers);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static int GetNextCustomerId()
        {
            var json = File.ReadAllText(AppConfiguration.CustomerFile);
            List<Customer> list = JsonConvert.DeserializeObject<List<Customer>>(json);
            if (list == null || list.Count == 0) return 1;
            return (list.Select(s => s.CustomerId).Max() + 1);

        }

        public static int GetNextCustomerSeqNo()
        {
            var json = File.ReadAllText(AppConfiguration.CustomerFile);
            List<Customer> list = JsonConvert.DeserializeObject<List<Customer>>(json);
            if (list == null || list.Count == 0) return 1;
            return (list.Select(s => s.CustomerSeqNumber).Max() + 1);

        }

    }


}
