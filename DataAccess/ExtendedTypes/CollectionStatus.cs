using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExtendedTypes
{
    public class CollectionStatus
    {
        public string Name { get; set; }
        public int LoanAmount { get; set; }
        public int CxnAmount { get; set; }
        public int Balance { get; set; }
        public string LastDate { get; set; }
        public DateTime? TxnDate { get; set; }
        public int? TxnId { get; set; }

        public ReturnTypeEnum ReturnType { get; set; }

        public bool IsPersonal { get; set; }
        public string IsToday
        {
            get
            {
                return DailyCollectionDetail.GetLastCollectionDate().ToShortDateString() == LastDate ? "Y" : "N";
            }
        }
    }
}
