using Common;
using System;
using System.Collections.Generic;

namespace DataAccess.PrimaryTypes
{
    public class DetailedAmount : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.DetailedAmountFile;

        public int WithMe { get; set; }
        public int WithUncle { get; set; }
        public int Itook { get; set; }
        public int UncleTook { get; set; }
        public int UnclesHand { get; set; }
        public string CreatedDate { get; set; } = DateTime.Today.ToLongTimeString();
        public bool IsCorrect { get; set; }


        public static void AddDetailedAmount(DetailedAmount detailedAmount)
        {
            detailedAmount.CreatedDate = DateTime.Today.Date.ToShortDateString();
            InsertSingleObjectToListJson(JsonFilePath, detailedAmount);
        }


    }

}
