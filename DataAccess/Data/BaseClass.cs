using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccess.Data
{
    public class BaseClass<T>
    {

        public static string AddObjectsToJson<T>(string json, List<T> objects)
        {
            List<T> list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();

            list.AddRange(objects);
            return JsonConvert.SerializeObject(list, Formatting.Indented);
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
