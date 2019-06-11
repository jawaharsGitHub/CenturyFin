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
                    //data.BankTxnOut = dailyCol.BankTxnOut;
                    //data.ClosedAccounts = dailyCol.ClosedAccounts;
                    //data.CollectionAmount = dailyCol.CollectionAmount;
                    data.Comments = dailyCol.Comments;
                    //data.Date = dailyCol.Date;
                    //data.GivenAmount = dailyCol.GivenAmount;
                    //data.InBank = dailyCol.InBank;
                    //data.Interest = dailyCol.Interest;
                    //data.OpenedAccounts = dailyCol.OpenedAccounts;
                    //data.OtherExpenditire = dailyCol.OtherExpenditire;
                    //data.OtherInvestment = dailyCol.OtherInvestment;
                    //data.SentFromUSA = dailyCol.SentFromUSA;
                    //data.TakenFromBank = dailyCol.TakenFromBank;
                    //data.TodayInHand = dailyCol.TodayInHand;
                    //data.TomorrowDiff = dailyCol.TomorrowDiff;
                    //data.TomorrowNeed = dailyCol.TomorrowNeed;
                    //data.YesterdayAmountInHand = dailyCol.YesterdayAmountInHand;

                    WriteObjectsToFile(list, JsonFilePath);
                }
                else // New (Insert)
                {
                    InsertSingleObjectToListJson(JsonFilePath, dailyCol);

                    // Update In Hand and In Bank amount.
                    var inhand = new InHandAndBank()
                    {
                        Date = dailyCol.Date,
                        InBank = dailyCol.InBank.Value,
                        InHandAmount = dailyCol.TodayInHand.Value
                    };

                    InHandAndBank.AddInHand(inhand, dailyCol.TakenFromBank);

                }

            }


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

        public static DateTime GeLatesttDailyTxnDate()
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

        public static int GetActualInvestmentTxnDate(DateTime? date = null)
        {
            try
            {
                List<DailyCollectionDetail> list = ReadFileAsObjects<DailyCollectionDetail>(JsonFilePath);

                var actualInvestment = 0;


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
                    actualInvestment = list.OrderBy(s => Convert.ToDateTime(s.Date)).Last().ActualMoneyInBusiness;
                }
                else
                {

                    actualInvestment = list.Where(w => Convert.ToDateTime(w.Date).Date == date.Value.Date).First().ActualMoneyInBusiness;
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