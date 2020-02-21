using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExtendedTypes
{
    public class NotesPerMonth
    {
        public string Month { get; set; }

        public int GivenCount { get; set; }

        public string ClosedCount { get; set; }

        public string LoanAmount { get; set; }

        public string GivenAmount { get; set; }

        public string FutureInterest { get; set; }
    }
}
