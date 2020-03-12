using Common.ExtensionMethod;
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
                var salary = 0;
                var month = Convert.ToDateTime(MonthYear).Month;

                var yearMonth = Convert.ToDateTime(MonthYear).ToString("yyyyMM").ToInt32();

                var year = Convert.ToDateTime(MonthYear).ToString("yyyy").ToInt32();

                // SALARY VARIATIONS.
                if (yearMonth <= 201805)
                {
                    salary = 10000;
                }
                else if (yearMonth > DateTime.Today.ToString("yyyyMM").ToInt32())
                {
                    salary = 0;
                }
                else if (yearMonth >= 201902 && yearMonth <= 201904) // 2019 salary increase. 2k increase)
                {
                    salary = 14000;
                }
                else if (yearMonth >= 201905) // 2019 salary increase. 1k increase)
                {
                    salary = 15000;
                }

                else //(month > 201805 && month <= DateTime.Today.ToString("yyyyMM").ToInt32())
                {
                    salary = 12000;
                }

                // SALARY VARIATIONS.
                if (yearMonth >= 201903)
                {
                    salary += 2500;
                }

                // EXTRA CONDITIONS.
                if (year <= 2018 && (month == 1 || month == 10))
                {
                    salary += 10000;
                }
                else if (year == 2019 && month == 1)
                {
                    salary += 10000;
                }
                else if (year >= 2019 && month == 10)
                {
                    salary += 5000;
                }
                else if (yearMonth == 201904)
                {
                    salary += 10000;
                }

                return salary;
            }
        }
        public int CloseCount { get; set; }

    }
}
