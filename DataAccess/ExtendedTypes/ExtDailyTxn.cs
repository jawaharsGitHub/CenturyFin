﻿namespace DataAccess.ExtendedTypes
{
    public class ExtDailyTxn
    {
        public string Date { get; set; }
        public int? CollectionAmount { get; set; }
        //public int? ExpectedCollectionAmount { get; set; }
        public int? Closed { get; set; }
        public int? New { get; set; }
        public int? GivenAmount { get; set; }
    }
}
