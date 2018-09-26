using System;

namespace DataAccess.PrimaryTypes
{
    public class IncomeReport
    {
        public string MonthYear { get; set; }
        public int ExpectedIncome { get; set; }
        public int ActualIncome { get; set; }
        public int MonthlySalary
        {
            get
            {
                return (Convert.ToDateTime(MonthYear).Month >= 6 || Convert.ToDateTime(MonthYear).Year >= 2019) ? 12000 : 10000;
            }
        }
    }
}
