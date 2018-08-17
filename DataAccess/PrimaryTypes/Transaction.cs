using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;

namespace DataAccess.PrimaryTypes
{
    public class Transaction
    {

        public int CustomerId { get; set; }

        public bool IsClosed { get; set; }

        public int TransactionId { get; set; }

        public int AmountReceived { get; set; }

        public int Balance { get; set; }

        public DateTime? TxnUpdatedDate { get; set; }

        public int CustomerSequenceNo { get; set; }

        public DateTime TxnDate { get; set; }



        private static string AddObjectsToJson<T>(string json, List<T> objects)
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

            var jsonFilePath = newTxn.IsClosed ? $"{AppConfiguration.ClosedNotesFile}/{newTxn.CustomerId}/{newTxn.CustomerId}_{newTxn.CustomerSequenceNo}.json" : AppConfiguration.TransactionFile;

            // Get existing transactions
            string baseJson = File.ReadAllText(jsonFilePath);

            //Merge the transactions
            string updatedJson = AddObjectsToJson(baseJson, transactions);

            // Add into json
            File.WriteAllText(jsonFilePath, updatedJson);

        }

        public static void AddDailyTransactions(Transaction dailyTxn)
        {

            var txn = new List<Transaction>() { dailyTxn };

            // Get existing transactions
            string baseJson = File.ReadAllText(AppConfiguration.TransactionFile);

            //Merge the transactions
            string updatedJson = AddObjectsToJson(baseJson, txn);

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

        public static void AddBatchTransactions(List<Transaction> newTxns, string fileName)
        {
            var fullFilePath = Path.Combine(AppConfiguration.DailyBatchFile, fileName);
            List<Transaction> transactions = newTxns;

            var jsonText = JsonConvert.SerializeObject(newTxns, Formatting.Indented);

            if (File.Exists(fullFilePath)) File.Delete(fullFilePath);

            // Add into json
            File.WriteAllText(fullFilePath, jsonText);

        }


        //TODO: its not yet started using.
        public static bool DeleteTransactionDetails(int customerId, int sequenceNo, bool isActive)
        {

            try
            {
                var txnFile = isActive ? AppConfiguration.TransactionFile : $"{AppConfiguration.ClosedNotesFile}/{customerId}/{customerId}_{sequenceNo}.json";
                //if (isClosedTxn) File.Delete(txnFile);
                if (isActive)
                {
                    var json = File.ReadAllText(txnFile);
                    List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                    //if (list == null) return null;

                    list.RemoveAll(c => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo);
                    return true;
                }

                return false;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void AddClosedTransaction(List<Transaction> closedTxn)
        {

            var customer = closedTxn.First();


            string customerBackupFolderPath = Path.Combine(AppConfiguration.ClosedNotesFile, customer.CustomerId.ToString());

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

                var u = list.Where(c => c.TransactionId == updatedTransaction.TransactionId && c.CustomerSequenceNo == updatedTransaction.CustomerSequenceNo && c.CustomerId == updatedTransaction.CustomerId).FirstOrDefault();

                u.AmountReceived = updatedTransaction.AmountReceived;
                u.TxnUpdatedDate = DateTime.Today;

                string updatedString = JsonConvert.SerializeObject(list, Formatting.Indented);


                File.WriteAllText(AppConfiguration.TransactionFile, updatedString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void CorrectTransactionData(Transaction updatedTransaction)
        {

            try
            {
                var filePath = updatedTransaction.IsClosed ? $"{AppConfiguration.ClosedNotesFile}/{updatedTransaction.CustomerId}/{updatedTransaction.CustomerId}_{updatedTransaction.CustomerSequenceNo}.json" : AppConfiguration.TransactionFile;

                var json = File.ReadAllText(filePath);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);

                var u = list.Where(c => c.TransactionId == updatedTransaction.TransactionId).FirstOrDefault();

                u.AmountReceived = updatedTransaction.AmountReceived;
                u.TxnDate = updatedTransaction.TxnDate;
                u.Balance = updatedTransaction.Balance;

                string updatedString = JsonConvert.SerializeObject(list, Formatting.Indented);


                File.WriteAllText(filePath, updatedString);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static Transaction GetTransactionDetail(int txnId)
        {

            try
            {
                var txnFile = AppConfiguration.TransactionFile;

                var json = File.ReadAllText(txnFile);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                if (list == null) return null;
                return list.Where(c => c.TransactionId == txnId).First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Transaction> GetTransactionDetails(int customerId, int sequenceNo, bool isClosedTxn)
        {

            try
            {
                var txnFile = isClosedTxn ? $"{AppConfiguration.ClosedNotesFile}/{customerId}/{customerId}_{sequenceNo}.json" : AppConfiguration.TransactionFile;

                if (File.Exists(txnFile))
                {
                    var json = File.ReadAllText(txnFile);
                    List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                    if (list == null) return null;
                    return list.Where(c => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo).OrderBy(o => o.TransactionId).ToList();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Transaction> GetTransactionForDate(int customerId, int sequenceNo, DateTime txnDate)
        {

            try
            {
                //TODO: need to do it for closed txn also?
                var txnFile = AppConfiguration.TransactionFile;

                var json = File.ReadAllText(txnFile);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                if (list == null) return null;

                return list.Where(c => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo && c.TxnDate.Date == txnDate.Date).OrderByDescending(o => o.TransactionId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Transaction> GetTransactionForDate(DateTime txnDate)
        {

            try
            {
                //TODO: need to do it for closed txn also?
                var txnFile = AppConfiguration.TransactionFile;

                var json = File.ReadAllText(txnFile);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                if (list == null) return null;

                return list.Where(c => c.TxnDate.Date == txnDate.Date).OrderByDescending(o => o.TransactionId).ToList();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        public static List<Transaction> GetClosedTransactionForDate(DateTime txnDate)
        {

            try
            {
                //TODO: need to do it for closed txn also?
                var txnFile = AppConfiguration.ClosedNotesFile;


                //var sb = new StringBuilder();
                var result = new List<Transaction>();

                foreach (var folder in Directory.GetDirectories(txnFile))
                {
                    foreach (var file in Directory.GetFiles(folder))
                    {
                        //sb.AppendLine($"{folder}/{file}");

                        //Transaction
                        var json = File.ReadAllText(file);
                        List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                        //if (list == null) return null;

                        var data = list.Where(w => w.TxnDate.Date == txnDate.Date).OrderByDescending(o => o.TransactionId).ToList();

                        result.AddRange(data);

                    }

                }

                return result;


            }
            catch (Exception ex)
            {
                throw ex;
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
                throw ex;
            }
        }

        public static void DeleteTransactionDetails(int customerId, int sequenceNo)
        {

            try
            {
                //TODO: Need to implement delete txn for closed txns also.
                var json = File.ReadAllText(AppConfiguration.TransactionFile);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                var itemToDelete = list.Where(c => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo).ToList();
                list.RemoveAll((c) => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo);
                string updatedCustomers = JsonConvert.SerializeObject(list, Formatting.Indented);
                File.WriteAllText(AppConfiguration.TransactionFile, updatedCustomers);
            }
            catch (Exception ex)
            {
                throw ex;
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

        public static List<Transaction> GetActiveTransactions()
        {
            var json = File.ReadAllText(AppConfiguration.TransactionFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);

            var result = new List<Transaction>();

            var outsideMoney = (from L in list
                                group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                //from g in newGroup.ToList()
                                select newGroup).ToList();

            outsideMoney.ForEach(fe =>
            {
                result.Add(fe.OrderBy(o => o.Balance).First());
            });

            return result;


        }

        public static dynamic GetTransactionsToBeClosedSoon(int top)
        {
            var json = File.ReadAllText(AppConfiguration.TransactionFile);

            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);

            var result = new List<Transaction>();

            var outsideMoney = (from L in list
                                group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                select newGroup).ToList();

            var customers = Customer.GetAllCustomer().Where(w => w.IsActive).ToList();


            outsideMoney.ForEach(fe =>
            {
                result.Add(fe.OrderBy(o => o.Balance).First());
            });

            var data = (from c in customers
                        join t in result on
                        new { CustomerSeqNumber = c.CustomerSeqNumber, c.CustomerId } equals new { CustomerSeqNumber = t.CustomerSequenceNo, t.CustomerId }

                        select new
                        {
                            c.Name,
                            NeedToClose = ((DateTime.Now - c.AmountGivenDate.Value).TotalDays) > 100 ? 0 : ((c.AmountGivenDate.Value.AddDays(100) - DateTime.Today.Date).Days),
                            RunningDays = (DateTime.Now - c.AmountGivenDate).Value.Days,
                            DaysToClose = ((DateTime.Now - c.AmountGivenDate.Value).TotalDays) > 100 ? (100 - (DateTime.Now - c.AmountGivenDate.Value.Date).Days) : (t.Balance / (c.LoanAmount / 100)),
                            t.Balance,
                            c.LoanAmount,
                            c.AmountGivenDate,
                            c.CustomerSeqNumber,
                        }).OrderBy(o => o.NeedToClose).Take(top).ToList();

            return data;


        }

        public static dynamic GetTransactionsNotGivenForFewDays()
        {
            var json = File.ReadAllText(AppConfiguration.TransactionFile);

            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);

            var result = new List<Transaction>();

            var outsideMoney = (from L in list
                                group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                select newGroup).ToList();

            var customers = Customer.GetAllCustomer().Where(w => w.IsActive).ToList();


            outsideMoney.ForEach(fe =>
            {
                result.Add(fe.OrderBy(o => o.TxnDate).Last());
            });

            var data = (from c in customers
                        join t in result on c.CustomerSeqNumber equals t.CustomerSequenceNo
                        select new
                        {
                            c.CustomerSeqNumber,
                            c.Name,
                            t.TxnDate,
                            c.AmountGivenDate,
                            c.LoanAmount,
                            t.Balance,
                            NotGivenFor = (DateTime.Now.Date - t.TxnDate.Date).TotalDays
                            //RunningDays = (DateTime.Now - c.AmountGivenDate).Value.Days,
                            //DaysToClose = ((DateTime.Now - c.AmountGivenDate.Value).TotalDays > 100 ? -1 : (t.Balance / (c.LoanAmount / 100))),

                        }).Where(w => w.NotGivenFor > 2).OrderByDescending(o => o.NotGivenFor).ToList();

            return data;


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

                //var groupedList = (from L in list
                //                   group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                //                   select newGroup).ToList();

                //int outsideMoney = 0;

                //foreach (var item in groupedList)
                //{
                //    outsideMoney += item.OrderBy(o => o.Balance).First().Balance;
                //}

                // This needs to validate.
                var outsideMoney = (from L in list

                                    group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                    //from g in newGroup.ToList()
                                    select newGroup.ToList().OrderBy(w => w.Balance).First()).ToList(); //.Sum(s => s.Balance);

                // this is useful to calculate all external balances
                var result = (from c in Customer.GetAllCustomer()
                              join t in outsideMoney on new { CustomerSeqNumber = c.CustomerSeqNumber, c.CustomerId } equals new
                              {
                                  CustomerSeqNumber = t.CustomerSequenceNo,
                                  t.CustomerId
                              }
                              orderby c.AmountGivenDate
                              select new { c.Name, c.AmountGivenDate, c.CustomerSeqNumber, c.CustomerId, t.Balance, c.LoanAmount, t.TransactionId }).ToList();


                return result.Sum(s => s.Balance);

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
        public static int GetClosedTxn()
        {
            var json = File.ReadAllText(AppConfiguration.TransactionFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json) ?? new List<Transaction>();
            return list.Where(w => w.Balance == 0).Count();
        }

        public static List<Transaction> GetDailyCollectionDetails(DateTime inputDate)
        {

            // Get from Ongoing Transcations

            var txnFile = AppConfiguration.TransactionFile;  // old approach.

            //var txnFile = Path.Combine(AppConfiguration.DailyBatchFile, inputDate.ToString("dd-MM-yyyy")); // new approach
            if (File.Exists(txnFile) == false) return new List<Transaction>();
            var json = File.ReadAllText(txnFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
            if (list == null) return null;
            var fromActiveTxn = list.Where(c => c.TxnDate.Date == inputDate.Date).ToList();

            //Get from Closed Transactions
            var fromClosedTxn = ProcessDirectory(AppConfiguration.ClosedNotesFile, inputDate); // old approach.

            fromActiveTxn.AddRange(fromClosedTxn); // old approach.

            return fromActiveTxn;


        }

        public static List<Transaction> GetDailyCollectionDetails_V0(DateTime inputDate)
        {

            //Get from Ongoing Transcations

            var txnFile = AppConfiguration.TransactionFile;

            var json = File.ReadAllText(txnFile);
            List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
            if (list == null) return null;
            var fromActiveTxn = list.Where(c => c.TxnDate.Date == inputDate.Date).ToList();

            //Get from Closed Transactions
            var fromClosedTxn = ProcessDirectory(AppConfiguration.ClosedNotesFile, inputDate);

            fromActiveTxn.AddRange(fromClosedTxn);

            return fromActiveTxn;


        }

        private static List<Transaction> ProcessDirectory(string targetDirectory, DateTime inputDate)
        {
            List<Transaction> result = new List<Transaction>();
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                var json = File.ReadAllText(fileName);
                List<Transaction> list = JsonConvert.DeserializeObject<List<Transaction>>(json);
                if (list == null) return null;
                var data = list.Where(c => c.TxnDate.Date == inputDate.Date);
                if (data != null && data.Count() > 0)
                    result.AddRange(data);
            }


            // Recurse into subdirectories of this directory.
            string[] subdirectoryEntries = Directory.GetDirectories(targetDirectory);
            foreach (string subdirectory in subdirectoryEntries)
                result.AddRange(ProcessDirectory(subdirectory, inputDate));

            return result;
        }

    }

}
