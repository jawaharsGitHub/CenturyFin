using System;

namespace DataAccess.ExtendedTypes
{
    public class DynamicReportNotGivenDays
    {
        public string Name { get; set; }
        public int LoanAmount { get; set; }
        public int Balance { get; set; }
        public double CreditScore { get; set; }
        public double NotGivenFor { get; set; }
        public DateTime LastTxnDate { get; set; }
        public DateTime? AmountGivenDate { get; set; }
        public int CustomerSeqNumber { get; set; }
        public ReturnTypeEnum ReturnType { get; set; }
        public int? Interest { get; set; }
        public bool NeedInvestigation { get; set; }

    }
}
