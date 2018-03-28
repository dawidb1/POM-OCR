using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Windows;
using System.IO;

namespace text_region_detection
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            Image<Bgr, Byte> img = new Image<Bgr, Byte>(@"C:\Users\Dawid\Documents\GitHub\POM-OCR\POM-OCR\text region detection\Image\Przechwytywanie.PNG");
            detectLetters(img);
            //List<Rectangle> rects = detectLetters(img);
            //for (int i = 0; i < rects.Count(); i++)
            //    img.Draw(rects.ElementAt<Rectangle>(i), new Bgr(0, 255, 0), 3);
            //CvInvoke.cvShowImage("Display", img.Ptr);
            //CvInvoke.cvWaitKey(0);
            //CvInvoke.cvDestroyWindow("Display");
        }
        //List<Rectangle>
        public void detectLetters(Image<Bgr, Byte> img)
        {
            List<Rectangle> rects = new List<Rectangle>();
            Image<Gray, Single> img_sobel;
            Image<Gray, Byte> img_gray, img_threshold, imgout;
            img_gray = img.Convert<Gray, Byte>();
            img_sobel = img_gray.Sobel(1, 0, 3);
            img_threshold = new Image<Gray, byte>(img_sobel.Size);
            imgout = new Image<Gray, byte>(img_gray.Size);

            CvInvoke.Threshold(img_sobel.Convert<Gray, Byte>(), img_threshold, 0, 255, ThresholdType.Otsu);

            Mat structure = CvInvoke.GetStructuringElement(ElementShape.Rectangle, new System.Drawing.Size(3, 17), new System.Drawing.Point(1, 6));
            CvInvoke.MorphologyEx(img_threshold, img_threshold, MorphOp.Close, structure, new System.Drawing.Point(-1,-1), 1,  BorderType.Default, new MCvScalar());

            Emgu.CV.Util.VectorOfVectorOfPoint contours = new Emgu.CV.Util.VectorOfVectorOfPoint();
            Mat hier = new Mat();

            CvInvoke.FindContours(img_threshold, contours, hier, RetrType.External, ChainApproxMethod.ChainApproxNone);

          
            CvInvoke.DrawContours(imgout, contours, -1, new MCvScalar(255, 0, 0));

            image1.Source = convertBitmapToImage(imgout.Bitmap);

            //   imgout.Bitmap;
            //for (contours = img_threshold.FindContours(); contours != null; contours = contours.HNext)
            //{
            //    if (contours.Area > 100)
            //    {
            //        Contour<System.Drawing.Point> contours_poly = contours.ApproxPoly(3);
            //        rects.Add(new Rectangle(contours_poly.BoundingRectangle.X, contours_poly.BoundingRectangle.Y, contours_poly.BoundingRectangle.Width, contours_poly.BoundingRectangle.Height));
            //    }
            //}

        }
        BitmapImage convertBitmapToImage(Bitmap bmp)
        {
            MemoryStream ms = new MemoryStream();
            bmp.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
            ms.Position = 0;
            BitmapImage bi = new BitmapImage();
            bi.BeginInit();
            bi.StreamSource = ms;
            bi.EndInit();

            return bi;
        }
    }
}
