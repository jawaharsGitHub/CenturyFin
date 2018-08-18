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
        public int? SanthanamUncle { get; set; }
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


        public static void AddDaily(DailyCollectionDetail dailyCol)
        {
            InsertSingleObjectToListJson(JsonFilePath, dailyCol);
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

    }
}