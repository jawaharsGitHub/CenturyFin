using Common.ExtensionMethod;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExtendedTypes
{
    public class BalanceCheckData
    {
        public int Balance { get; set; }
        public DateTime LastTxnDate { get; set; }
        public int Diff { get; set; }

        public string DataForColumn
        {
            get
            {
                return $"{Balance} on  {LastTxnDate.ddmmyyyy()}[{Diff - 1}]";
            }

        }
    }
}

