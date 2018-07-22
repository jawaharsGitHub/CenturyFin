using Common;
using Newtonsoft.Json;
using System;
using System.IO;

namespace DataAccess
{
    public class InHandAndBank
    {


        public int InHandAmount { get; set; }

        public decimal InBank { get; set; }

        public string Date { get; set; }



        public static void AddInHand(InHandAndBank inHand)
        {
            var existingMoney = GetAllhandMoney();
            existingMoney = inHand;

            //if (fromJawahar) existingMoney.JawaharShare += amount;


            var updatedInHand = JsonConvert.SerializeObject(existingMoney, Formatting.Indented);

            // Add into json
            File.WriteAllText(AppConfiguration.InHandFile, updatedInHand);

        }

        public static void ReduceInHand(int amount)
        {
            var existingMoney = GetAllhandMoney();
            existingMoney.InHandAmount -= amount;

            var updatedInHand = JsonConvert.SerializeObject(existingMoney, Formatting.Indented);

            // Add into json
            File.WriteAllText(AppConfiguration.InHandFile, updatedInHand);

        }

        public static InHandAndBank GetAllhandMoney()
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.InHandFile);
                InHandAndBank list = JsonConvert.DeserializeObject<InHandAndBank>(json) ?? new InHandAndBank();
                return list;
            }
            catch (Exception ex)
            {

                throw;
            }
        }


        public static string AddObjectsToJson<T>(string json, T objects)
        {
            T list = JsonConvert.DeserializeObject<T>(json);

            //list.Add(objects);
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }


    }


}
