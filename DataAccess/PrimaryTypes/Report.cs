using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.PrimaryTypes
{
    public static class Report
    {

        public static int GetActualLoss()
        {
            var loss = (from cc in Customer.GetClosedCustomer()
                       where cc.Interest < 0
                       select cc.Interest).Sum();

            return Math.Abs(loss);
        }

        public static int GetExpectedLoss()
        {
            var loss = (from cc in Customer.GetActiveCustomer()
                        where cc.NeedInvestigation
                        select Transaction.GetBalanceAndLastDate(cc).Balance - cc.Interest).Sum();

            return loss;
        }
    }
}
