
using System;
using System.Diagnostics;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

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

        public static void CreateHTML(string fileName, string htmlString)
        {
            using (FileStream fs = new FileStream(fileName, FileMode.Create))
            {
                using (StreamWriter w = new StreamWriter(fs, Encoding.UTF8))
                {
                    w.WriteLine(htmlString);
                }
            }

            Process.Start(fileName);
        }


        //public static void ConvertHtmlToImage()
        //{
        //    Bitmap m_Bitmap = new Bitmap(400, 600);
        //    PointF point = new PointF(0, 0);
        //    SizeF maxSize = new System.Drawing.SizeF(500, 500);
        //    HtmlRender.Ren(Graphics.FromImage(m_Bitmap),
        //                                            "<html><body><p>This is a shitty html code</p>"
        //                                            + "<p>This is another html line</p></body>",
        //                                             point, maxSize);

        //    m_Bitmap.Save(@"C:\Test.png", ImageFormat.Png);
        //}



    }
}
