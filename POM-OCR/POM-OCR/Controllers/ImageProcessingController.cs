using IronOcr;
using System;
using System.Collections.Generic;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using POM_OCR.Models;
using Image = System.Web.UI.WebControls.Image;

namespace POM_OCR.Controllers
{
    public class ImageProcessingController : Controller
    {
        public ActionResult OpenImage()
        {
            HttpCookie imagePath = Request.Cookies["ImageTestCookie"];
            var path = imagePath.Value;

            return View(null, null, path);
        }

        // GET: ImageProcessing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult OcrImage()
        {
            List<OcrResult> ocrResults = new List<OcrResult>();

            HttpCookie imagePath = Request.Cookies["ImageTestCookie"];
            var path = Server.MapPath(imagePath.Value);
            ViewBag.Path = imagePath.Value.ToString();

            List<Emgu.CV.Image<Bgr, Byte>> rectangles = ImageToRectangles.GetRectangles(path);
            int i = 0;
           
            foreach (var item in rectangles)
            {
                // item.ToBitmap().Save("nazwa.png");
                //using (MemoryStream myMemoryStream = new MemoryStream())
                //{
                // System.Drawing.Image mySystemImage = item.ToBitmap();
                // var path1 = "obrazek.png";
                //  mySystemImage.Save(path1);
                //mySystemImage.Save(myMemoryStream, ImageFormat.Jpeg);
                //}

                //using (var streamBitmap = new MemoryStream())
                //{
                //    using (System.Drawing.Image img = System.Drawing.Image.FromStream(streamBitmap))
                //    {
                //    img.Save("obrazek.png");
                //}
                //}
                MemoryStream MyMemoryStream = new MemoryStream();

                System.Drawing.Image MySystemImage = item.ToBitmap();
                MySystemImage.Save(MyMemoryStream, ImageFormat.Jpeg);
                using (FileStream file = new FileStream(path, FileMode.Create, System.IO.FileAccess.Write))
                {
                    byte[] bytes = new byte[MyMemoryStream.Length];
                    MyMemoryStream.Read(bytes, 0, (int)MyMemoryStream.Length);
                    file.Write(bytes, 0, bytes.Length);
                    MyMemoryStream.Close();
                }
                i++;
                //path = "nazwa" + i + ".png";
                // zapis Image do folderu
                // wyciągnięcie ścieżki do obrazu
                // string path = ....

                ocrResults.Add(DoOcr(path));
            }

            return View(ocrResults);
            //
        }

        OcrResult DoOcr(string path)
        {
            var Ocr = new AdvancedOcr()
            {
                CleanBackgroundNoise = true,
                EnhanceContrast = true,
                EnhanceResolution = true,
                Language = IronOcr.Languages.English.OcrLanguagePack,
                Strategy = IronOcr.AdvancedOcr.OcrStrategy.Advanced,
                ColorSpace = AdvancedOcr.OcrColorSpace.GrayScale,
                DetectWhiteTextOnDarkBackgrounds = false,
                InputImageType = AdvancedOcr.InputTypes.AutoDetect,
                RotateAndStraighten = false,
                ReadBarCodes = false,
                ColorDepth = 4
            };
            
            OcrResult Result;
            if (Path.GetExtension(path) == "pdf")
            {
                Result = Ocr.ReadPdf(path);
            }
            else Result = Ocr.Read(path);

            return Result;
        }



    }
}