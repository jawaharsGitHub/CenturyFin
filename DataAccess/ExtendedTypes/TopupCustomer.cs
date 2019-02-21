using Common;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExtendedTypes
{
    public class TopupCustomer : Customer
    {
        private static readonly string JsonFilePath = AppConfiguration.TopupCustomerFile;

        public static void AddTopupCustomer(Customer newCustomer)
        {
            newCustomer.ModifiedDate = null;
            newCustomer.IsActive = true;
            newCustomer.ClosedDate = null;
            

            InsertSingleObjectToListJson(JsonFilePath, newCustomer);


        }

        public static List<TopupCustomer> GetAllTopupCustomer()
        {
            try
            {
                List<TopupCustomer> list = ReadFileAsObjects<TopupCustomer>(JsonFilePath);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }
    }
}
