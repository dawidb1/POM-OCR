using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.IO;


namespace POM_OCR.Models
{
    public class ImageToRectangles
    {
        Image<Bgr, Byte> img;
        List<Emgu.CV.Image<Bgr,Byte>> resultList;
        static int counter;
        static int enumCounter;

        void doMagic(string path)
        {
            //string middle = @"\Image\";
            //string root = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            //string path = $"{root}{middle}{fileName}";

            img = new Image<Bgr, Byte>(path);
            DetectText(img);
        }

        private void DetectText(Image<Bgr, byte> img)
        {
            /*
             1. Edge detection (sobel)
             2. Dilation (10,1)
             3. FindContours
             4. Geometrical Constrints
             */
            //sobel
            Image<Gray, byte> sobel = img.Convert<Gray, byte>().Sobel(1, 0, 3).AbsDiff(new Gray(0.0)).Convert<Gray, byte>().ThresholdBinary(new Gray(50), new Gray(255));
            Mat SE = CvInvoke.GetStructuringElement(Emgu.CV.CvEnum.ElementShape.Rectangle, new System.Drawing.Size(10, 2), new System.Drawing.Point(-1, -1));
            sobel = sobel.MorphologyEx(Emgu.CV.CvEnum.MorphOp.Dilate, SE, new System.Drawing.Point(-1, -1), 1, Emgu.CV.CvEnum.BorderType.Reflect, new MCvScalar(255));
            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
            Mat m = new Mat();

            CvInvoke.FindContours(sobel, contours, m, Emgu.CV.CvEnum.RetrType.External, Emgu.CV.CvEnum.ChainApproxMethod.ChainApproxSimple);

            List<Rectangle> list = new List<Rectangle>();
            Image<Bgr, byte> result = img.Copy();
            Image<Bgr, byte> mask;

            resultList = new List<Image<Bgr,Byte>>();

            for (int i = 0; i < contours.Size; i++)
            {
                Rectangle brect = CvInvoke.BoundingRectangle(contours[i]);

                double ar = brect.Width / brect.Height;
                if (ar > 2 && brect.Width > 25 && brect.Height > 8 && brect.Height < 100)
                {
                    mask = new Image<Bgr, byte>(result.Size);
                    list.Add(brect);
                    CvInvoke.Rectangle(mask, brect, new MCvScalar(255, 255, 255), -1);
                    mask._And(result);
                    resultList.Add(mask);
                }
            }
        }
    }

   
}