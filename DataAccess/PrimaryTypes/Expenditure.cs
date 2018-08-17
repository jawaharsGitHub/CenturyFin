﻿using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace DataAccess.PrimaryTypes
{

    public class Expenditure : BaseClass
    {
        public string Date { get; set; }

        public int Amount { get; set; }

        public string Reason { get; set; }


        public static void AddExpenditure(Expenditure expenditure)
        {
            expenditure.Date = DateTime.Today.ToLongTimeString();

            string baseJson = File.ReadAllText(AppConfiguration.ExpenditureFile);

            //Merge the customer
            string updatedJson = AddSingleObjectToJson(baseJson, expenditure);

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
                throw ex;
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
                throw ex;
            }
        }
    }

}
