using Common;
using Common.ExtensionMethod;
using DataAccess.ExtendedTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAccess.PrimaryTypes
{
    public class Transaction : BaseClass
    {

        private static string ClosedTxnFilePath = AppConfiguration.ClosedNotesFile;
        private static string JsonFilePath = AppConfiguration.TransactionFile;

        public int CustomerId { get; set; }
        public bool IsClosed { get; set; }
        public int TransactionId { get; set; }
        public int AmountReceived { get; set; }
        public int Balance { get; set; }
        public DateTime? TxnUpdatedDate { get; set; }
        public int CustomerSequenceNo { get; set; }
        public DateTime TxnDate { get; set; }

        public int SerialNo { get; set; }
        public int Diff { get; set; }


        public static void AddTransaction(Transaction newTxn)
        {
            newTxn.TxnUpdatedDate = null;

            var jsonFilePath = newTxn.IsClosed ? $"{ClosedTxnFilePath}/{newTxn.CustomerId}/{newTxn.CustomerId}_{newTxn.CustomerSequenceNo}.json" : JsonFilePath;

            InsertSingleObjectToListJson(jsonFilePath, newTxn);
        }

        public static void AddDailyTransactions(Transaction txn)
        {
            InsertSingleObjectToListJson(JsonFilePath, txn);
        }

        public static void AddTransactions(List<Transaction> newTxns)
        {
            InsertObjectsToJson(JsonFilePath, newTxns);
        }

        public static void AddBatchTransactions(List<Transaction> newTxns, string fileName)
        {
            var fullFilePath = Path.Combine(AppConfiguration.DailyBatchFile, fileName);
            if (File.Exists(fullFilePath)) File.Delete(fullFilePath);

            WriteObjectsToFile(newTxns, fullFilePath);
        }


        public static void AddClosedTransaction(List<Transaction> closedTxn)
        {
            var customer = closedTxn.First();

            string customerBackupFolderPath = Path.Combine(ClosedTxnFilePath, customer.CustomerId.ToString());

            if (Directory.Exists(customerBackupFolderPath) == false)
            {
                Directory.CreateDirectory(customerBackupFolderPath);
            }

            string backupFileName = Path.Combine(customerBackupFolderPath, $"{customer.CustomerId.ToString()}_{customer.CustomerSequenceNo.ToString()}.json");

            WriteObjectsToFile(closedTxn, backupFileName);
        }

        public static void UpdateTransactionDetails(Transaction updatedTransaction)
        {
            try
            {

                var jsonFilePath = updatedTransaction.IsClosed ? $"{ClosedTxnFilePath}/{updatedTransaction.CustomerId}/{updatedTransaction.CustomerId}_{updatedTransaction.CustomerSequenceNo}.json" : JsonFilePath;

                List<Transaction> list = ReadFileAsObjects<Transaction>(jsonFilePath);

                var u = list.Where(c => c.TransactionId == updatedTransaction.TransactionId && c.CustomerSequenceNo == updatedTransaction.CustomerSequenceNo && c.CustomerId == updatedTransaction.CustomerId).FirstOrDefault();

                u.Balance = ((u.Balance + u.AmountReceived) - updatedTransaction.AmountReceived); // Very important calculation when corrected collection amount more than 1 time for he same date.
                u.AmountReceived = updatedTransaction.AmountReceived;
                u.TxnUpdatedDate = DateTime.Today;

                WriteObjectsToFile(list, AppConfiguration.TransactionFile);

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

                List<Transaction> list = ReadFileAsObjects<Transaction>(filePath);

                var u = list.Where(c => c.TransactionId == updatedTransaction.TransactionId).FirstOrDefault();

                u.AmountReceived = updatedTransaction.AmountReceived;
                u.TxnDate = updatedTransaction.TxnDate;
                u.Balance = updatedTransaction.Balance;

                WriteObjectsToFile(list, filePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void MergeTransactionLoanAmount(Transaction updatedTransaction)
        {

            try
            {
                var filePath = updatedTransaction.IsClosed ? $"{AppConfiguration.ClosedNotesFile}/{updatedTransaction.CustomerId}/{updatedTransaction.CustomerId}_{updatedTransaction.CustomerSequenceNo}.json" : AppConfiguration.TransactionFile;

                List<Transaction> list = ReadFileAsObjects<Transaction>(filePath);

                var u = list.Where(c => c.TransactionId == updatedTransaction.TransactionId).FirstOrDefault();

                //u.AmountReceived = updatedTransaction.AmountReceived;
                u.TxnDate = updatedTransaction.TxnDate;
                u.Balance = updatedTransaction.Balance;

                WriteObjectsToFile(list, filePath);
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
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);
                if (list == null) return null;
                return list.Where(c => c.TransactionId == txnId).First();
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Transaction> GetTransactionDetails(Customer customer)
        {
            try
            {
                var txnFile = (customer.IsActive == false) ? $"{ClosedTxnFilePath}/{customer.CustomerId}/{customer.CustomerId}_{customer.CustomerSeqNumber}.json" : JsonFilePath;

                if (File.Exists(txnFile))
                {
                    var list = ReadFileAsObjects<Transaction>(txnFile);
                    if (list == null) return null;
                    return list.Where(c => c.CustomerId == customer.CustomerId && c.CustomerSequenceNo == customer.CustomerSeqNumber).OrderBy(o => o.TxnDate.Date).ThenByDescending(t => t.Balance).ToList();
                }

                return null;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Transaction> GetActiveCustomersLastTransactionDetails()
        {
            try
            {

                var list = ReadFileAsObjects<Transaction>(JsonFilePath);
                if (list == null) return null;

                var txns = (from L in list
                            group L by new { L.CustomerSequenceNo, L.CustomerId } into newGroup
                            select newGroup.OrderByDescending(o => o.TxnDate).First()).ToList();

                return txns;

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static List<Transaction> GetTransactionForDate(Transaction txn)
        {
            try
            {
                //TODO: need to do it for closed txn also?
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);
                if (list == null) return null;

                return list.Where(c => c.CustomerId == txn.CustomerId && c.CustomerSequenceNo == txn.CustomerSequenceNo && c.TxnDate.Date == txn.TxnDate.Date).OrderByDescending(o => o.TransactionId).ToList();
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
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);

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
                var result = new List<Transaction>();

                foreach (var folder in Directory.GetDirectories(ClosedTxnFilePath))
                {
                    foreach (var file in Directory.GetFiles(folder))
                    {
                        var list = ReadFileAsObjects<Transaction>(file);
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
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);
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
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);

                //var itemToDelete = list.Where(c => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo).ToList();
                list.RemoveAll((c) => c.CustomerId == customerId && c.CustomerSequenceNo == sequenceNo);

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static void DeleteTransactionDetails(Transaction txn)
        {
            try
            {
                var filePath = txn.IsClosed ? $"{ClosedTxnFilePath}/{txn.CustomerId}/{txn.CustomerId}_{txn.CustomerSequenceNo}.json" : JsonFilePath;
                var list = ReadFileAsObjects<Transaction>(filePath);

                var itemToDelete = list.FirstOrDefault(c => c.CustomerId == txn.CustomerId && c.CustomerSequenceNo == txn.CustomerSequenceNo && c.TransactionId == txn.TransactionId);
                list.Remove(itemToDelete);

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetNextTransactionId()
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            if (list == null || list.Count == 0) return 1;
            return (list.Select(s => s.TransactionId).Max() + 1);

        }


        public static string GetLastTransactionDate(Customer customer)
        {

            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            var latestDate = (from txn in list
                              where txn.CustomerSequenceNo == customer.CustomerSeqNumber && txn.AmountReceived >= 0
                              orderby txn.TransactionId descending
                              select txn).First().TxnDate.ToShortDateString();

            return latestDate;

        }

        public static string GetTransactionSummaryForWeek(Customer customer)
        {

            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            var latestDate = (from txn in list
                              where txn.CustomerSequenceNo == customer.CustomerSeqNumber
                              orderby txn.TransactionId
                              select $"{txn.TxnDate.Day}({txn.TxnDate.DayOfWeek.ToString().Substring(0, 2)})").ToList();

            var result = String.Join("-", latestDate);

            return result;

        }

        public static int GetBalance(Customer customer)
        {
            if (customer.IsActive == false) return 0;


            var list = ReadFileAsObjects<Transaction>(JsonFilePath);
            if (list == null || list.Count == 0) return customer.LoanAmount - 0;

            var customerTxns = list.Where(s => s.CustomerSequenceNo == customer.CustomerSeqNumber && s.CustomerId == customer.CustomerId);

            if (customer.ReturnType == ReturnTypeEnum.Monthly)
            {
                return customerTxns.OrderByDescending(m => m.TxnDate).First().Balance;
            }

            var paidAmount = customerTxns.Sum(s => s.AmountReceived);

            var txnLoanAmount = customerTxns.First(f => f.AmountReceived == 0).Balance;

            // Scenario: 1. Balance in customer and txn differs
            // Eg: cus1 got 40k and cus2 got 30k, now cus1 balance is 35k and cus2 balance is 20k, now cus1 wants to take responsibility of cus1 balance.
            // so, now cus balance is 35K + 20K = 55k but still balance in txns is 40k but in customer it is 55k. cus2 will be deleted.
            //if (customer.LoanAmount != txnLoanAmount)
            //{
            //    return txnLoanAmount - paidAmount;
            //}

            return customerTxns.OrderByDescending(m => m.TxnDate).ThenByDescending(t => t.TransactionId).First().Balance;   //customerTxns.Min(m => m.Balance); // Both seems to be same result. - for womething it shows worng eg: some tool tip balance.
                                                                                                                            // return (customer.LoanAmount - paidAmount);

        }

        public static string GetBalanceAndLastDate(Customer customer)
        {
            //if (customer.IsActive == false) return (0, null);

            Transaction result;

            var list = ReadFileAsObjects<Transaction>(JsonFilePath);
            //if (list == null || list.Count == 0) return $"{customer.LoanAmount - 0} on {customer.AmountGivenDate);

            var customerTxns = list.Where(s => s.CustomerSequenceNo == customer.CustomerSeqNumber && s.CustomerId == customer.CustomerId);

            if (customer.ReturnType == ReturnTypeEnum.Monthly)
            {
                result = customerTxns.OrderByDescending(m => m.TxnDate).First();
            }
            else
            {
                var paidAmount = customerTxns.Sum(s => s.AmountReceived);
                //var txnLoanAmount = customerTxns.First(f => f.AmountReceived == 0).Balance;
                result = customerTxns.OrderByDescending(m => m.TxnDate).ThenByDescending(t => t.TransactionId).First();
            }                                                                                                            // return (customer.LoanAmount - paidAmount);

            return $"{result.Balance} on  {result.TxnDate.ToShortDateString()}[{(DateTime.Today - result.TxnDate).Days}]";
        }

        public static List<Transaction> GetActiveTransactions()
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            var result = new List<Transaction>();

            var outsideMoney = (from L in list
                                group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                select newGroup).ToList();

            outsideMoney.ForEach(fe =>
            {
                result.Add(fe.OrderBy(o => o.Balance).First());
            });

            return result;


        }

        public static List<DynamicReportClosedSoon> GetTransactionsToBeClosedSoon(int top)
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            var result = new List<Transaction>();

            var outsideMoney = (from L in list
                                group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                select newGroup).ToList();

            var customers = Customer.GetAllCustomer().Where(w => w.IsActive).ToList();


            outsideMoney.ForEach(fe =>
            {
                result.Add(fe.OrderByDescending(o => o.TxnDate.Date).ThenByDescending(t => t.TransactionId).First());
            });

            var data = (from c in customers
                        join t in result on
                        new { c.CustomerSeqNumber, c.CustomerId } equals new { CustomerSeqNumber = t.CustomerSequenceNo, t.CustomerId }

                        select new DynamicReportClosedSoon()
                        {
                            RunningDays = (DateTime.Now - c.AmountGivenDate).Value.Days,
                            Name = c.Name,
                            LoanAmount = c.LoanAmount,
                            Balance = t.Balance,
                            BalancePerc = (Convert.ToDecimal(t.Balance) / Convert.ToDecimal(c.LoanAmount)) * 100,
                            CreditScore = Customer.GetCreditScore(c.CustomerId),
                            NeedToClose = ((DateTime.Now - c.AmountGivenDate.Value).TotalDays) > 100 ? 0 : ((c.AmountGivenDate.Value.AddDays(100) - DateTime.Today.Date).Days),
                            DaysToClose = ((DateTime.Now - c.AmountGivenDate.Value).TotalDays) > 100 ? (100 - (DateTime.Now - c.AmountGivenDate.Value.Date).Days) : (t.Balance / (c.LoanAmount / 100)),
                            AmountGivenDate = c.AmountGivenDate,
                            CustomerSeqNumber = c.CustomerSeqNumber,
                            Interest = c.Interest
                        }).OrderBy(o => o.BalancePerc).ThenBy(t => t.Balance).Take(top).ToList();

            return data;


        }

        public static List<DynamicReportNotGivenDays> GetTransactionsNotGivenForFewDays()
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

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
                        select new DynamicReportNotGivenDays()
                        {
                            Name = c.Name,
                            LoanAmount = c.LoanAmount,
                            Balance = t.Balance,
                            CreditScore = Customer.GetCreditScore(c.CustomerId),
                            NotGivenFor = (DateTime.Now.Date - t.TxnDate.Date).TotalDays,
                            LastTxnDate = t.TxnDate,
                            AmountGivenDate = c.AmountGivenDate,
                            CustomerSeqNumber = c.CustomerSeqNumber,
                            ReturnType = c.ReturnType,
                            Interest = c.Interest,
                            NeedInvestigation = c.NeedInvestigation,
                            MonthlyInterest = c.MonthlyInterest,
                            CustomerId = c.CustomerId
                        }).Where(w => w.NotGivenFor > 2).OrderByDescending(o => o.NotGivenFor).ToList();

            return data;
        }


        public static (int actual, int includesProfit) GetAllOutstandingAmount()
        {
            try
            {
                var list = ReadFileAsObjects<Transaction>(JsonFilePath);

                if (list == null)
                {
                    var customersOutstanding = (Customer.GetAllCustomer() ?? new List<Customer>()).Where(w => w.IsActive);
                    return (customersOutstanding.Sum(s => s.LoanAmount - s.Interest), customersOutstanding.Sum(s => s.LoanAmount));
                }

                // This needs to validate.
                var outsideMoney = (from L in list

                                    group L by new { L.CustomerId, L.CustomerSequenceNo } into newGroup
                                    select newGroup.ToList().OrderByDescending(w => w.TxnDate).First()).ToList(); //.Sum(s => s.Balance);

                // this is useful to calculate all external balances
                var result = (from c in Customer.GetAllCustomer()
                              join t in outsideMoney on new { CustomerSeqNumber = c.CustomerSeqNumber, c.CustomerId } equals new
                              {
                                  CustomerSeqNumber = t.CustomerSequenceNo,
                                  t.CustomerId
                              }
                              orderby c.AmountGivenDate
                              select new
                              {
                                  c.Name,
                                  c.AmountGivenDate,
                                  c.CustomerSeqNumber,
                                  c.CustomerId,
                                  t.Balance,
                                  c.LoanAmount,
                                  c.Interest,
                                  t.TransactionId
                              }).ToList();


                return (result.Sum(s => s.Balance - s.Interest), result.Sum(s => s.Balance));

            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static int GetClosedTxn()
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            return list.Where(w => w.Balance == 0).Count();
        }

        public static dynamic GetGivenTxnForMonth(string txnMonthAndYear)
        {
            //var list = ReadFileAsObjects<Transaction>(JsonFilePath);

            //var result = from t in txn
            //             join c in cus
            //             on t.CustomerSequenceNo equals c.CustomerSeqNumber
            //             select new
            //             {

            //             };

            //return list.Where(w => w.TxnDate.ToString("Y") == txnMonthAndYear).ToList();

            return null;
        }

        public static List<Transaction> GetDailyCollectionDetails(DateTime inputDate)
        {

            // Get from Ongoing Transcations
            var txnFile = Path.Combine(AppConfiguration.DailyBatchFile, inputDate.ToString("dd-MM-yyyy")); // TODO: Ned to fix this as data is not showing till run daily txn batch // new approach
            if (File.Exists(txnFile) == false) return new List<Transaction>();

            var list = ReadFileAsObjects<Transaction>(txnFile);

            if (list == null) return null;

            var fromActiveTxn = list.Where(c => (c.TxnDate.Date == inputDate.Date)).ToList(); // && c.AmountReceived > 0

            return fromActiveTxn;
        }

        public static List<Transaction> GetDailyCollectionDetails_V0(DateTime inputDate)
        {
            var list = ReadFileAsObjects<Transaction>(JsonFilePath);
            if (list == null) return null;
            var fromActiveTxn = list.Where(c => c.TxnDate.Date == inputDate.Date).OrderBy(o => o.TransactionId).ToList(); //  && c.AmountReceived > 0

            //Get from Closed Transactions
            var fromClosedTxn = ProcessDirectory(ClosedTxnFilePath, inputDate);

            fromActiveTxn.AddRange(fromClosedTxn);

            return fromActiveTxn;
        }

        public static int GetDailyCollectionAmount(DateTime inputDate)
        {
            return Transaction.GetDailyCollectionDetails_V0(inputDate).Where(w => w.AmountReceived > 0).Sum(s => s.AmountReceived);
        }

        private static List<Transaction> ProcessDirectory(string targetDirectory, DateTime inputDate)
        {
            List<Transaction> result = new List<Transaction>();
            // Process the list of files found in the directory.
            string[] fileEntries = Directory.GetFiles(targetDirectory);
            foreach (string fileName in fileEntries)
            {
                var list = ReadFileAsObjects<Transaction>(fileName);
                // if (list == null) return null;
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
