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
using System.Threading;

namespace text_region_detection 
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        Image<Bgr, Byte> img;
        List<BitmapImage> resultList;
        static int counter;
        static int enumCounter;
        public MainWindow()
        {
            InitializeComponent();
            string fileName = "test0.jpg";
            enumCounter++;

            doMagic(fileName);
        }
        void doMagic(string fileName)
        {
            string middle = @"\Image\";
            string root = Path.GetDirectoryName(Path.GetDirectoryName(System.IO.Directory.GetCurrentDirectory()));
            string path = $"{root}{middle}{fileName}";

            img = new Image<Bgr, Byte>(path);
            image1.Source = convertBitmapToImage(img.Bitmap);

            DetectText(img);
            btnClick.Visibility = Visibility.Visible;
            counter = resultList.Count;
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

            resultList = new List<BitmapImage>();

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
                    resultList.Add(convertBitmapToImage(mask.Bitmap));
                }
            }
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

        private void btnClick_Click(object sender, RoutedEventArgs e)
        {
            if (counter > 0)
            {
                counter--;
                image2.Source = resultList[counter];
            }
            else
            {
                MessageBox.Show("Koniec wykrytych fragmentów");
                string test = Enum.GetName(typeof(testList), enumCounter);
                test = test + ".jpg";
                doMagic(test);
                if (enumCounter < 7)
                {
                    enumCounter++;
                }
                else enumCounter = 0;
            }
        }
    }
    enum testList
    {
        test0,test1,test2,test3,test4,test5,test6,test7,test8
    };
}
