using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common
{

    public static class HTMLhelper
    {
        private static Bitmap measurementBitmap;
        private static Graphics measurementGraphics;

        static HTMLhelper()
        {
            measurementBitmap = new Bitmap(1, 1, PixelFormat.Format32bppArgb);
            measurementGraphics = Graphics.FromImage(measurementBitmap);
        }

        public static Graphics Graphics
        {
            get
            {
                return measurementGraphics;
            }
        }

        private static void RenderStringToJpeg(string htmlString, Color backgroundColor, Color textColor, Font font, string filename)
        {
            SizeF size = HTMLhelper.Graphics.MeasureString(htmlString, font);
            Bitmap bm = new Bitmap((int)Math.Ceiling(size.Width), (int)Math.Ceiling(size.Height), PixelFormat.Format32bppArgb);
            Graphics g = Graphics.FromImage(bm);
            g.Clear(backgroundColor);
            g.DrawString(htmlString, font, new SolidBrush(textColor), new PointF(0, 0));
            bm.Save(filename, ImageFormat.Jpeg);
        }

        public static void HtmlToImg(string htmlString, string fileName)
        {
            //string htmlString = "<div><div>Test string</div><div>test 2</div></div>";
            Color backgroundColor = Color.White;
            Color textColor = Color.Black;
            Font font = new Font("Arial", 12.0f);
            //string filename = "image.jpg";

            RenderStringToJpeg(htmlString, backgroundColor, textColor, font, fileName);
        }

    }


}
