using Common;
using Newtonsoft.Json;
using System;
using System.IO;

namespace DataAccess
{
    public class InHand
    {


        public int InHandAmount { get; set; }

        public int JawaharShare { get; set; }
        public string Date { get; set; }



        public static void AddInHand(int amount, bool fromJawahar = false)
        {
            var existingMoney = GetAllhandMoney();
            existingMoney.InHandAmount += amount;

            if (fromJawahar) existingMoney.JawaharShare += amount;


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

        public static InHand GetAllhandMoney()
        {

            try
            {
                var json = File.ReadAllText(AppConfiguration.InHandFile);
                InHand list = JsonConvert.DeserializeObject<InHand>(json) ?? new InHand();
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
