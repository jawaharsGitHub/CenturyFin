using Common;
using DataAccess.PrimaryTypes;
using System;
using System.Collections.Generic;

namespace DataAccess.ExtendedTypes
{
    public class TopupCustomer : Customer
    {
        private static readonly string JsonFilePath = AppConfiguration.TopupCustomerFile;

        public static void AddTopupCustomer(Customer topupCustomer)
        {
            topupCustomer.ModifiedDate = null;
            topupCustomer.IsActive = true;
            topupCustomer.ClosedDate = null;
            

            InsertSingleObjectToListJson(JsonFilePath, topupCustomer);


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
