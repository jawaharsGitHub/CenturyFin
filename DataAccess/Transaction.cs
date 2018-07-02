using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAccess
{
    public class Transaction
    {


        public int CustomerId { get; set; }
        public bool IsClosed { get; set; }
        //public Customer TxnCustomer;

        public int TransactionId { get; set; }
        public int AmountReceived { get; set; }
        public int Balance { get; set; }
        //public DateTime TxnDate { get; set; }
        public DateTime? TxnUpdatedDate { get; set; }
        public int CustomerSequenceNo { get; set; }
        public DateTime TxnDate { get; set; } //{ get => TxnDate; set => TxnDate = value; }

        

        // Add
        public static string AddObjectsToJson<T>(string json, List<T> objects)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();

            list.AddRange(objects);
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }




        public static void AddTransaction(Transaction newTxn)
        {
            //newTxn.TxnDate = DateTime.Today;
            newTxn.TxnUpdatedDate = null;
            List<Transaction> transactions = new List<Transaction>() { newTxn };

            // Get existing transactions
            string baseJson = File.ReadAllText(AppConfiguration.TransactionFile);

            //Merge the transactions
            string updatedJson = AddObjectsToJson(baseJson, transactions);

            // Add into json
            File.WriteAllText(AppConfiguration.TransactionFile, updatedJson);

        }

        public static void AddDailyTransactions(List<Transaction> newTxn)
        {
            //newTxn.TxnDate = DateTime.Today;
            //newTxn.TxnUpdatedDate = null;
            //List<Transaction> transactions = newTxn;

            newTxn.ForEach(f =>
            {
                //f.Balance = GetBalance(f.TxnCustomer);
            });



            // Get existing transactions
            string baseJson = File.ReadAllText(AppConfiguration.TransactionFile);

            //Merge the transactions
            string updatedJson = AddObjectsToJson(baseJson, newTxn);

            // Add into json
            File.WriteAllText(AppConfiguration.TransactionFile, updatedJson);

        }


        public static void AddTransactions(List<Transaction> newTxns)
        {

            List<Transaction> transactions = newTxns;

            // Get existing transactions
            string baseJson = File.ReadAllText(AppConfiguration.TransactionFile);

            //Merge the transactions
            string updatedJson = AddObjectsToJson(baseJson, transactions);

            // Add into json
            File.WriteAllText(AppConfiguration.TransactionFile, updatedJson);

        }



        public static void AddClosedTransaction(List<Transaction> closedTxn)
        {

            var customer = closedTxn.First();


            string customerBackupFolderPath = Path.Combine(AppConfiguration.BackupFolderPath, customer.CustomerId.ToString());

            if (Directory.Exists(customerBackupFolderPath) == false)
            {
                Directory.CreateDirectory(customerBackupFolderPath);
            }

            string backupFileName = Path.Combine(customerBackupFolderPath, $"{customer.CustomerId.ToString()}_{customer.CustomerSequenceNo.ToString()}.json");

            var content = JsonConvert.SerializeObject(closedTxn, Formatting.Indented);

            // Add into json
            File.WriteAllText(backupFileName, content);

        }



        public static void UpdateTransactionDetails(Transaction updatedTransaction)
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.TransactionFile);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);

                var u = list.Where(c => c.TransactionId == updatedTransaction.TransactionId).FirstOrDefault();

                u.AmountReceived = updatedTransaction.AmountReceived;
                u.TxnUpdatedDate = DateTime.Today;

                string updatedString = JsonConvert.SerializeObject(list, Formatting.Indented);


                File.WriteAllText(AppConfiguration.TransactionFile, updatedString);

            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static List<Transaction> GetTransactionDetails(int customerId, int sequenceNo, bool isClosedTxn)
        {

            try
            {
                var txnFile = isClosedTxn ? $"{AppConfiguration.BackupFolderPath}/{customerId}/{customerId}_{sequenceNo}.json" : AppConfiguration.TransactionFile;

                var json = File.ReadAllText(txnFile);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                if (list == null) return null;
                return list.Where(c => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo).OrderBy(o => o.TransactionId).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public static List<Transaction> GetTransactionCount(int customerId, int sequenceNo)
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.TransactionFile);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                if (list == null) return null;
                return list.Where(c => c.CustomerId == customerId && c.CustomerSequenceNo == c.CustomerSequenceNo).OrderBy(o => o.TransactionId).ToList();
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static void DeleteTransactionDetails(int customerId, int sequenceNo)
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.TransactionFile);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                var itemToDelete = list.Where(c => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo).ToList();
                list.RemoveAll((c) => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo);
                string updatedCustomers = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(AppConfiguration.TransactionFile, updatedCustomers);
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static int GetNextTransactionId()
        {
            var json = File.ReadAllText(AppConfiguration.TransactionFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
            if (list == null || list.Count == 0) return 1;
            return (list.Select(s => s.TransactionId).Max() + 1);

        }

        public static int GetBalance(int loanAmount, int sequenceNo, int customerNo)
        {
            var json = File.ReadAllText(AppConfiguration.TransactionFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
            if (list == null || list.Count == 0) return loanAmount - 0;

            var paidAmount = (list.Where(s => s.CustomerSequenceNo == sequenceNo && s.CustomerId == customerNo).Sum(s => s.AmountReceived));
            return loanAmount - paidAmount;

        }


        public static int GetAllOutstandingAmount()
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.TransactionFile);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                if (list == null)
                {

                    var customersOutstanding = (Customer.GetAllCustomer() ?? new List<Customer>()).Where(w => w.IsActive).Sum(s => s.LoanAmount);
                    return customersOutstanding;
                }

                var groupedList = (from L in list
                                   group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                   select newGroup).ToList();

                int outsideMoney = 0;

                foreach (var item in groupedList)
                {

                    outsideMoney += item.OrderBy(o => o.Balance).First().Balance;

                }


                return outsideMoney;

            }
            catch (Exception ex)
            {

                throw;
            }
        }
        public static int GetClosedTxn()
        {
            var json = File.ReadAllText(AppConfiguration.TransactionFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json) ?? new List<Transaction>();
            return list.Where(w => w.Balance == 0).Count();
        }



    }


}
