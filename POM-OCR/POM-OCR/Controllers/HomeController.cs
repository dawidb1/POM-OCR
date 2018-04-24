using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using POM_OCR.Models;
using IronOcr;
using Emgu.CV;
using Emgu.CV.Structure;
using Emgu.CV.CvEnum;

namespace POM_OCR.Controllers
{
    public class HomeController : Controller
    {
  
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult Add(Image imageModel)
        {
            string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            imageModel.ImagePath = "~/Image/" + fileName;
            fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            imageModel.ImageFile.SaveAs(fileName);

            HttpCookie imageCookie = new HttpCookie("ImageTestCookie");
            imageCookie.Value = imageModel.ImagePath;
            Response.Cookies.Add(imageCookie);


            return RedirectToAction("OpenImage","ImageProcessing");
            
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