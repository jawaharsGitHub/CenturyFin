using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace CenturyFinCorpApp
{
    public partial class frmPrediction : UserControl
    {
        public frmPrediction()
        {
            InitializeComponent();
        }


        public static void Predict()
        {

            var inputMoney = 1000000;

            var previousPrediction = new Prediction() { NewOutstanding = inputMoney };
            List<Prediction> lst = new List<Prediction>();



            for (int i = 1; i <= 100; i++)
            {
                var predict = new Prediction() { OutstandingAmount = previousPrediction.NewOutstanding, NewCustomerNumber = i };

                predict.CollectionAmount = (previousPrediction.NewOutstanding / 100) + ((predict.NewCustomerNumber - 1) * 100);

                predict.OutstandingAmount = previousPrediction.NewOutstanding - predict.CollectionAmount;

                predict.InHandBeforeGiven = predict.CollectionAmount + previousPrediction.InHandAfterGiven;

                var canGive = (predict.InHandBeforeGiven / 10000) > 0 ? (predict.InHandBeforeGiven / 10000) * 10000 : 0;

                //predict.InHandAfterGiven = (predict.InHandBeforeGiven % 10000);

                
                int interest = 0;
                if (canGive >= 10000)
                {
                    interest = (canGive / 100) * 10; // given money's 10%'
                    predict.GivenMoney = canGive; // ROund 0f 1000 inhand money.
                    predict.InHandAfterGiven = (predict.InHandBeforeGiven - (predict.GivenMoney - interest)); // + (predict.InHandBeforeGiven % 10000);
                }

                predict.NewOutstanding = predict.OutstandingAmount + predict.GivenMoney;

                predict.TotalAsset = predict.NewOutstanding + interest;

                lst.Add(predict);

                previousPrediction = predict;


            }

        }
    }

    public class Prediction
    {

        public int NewCustomerNumber { get; set; }

        public int CollectionAmount { get; set; }

        public int OutstandingAmount { get; set; }

        public int InHandBeforeGiven { get; set; }

        public int GivenMoney { get; set; }

        public int InHandAfterGiven { get; set; }

        public int NewOutstanding { get; set; }

        public int TotalAsset { get; set; }

    }
}
