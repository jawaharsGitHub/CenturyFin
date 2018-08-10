using DataAccess.PrimaryTypes;

namespace DataAccess.ExtendedTypes
{
    public class CustomerDailyTxn : Transaction
    {
        public string CustomerName { get; set; }
    }
}
