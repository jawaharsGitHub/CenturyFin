using System;

namespace DataAccess.ExtendedTypes
{
    public class DynamicReportClosedSoon
    {
        public int RunningDays { get; set; }
        public string Name { get; set; }
        public int LoanAmount { get; set; }
        public int Balance { get; set; }
        public decimal BalancePerc { get; set; }
        public double CreditScore { get; set; }
        public int NeedToClose { get; set; }
        public int DaysToClose { get; set; }
        public DateTime? AmountGivenDate { get; set; }
        public int CustomerSeqNumber { get; set; }
        public int Interest { get; set; }
    }
}
