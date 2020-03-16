using Common;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.ExtendedTypes
{
    public class BusinessType : BaseClass
    {
        private static string JsonFilePath = AppConfiguration.BusinessTypeFile;

        public int Id { get; set; }

        public string Name { get; set; }

        public string IdAndName
        {
            get
            {
                return $"{Id}-{Name}";
            }
        }

        public static List<BusinessType> GetBusinessTypes()
        {
            var json = File.ReadAllText(JsonFilePath);
            List<BusinessType> list = JsonConvert.DeserializeObject<List<BusinessType>>(json);
            return list;
        }

        public static string AddBusinessType(BusinessType bt)
        {
            var bts = GetBusinessTypes();

            if (bts != null && bts.Count > 0)
            {
                if (bts.Select(s => s.Name.Trim()).Contains(bt.Name.Trim()))
                {
                    return "Duplicate Name";
                }
                else
                {
                    bt.Id = bts.Max(m => m.Id) + 1;
                }
            }
            else
            {
                bt.Id = 1;
            }


            InsertSingleObjectToListJson(JsonFilePath, bt);
            return $"{bt.Id}-{bt.Name} added!";
        }

        public static string UpdateBusinessType(BusinessType bt)
        {
            List<BusinessType> bts = ReadFileAsObjects<BusinessType>(JsonFilePath);

            if (bts.Select(s => s.Name.Trim()).Contains(bt.Name.Trim()))
            {
                return "Duplicate Name";
            }

            var u = bts.Where(c => c.Id == bt.Id).First();
            u.Name = bt.Name;

            WriteObjectsToFile(bts, JsonFilePath);
            return "edit sucess";
        }

        public static bool DeleteBusinessType(int btId)
        {
            var list = ReadFileAsObjects<BusinessType>(JsonFilePath);

            var itemToDelete = list.FirstOrDefault(c => c.Id == btId);
            list.Remove(itemToDelete);

            WriteObjectsToFile(list, JsonFilePath);
            return true;
        }

    }
}
