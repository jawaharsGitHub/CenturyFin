using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAccess
{
//test
    public class Expenditure
    {
        public string Date { get; set; }

        public int Amount { get; set; }
        public string Reason { get; set; }


        public static void AddExpenditure(Expenditure expenditure)
        {
            expenditure.Date = DateTime.Today.ToLongTimeString();

            string baseJson = File.ReadAllText(AppConfiguration.ExpenditureFile);

            //Merge the customer
            string updatedJson = AddObjectsToJson(baseJson, expenditure);

            // Add into json
            File.WriteAllText(AppConfiguration.ExpenditureFile, updatedJson);

        }

        public static List<Expenditure> GetAllExpenditure()
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.ExpenditureFile);
                List<Expenditure> list = JsonConvert.DeserializeObject<List<Expenditure>>(json);
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }

        public static int GetTotalExpenditure()
        {

            try
            {
                var json = GetAllExpenditure();
                return json == null ? 0 : json.Sum(s => s.Amount);
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public static string AddObjectsToJson<T>(string json, T objects)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            list.Add(objects);
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }


    }


}
