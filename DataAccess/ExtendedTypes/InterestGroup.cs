using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExtendedTypes
{
    public class InterestGroup
    {
        public int CustomerId { get; set; }
        public string Name { get; set; }
        public int TotalInterest { get; set; }
        public int TotalBalance { get; set; }
        public int Profit
        {
            get
            {
                return TotalInterest - TotalBalance;
            }
        }

        public int Count { get; set; }

    }
}
