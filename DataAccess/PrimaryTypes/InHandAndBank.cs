using Common;
using System;

namespace DataAccess.PrimaryTypes
{
    public class InHandAndBank : BaseClass
    {

        private static string JsonFilePath = AppConfiguration.InHandFile;

        public int InHandAmount { get; set; }

        public decimal InBank { get; set; }

        public string Date { get; set; }

        public int? RealInvestment { get; set; }



        public static void AddInHand(InHandAndBank inHand, int? takenFromBank)
        {
            var existingMoney = GetAllhandMoney();

            inHand.RealInvestment = (existingMoney.RealInvestment + takenFromBank);
            //existingMoney = inHand;

            //var updatedInHand = JsonConvert.SerializeObject(inHand, Formatting.Indented);

            //// Add into json
            //File.WriteAllText(AppConfiguration.InHandFile, updatedInHand);

            InsertSingleObjectToSingleJson(JsonFilePath, inHand);


        }

        public static InHandAndBank GetAllhandMoney()
        {

            try
            {
                //var json = File.ReadAllText(AppConfiguration.InHandFile);
                //InHandAndBank list = JsonConvert.DeserializeObject<InHandAndBank>(json) ?? new InHandAndBank();
                //var list = ReadFileAsObjects
                return ReadFileAsSingleObject<InHandAndBank>(JsonFilePath);
            }
            catch (Exception ex)
            {
                throw ex;
            }
        }



    }


}
