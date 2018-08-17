using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

namespace Common
{
    public class BaseClass
    {

        public static void InsertObjectsToJson<T>(string filePath, List<T> objectsToAdd)
        {
            if (File.Exists(filePath) == false) File.Create(filePath);

            // Get existing customers
            //string baseJson = File.ReadAllText(AppConfiguration.CustomerFile);

            List<T> list = ReadFileAsObjects<T>(filePath); //JsonConvert.DeserializeObject<List<T>>(baseJson) ?? new List<T>();

            list.AddRange(objectsToAdd); // add to existing customer.

            WriteObjectsToFile(list, filePath);

            //var updatedJson = JsonConvert.SerializeObject(list, Formatting.Indented);

            //// write to teh file.
            //File.WriteAllText(AppConfiguration.CustomerFile, updatedJson);


        }

        public static void InsertSingleObjectToListJson<T>(string filePath, T singleObject)
        {
            //List<T> list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            //list.Add(objects);
            //return JsonConvert.SerializeObject(list, Formatting.Indented);

            List<T> list = new List<T>() { singleObject };

            InsertObjectsToJson(filePath, list);
        }

        // TODO: Need to change as json array file.
        public static void InsertSingleObjectToSingleJson<T>(string filePath, T singleObject)
        {
            //List<T> list = JsonConvert.DeserializeObject<List<T>>(json) ?? new List<T>();
            //list.Add(objects);
            //return JsonConvert.SerializeObject(list, Formatting.Indented);

            //List<T> list = new List<T>() { singleObject };

            //InsertObjectsToJson(filePath, list);
            WriteSingleObjectToFile(singleObject, filePath);

        }

        public static List<T> ReadFileAsObjects<T>(string filePath)
        {
            var jsonText = File.ReadAllText(filePath);
            List<T> list = JsonConvert.DeserializeObject<List<T>>(jsonText) ?? new List<T>();
            return list;
        }

        public static void WriteObjectsToFile<T>(List<T> listObject, string filePath)
        {
            string jsonString = JsonConvert.SerializeObject(listObject, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }


        public static T ReadFileAsSingleObject<T>(string filePath)
        {
            var jsonText = File.ReadAllText(filePath);
            T obj = JsonConvert.DeserializeObject<T>(jsonText);
            return obj;
        }

        public static void WriteSingleObjectToFile<T>(T listObject, string filePath)
        {
            string jsonString = JsonConvert.SerializeObject(listObject, Formatting.Indented);
            File.WriteAllText(filePath, jsonString);
        }



    }
}
