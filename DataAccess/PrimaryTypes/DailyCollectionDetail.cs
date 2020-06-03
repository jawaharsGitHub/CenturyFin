using Common;
using System;
using System.Collections.Generic;
using System.Linq;

namespace DataAccess.PrimaryTypes
{
    public class DailyCollectionDetail : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.DailyTxnFile;

        public string Date { get; set; }
        public int? YesterdayAmountInHand { get; set; }
        public decimal? SentFromUSA { get; set; }
        public decimal? InBank { get; set; }
        public decimal? BankTxnOut { get; set; }
        public int? TakenFromBank { get; set; }
        public int? CollectionAmount { get; set; }
        public int? GivenAmount { get; set; }
        public int? Interest { get; set; }
        public int? TodayInHand { get; set; }
        public int? ClosedAccounts { get; set; }
        public int? OpenedAccounts { get; set; }
        public int? TomorrowNeed { get; set; }
        public int? TomorrowDiff { get; set; }
        public string Comments { get; set; }
        public int OtherExpenditire { get; set; }
        public int OtherInvestment { get; set; }
        public int OutUsedMoney { get; set; }
        public int ActualMoneyInBusiness { get; set; }

        // Daily Calculation details.
        public int? InputMoney { get; set; }
        public int? OutGoingMoney { get; set; }
        public int? Difference { get; set; }
        public int? ExpectedInHand { get; set; }
        public int? ActualInHand { get; set; }
        public int? MamaExpenditure { get; set; }
        public int? MamaInputMoney { get; set; }
        public int? MamaAccount { get; set; }




        public static void AddOrUpdateDaily(DailyCollectionDetail dailyCol)
        {
            var list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);
            if (list != null && list.Count > 0)
            {
                var data = list.Where(w => w.Date == dailyCol.Date).ToList().FirstOrDefault();

                if (data != null) // Update
                {
                    data.Comments = dailyCol.Comments;
                    WriteObjectsToFile(list, JsonFilePath);
                }
                else // New (Insert)
                {
                    InsertSingleObjectToListJson(JsonFilePath, dailyCol);
                }
            }
        }

        public static bool UpdateDailyComments(DailyCollectionDetail dt, Customer from, Customer To)
        {
            var list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);
            //var data = dt;
            if (list != null && list.Count > 0)
            {
                var data = list.Where(w => w.Date == dt.Date).ToList().FirstOrDefault();

                if (data != null) // Update
                {
                    data.Comments = (data.Comments + (Environment.NewLine + $"Merged {from.Name}({from.CustomerSeqNumber}) to {To.Name}({To.CustomerSeqNumber})"));
                    WriteObjectsToFile(list, JsonFilePath);
                    return true;
                }
            }

            return false;
        }


        public static void UpdateVerifyDetails(DailyCollectionDetail dailyCol)
        {
            var list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);
            if (list != null && list.Count > 0)
            {
                var data = list.Where(w => w.Date == dailyCol.Date).ToList().FirstOrDefault();

                if (data != null) // Update
                {
                    data.InputMoney = dailyCol.InputMoney;
                    data.OutGoingMoney = dailyCol.OutGoingMoney;
                    data.Difference = dailyCol.Difference;
                    data.ExpectedInHand = dailyCol.ExpectedInHand;
                    data.ActualInHand = dailyCol.ActualInHand;
                    data.MamaAccount = dailyCol.MamaAccount;
                    data.MamaExpenditure = dailyCol.MamaExpenditure;
                    data.MamaInputMoney = dailyCol.MamaInputMoney;
                }

                WriteObjectsToFile(list, JsonFilePath);
            }
        }


        public static void DeleteDailyCxnDetails(DateTime cxndate)
        {
            try
            {
                List<DailyCollectionDetail> list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);

                var itemToDelete = list.Where(c => Convert.ToDateTime(c.Date).Date == cxndate.Date).FirstOrDefault();
                list.Remove(itemToDelete);

                WriteObjectsToFile(list, JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DailyCollectionDetail GetDailyTxn(DateTime date, bool isOnLoad)
        {
            try
            {
                List<DailyCollectionDetail> list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);
                var dailyTxn = list.Where(c => c.Date == date.ToShortDateString()).FirstOrDefault();

                return dailyTxn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }


        /// <summary>
        /// Specifies the last collection date ie., last date mama gave the info.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetLastCollectionDate()
        {
            try
            {
                List<DailyCollectionDetail> list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);
                var dailyTxn = list.Select(s => Convert.ToDateTime(s.Date)).Max();

                return dailyTxn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DailyCollectionDetail GetLastCollection()
        {
            try
            {
                List<DailyCollectionDetail> list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);
                var dailyTxn = list.Select(s => s).Last();

                return dailyTxn;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        /// <summary>
        /// Specifies the next collection date ie.,next to last.
        /// </summary>
        /// <returns></returns>
        public static DateTime GetNexttCollectionDate()
        {
            try
            {
                List<DailyCollectionDetail> list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);
                var dailyTxn = list.Select(s => Convert.ToDateTime(s.Date)).Max();

                return dailyTxn.AddDays(1);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static DailyCollectionDetail GetActualInvestmentTxnDate(DateTime? date = null)
        {
            try
            {
                List<DailyCollectionDetail> list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);

                DailyCollectionDetail actualInvestment;


                var data = new DailyCollectionDetail();
                if (date == null)
                {
                    data = null;
                }
                else
                {
                    data = list.Where(w => Convert.ToDateTime(w.Date).Date == date.Value.Date).FirstOrDefault();
                }

                if (date == null || data == null)
                {
                    actualInvestment = list.OrderBy(s => Convert.ToDateTime(s.Date)).Last();
                }
                else
                {

                    actualInvestment = list.Where(w => Convert.ToDateTime(w.Date).Date == date.Value.Date).First();
                }

                return actualInvestment;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

    }
}