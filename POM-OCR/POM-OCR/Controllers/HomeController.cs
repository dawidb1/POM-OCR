using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POM_OCR.Models;
using IronOcr;

namespace POM_OCR.Controllers
{
    public class HomeController : Controller
    {
        Image image;
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add(Image imageModel)
        {
            image = imageModel;
            //string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            //string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            //imageModel.ImagePath = "~/Image/" + fileName;
            //fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            //imageModel.ImageFile.SaveAs(fileName);

            //HttpCookie imageCookie = new HttpCookie("ImageTestCookie");
            //imageCookie.Value = imageModel.ImagePath;
            //Response.Cookies.Add(imageCookie);

            //inne
            //REQUEST COOKIE
            //HttpCookie imageCookie = Request.Cookies["imageTestCookie"];

            //IMAGE TO DB
            //using (DbModels db = new DbModels())
            //{
            //    db.Images.Add(imageModel);
            //    db.SaveChanges();
            //}
            return RedirectToAction("OpenImage","Home");
        }
        public ActionResult OpenImage()
        {
            ViewBag.Message = "Bla bla";
            HttpCookie imagePath = Request.Cookies["ImageTestCookie"];

            //ViewBag.Path = imagePath.Value.ToString(); //na teraz
            return View(image);
        }
        public ActionResult OcrImage()
        {
            HttpCookie imagePath = Request.Cookies["ImageTestCookie"];
            var path = Server.MapPath(imagePath.Value);
            ViewBag.Path = imagePath.Value.ToString();

            //CHOOSE AUTO OR ADVANCE OR PDF
            //var Ocr = new AutoOcr();
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

            return View(Result);
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}