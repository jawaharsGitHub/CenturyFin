
using System;
using System.IO;
using System.Net;
using System.Reflection;

namespace Common
{
    public static class General
    {
        public static string GetDaySuffix(int day)
        {
            switch (day)
            {
                case 1:
                case 21:
                case 31:
                    return "st";
                case 2:
                case 22:
                    return "nd";
                case 3:
                case 23:
                    return "rd";
                default:
                    return "th";
            }
        }

        public static string GetDataFolder(string debugFolder, string rootFolder)
        {
            string exeFile = (new Uri(Assembly.GetEntryAssembly().CodeBase)).AbsolutePath;
            string exeDir = Path.GetDirectoryName(exeFile);
            //string dataFolder = exeDir.Replace("CenturyFinCorpApp\\bin\\Debug", "DataAccess\\Data\\");
            string dataFolder = exeDir.Replace(debugFolder, rootFolder);

            return dataFolder;
        }

        public static bool CheckForInternetConnection()
        {
            try
            {
                using (var client = new WebClient())
                using (client.OpenRead("http://google.com/generate_204"))
                    return true;
            }
            catch
            {
                return false;
            }
        }

    }
}
