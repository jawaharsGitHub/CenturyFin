using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;

namespace DataAccess.PrimaryTypes
{
    public class Investment
    {
        public InvestmentFrom InvestType { get; set; }

        public int Amount { get; set; }
        public int Interest { get; set; }

        public int CustomerId { get; set; }

        public int CustomerSequenceNo { get; set; }

        public string CreatedDate { get; set; }


        public static void AddInvestment(Investment investment)
        {
            investment.CreatedDate = DateTime.Today.ToLongTimeString();
            //investment.ModifiedDate = null;
            //investment.IsActive = true;
            //List<Customer> customers = new List<Customer>() { investment };

            // Get existing customers
            string baseJson = File.ReadAllText(AppConfiguration.InvestmentFile);

            //Merge the customer
            string updatedJson = AddObjectsToJson(baseJson, investment);

            // Add into json
            File.WriteAllText(AppConfiguration.InvestmentFile, updatedJson);

        }

        public static List<Investment> GetAllInvestmet()
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.InvestmentFile);
                List<Investment> list = JsonConvert.DeserializeObject<List<Investment>>(json);
                return list;
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }

        public static string AddObjectsToJson<T>(string json, T objects)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            list.Add(objects);
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

    }

    public enum InvestmentFrom
    {
        Jawahar = 1,
        Company,

    }
}
