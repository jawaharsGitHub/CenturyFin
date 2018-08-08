using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess
{
    public class CollectionPerDay : BaseClass<CollectionPerDay>
    {

        public string Date { get; set; }

        public int ActualCollection { get; set; }

        public int ExpectedCollection { get; set; }
    }
}
