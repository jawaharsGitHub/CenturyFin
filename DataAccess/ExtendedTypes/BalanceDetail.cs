using Common;
using DataAccess.PrimaryTypes;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Common.ExtensionMethod;

namespace DataAccess.ExtendedTypes
{
    public class BalanceDetail
    {


        public static int GrossProfit { get; set; }

        public static int? ActualInHand { get; set; }

        public static int? MamaAccount { get; set; }



        public static string GetFullBalanceReport()
        {
            GrossProfit = Transaction.GetAllOutstandingAmount().includesProfit;

            var latestDailyCxn = DailyCollectionDetail.GetActualInvestmentTxnDate();

            ActualInHand = latestDailyCxn.ActualInHand;
            MamaAccount = latestDailyCxn.MamaAccount;

            var result =
                $"மாமாவிடம் கை-இருக்கும் பணம்: <b>{ActualInHand.ToMoney()}</b>" + "<br>" +
                $"மாமா தரவேண்டிய பணம்: <b>{MamaAccount.ToMoney()}</b>" + "<br>" +
                $"வெளியில் நிற்கும் பணம்: <b>{GrossProfit.ToMoneyFormat()}</b>" + "<br><br>";

            return result;


        }

    }
}
