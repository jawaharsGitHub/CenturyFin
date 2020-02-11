using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{
    public static class FileContentReader
    {
        //public static string SendBalanceHtml { get; } = File.ReadAllText("/HTMLTemplate/SendBalance.htm");

        public static string SendBalanceHtml
        {
            get
            {

                try
                {

                    var dataFolder = General.GetDataFolder("CenturyFinCorpApp\\bin\\Debug", "Common\\HTMLTemplate\\");
                    return File.ReadAllText($"{dataFolder}SendBalance.htm");

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

        public static string ReportRunHtml
        {
            get
            {

                try
                {

                    var dataFolder = General.GetDataFolder("CenturyFinCorpApp\\bin\\Debug", "Common\\HTMLTemplate\\");
                    return File.ReadAllText($"{dataFolder}ReportRun.htm");

                }
                catch (Exception ex)
                {

                    throw ex;
                }
            }
        }

    }
}
