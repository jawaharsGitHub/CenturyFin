using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace DataAccess
{
    public class BaseClass
    {

        public static string AddObjectsToJson<T>(string json, List<T> objects)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();

            list.AddRange(objects);
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        public static string AddSingleObjectToJson<T>(string json, T objects)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();

            list.Add(objects);
            return JsonConvert.SerializeObject(list, Formatting.Indented);
        }

        public static List<T> GetAllDetails<T>(string filePath)
        {
            var jsonText = File.ReadAllText(filePath);

            List<T> list = JsonConvert.DeserializeObject<List<T>>(jsonText) ?? new List<T>();

            //list.AddRange(objects);
            //return JsonConvert.SerializeObject(list, Formatting.Indented);
            return list;
        }


        //To update any existing data.
        //public static string AddObjectsToJson(string json, List<DailyCollectionDetail> objects)
        //{
        //    List<DailyCollectionDetail> list = JsonConvert.DeserializeObject<List<DailyCollectionDetail>>(json) ?? new List<DailyCollectionDetail>();

        //    list.AddRange(objects);

        //    list.ForEach(d => {
        //        d.Date = Convert.ToDateTime(d.Date).ToShortDateString();

        //    });
        //    return JsonConvert.SerializeObject(list, Formatting.Indented);
        //}

    }
}
