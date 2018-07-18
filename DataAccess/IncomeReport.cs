using System;

namespace DataAccess
{
    public class IncomeReport
    {
        public string Month { get; set; }

        public int ExpectedIncome { get; set; }

        public int ActualIncome { get; set; }

        public int MonthlySalary
        {
            get
            {
                return (Convert.ToDateTime(Month).Month >= 6) ? 12000 : 10000;
            }
        }
    }
}
