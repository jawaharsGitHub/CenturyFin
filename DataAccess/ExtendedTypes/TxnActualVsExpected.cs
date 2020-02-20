using Common.ExtensionMethod;
using System;

namespace DataAccess.ExtendedTypes
{
    public class TxnActualVsExpected
    {
        public int Expected { get; set; }
        public int Actual { get; set; }

        public int DaysTaken { get; set; }
        public string LastTxnDate { get; set; }

        public int PerDayPayment { get; internal set; }

        public int PaidWeek
        {
            get
            {
                return (Actual / PerDayPayment) / 7;
            }
        }
        public int TakenWeek
        {
            get
            {
                return DaysTaken / 7;
            }
        }

        public decimal WeekDiff
        {
            get
            {
                return (TakenWeek - PaidWeek);
            }
        }

        public string DataForCol
        {
            get
            {
                return $"{Expected}(E) Vs {Actual}(A)[{WeekDiff}] - {LastTxnDate}";
            }
        }



    }
}
