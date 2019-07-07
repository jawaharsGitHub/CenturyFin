using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExtendedTypes
{
    public static class CustomerExtension
    {

        public static bool IsMonthly(this Customer cus)
        {
            return (cus.ReturnType == ReturnTypeEnum.Monthly || cus.ReturnType == ReturnTypeEnum.GoldMonthly);
        }

        public static bool IsNotMonthly(this Customer cus)
        {
            return (cus.ReturnType != ReturnTypeEnum.Monthly && cus.ReturnType != ReturnTypeEnum.GoldMonthly);
        }
    }


    public static class DynamicReportNotGivenDaysExtension
    {

        public static bool IsMonthly(this DynamicReportNotGivenDays cus)
        {
            return (cus.ReturnType == ReturnTypeEnum.Monthly || cus.ReturnType == ReturnTypeEnum.GoldMonthly);
        }

        public static bool IsNotMonthly(this DynamicReportNotGivenDays cus)
        {
            return (cus.ReturnType != ReturnTypeEnum.Monthly && cus.ReturnType != ReturnTypeEnum.GoldMonthly);
        }
    }
}
