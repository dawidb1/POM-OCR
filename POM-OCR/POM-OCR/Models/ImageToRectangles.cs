using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.IO;
using POM_OCR.Models.ViewModels;

namespace POM_OCR.Models
{
    public class ImageToRectangles
    {
     
        static List<Emgu.CV.Image<Bgr,Byte>> resultList;

        public static List<Emgu.CV.Image<Bgr,Byte>> GetRectangles(Image<Bgr, Byte> img)
        {
            //Image<Bgr, Byte> img = new Image<Bgr, byte>(path);
            return DetectText(img);
        }

        public static Emgu.CV.Image<Bgr, Byte> RemovePictures(string path, List<CropperViewModel> CropperList)
        {
            var image1 = new Emgu.CV.Image<Bgr, Byte>(path);
            var newImage = image1.Copy();

            if (CropperList!=null)
            {
                foreach (var cropper in CropperList)
                {
                    //setPixelsWhite(ref image1, cropper);
                    for (int v = cropper.Y; v < cropper.Height + cropper.Y; v++)
                    {
                        for (int u = cropper.X; u < cropper.Width + cropper.X; u++)
                        {
                            newImage.Data[v, u, 0] = 0; //Set Pixel Color | fast way
                            newImage.Data[v, u, 1] = 0; //Set Pixel Color | fast way
                            newImage.Data[v, u, 2] = 0; //Set Pixel Color | fast way
                        }
                    }
                }
                return newImage;
            }
            else return image1;
        }

        private static void setPixelsWhite(ref Emgu.CV.Image<Bgr, Byte> image, CropperViewModel cropper)
        {
            for (int v = cropper.Y; v < cropper.Height; v++)
            {
                for (int u = cropper.X; u < cropper.Width; u++)
                {
                    image.Data[v, u, 0] = 0; //Set Pixel Color | fast way
                    image.Data[v, u, 1] = 0; //Set Pixel Color | fast way
                    image.Data[v, u, 2] = 0; //Set Pixel Color | fast way
                }
            }
        }

        private static List<Emgu.CV.Image<Bgr, Byte>> DetectText(Image<Bgr, byte> img)
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
            resultList.Reverse();
            return resultList;
        }

        public static byte[] BitmapToBytes(Bitmap img)
        {
            using (MemoryStream stream = new MemoryStream())
            {
                img.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                return stream.ToArray();
            }
        }
    }

   
}