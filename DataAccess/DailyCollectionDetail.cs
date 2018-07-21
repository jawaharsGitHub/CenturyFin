using Common;
using DataAccess.Data;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class DailyCollectionDetail : BaseClass<DailyCollectionDetail>
    {
        public DateTime Date { get; set; }

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



        public static void AddDaily(DailyCollectionDetail dailyCol)
        {

            List<DailyCollectionDetail> dilyTxns = new List<DailyCollectionDetail>() { dailyCol };

            // Get existing customers
            string baseJson = File.ReadAllText(AppConfiguration.DailyTxnFile);

            //Merge the customer
            string updatedJson = AddObjectsToJson(baseJson, dilyTxns);

            // Add into json
            File.WriteAllText(AppConfiguration.CustomerFile, updatedJson);

        }

        public static DailyCollectionDetail GetDailyTxn()
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.DailyTxnFile);
                List<DailyCollectionDetail> list = JsonConvert.DeserializeObject<List<DailyCollectionDetail>>(json);
                var dailyTxn = list.OrderByDescending(c => c.Date).FirstOrDefault();



                return dailyTxn;

            }
            catch (Exception ex)
            {

                throw;
            }
        }



    }
}
