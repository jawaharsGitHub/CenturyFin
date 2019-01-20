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
                var yearMonth = Convert.ToDateTime(MonthYear).ToString("yyyyMM").ToInt32();

                if (yearMonth <= 201805)
                {
                    return 10000;
                }
               
                else if (yearMonth > DateTime.Today.ToString("yyyyMM").ToInt32())
                {
                    return 0;
                }
                else if (yearMonth >= 201902) // 2019 salary increase. 2k increase)
                {
                    return 14000;
                }

                else //(month > 201805 && month <= DateTime.Today.ToString("yyyyMM").ToInt32())
                {
                    return 12000;
                }

                //return (Convert.ToDateTime(MonthYear).Month >= 6 || Convert.ToDateTime(MonthYear).Year >= 2019) ? 12000 : 10000;
            }
        }
        public int CloseCount { get; set; }

    }
}
