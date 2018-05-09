using IronOcr;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;
using System.Drawing;
using System.Web.Script.Serialization;
using POM_OCR.Models;
using POM_OCR.Models.ViewModels;

namespace POM_OCR.Controllers
{
    public class ImageProcessingController : Controller
    {
        public ActionResult OpenImage()
        {
            HttpCookie imagePath = Request.Cookies["ImageTestCookie"];
            var path = imagePath.Value;



            return View(null, null);
        }

        // GET: ImageProcessing
        public ActionResult Index()
        {
            return View();
        }

        public ActionResult _OcrImage(ICollection<CropperViewModel> CropList)
        {
            List<OcrResult> OcrResults = new List<OcrResult>();


            HttpCookie imagePath = Request.Cookies["ImageTestCookie"];
            var path = Server.MapPath(imagePath.Value);
            ViewBag.Path = imagePath.Value.ToString(); 

            var image = ImageToRectangles.RemovePictures(path, (List<CropperViewModel>)CropList);
            //var bitmapBytes =ImageToRectangles.BitmapToBytes(image.ToBitmap()); //Convert bitmap into a byte array
            //return Json(bitmapBytes); //Return as file result 

            //var truebytes = new byte[bytes.Length];
            //for (int i = 0; i < bytes.Length; i++)
            //{
            //    truebytes[i] = (byte)bytes[i];
            //}

            //Bitmap bitmap1;

            //using (MemoryStream ms = new MemoryStream(truebytes))
            //{
            //    bitmap1 = new Bitmap(ms);
            //}
            //var image = new Emgu.CV.Image<Bgr, Byte>(bitmap1);

            //#region image to rectangles
            //List<Emgu.CV.Image<Bgr, Byte>> rectangles = ImageToRectangles.GetRectangles(image);

            //foreach (var item in rectangles)
            //{
            //    // zapis Image do folderu
            //    // wyciągnięcie ścieżki do obrazu
            //    // string path = ....
            //    var bitmap = item.ToBitmap();
            //    var ocrResult = DoOcr(bitmap);
            //    OcrResults.Add(ocrResult);
            //}



            ////return Json(RenderViewToString PartialView());
            ////var qs = HttpUtility.ParseQueryString("");
            //string qs = "";
            //OcrResults.ForEach(x => qs += "<br>" + x.Text);
            //#endregion
            var ocrResult = DoOcr(image.ToBitmap());


            //return Json(Url.Action("PartialViewSample",qs));
            return Json(new { Url = Url.Action("Result"), Data = ocrResult.Text});
        }

        public ActionResult Result()
        {
            return PartialView();
        }

        OcrResult DoOcr(Bitmap bitmap)
        {
            var Ocr = new AutoOcr();
            
            return Ocr.Read(bitmap);
        }
        OcrResult DoAdvancedOcr(Bitmap bitmap)
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
            return Ocr.Read(bitmap);
        }
    }
}