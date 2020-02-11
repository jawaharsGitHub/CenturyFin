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
        public static string SendBalanceHtml { get; } = GetHtmlFileContent("SendBalance.htm");

        public static string ReportRunHtml { get; } = GetHtmlFileContent("ReportRun.htm");

        private static string GetHtmlFileContent(string fileName)
        {
            var dataFolder = General.GetDataFolder("CenturyFinCorpApp\\bin\\Debug", "Common\\HTMLTemplate\\");
            return File.ReadAllText($"{dataFolder}{fileName}");
        }

    }
}
