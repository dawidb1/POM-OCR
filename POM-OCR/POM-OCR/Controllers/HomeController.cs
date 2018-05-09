using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Caching;
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
        public ActionResult Add(HttpPostedFileBase file)
        {
            //string fileName = Path.GetFileNameWithoutExtension(imageModel.ImageFile.FileName);
            //string extension = Path.GetExtension(imageModel.ImageFile.FileName);
            //fileName = fileName + DateTime.Now.ToString("yymmssfff") + extension;
            //imageModel.ImagePath = "~/Image/" + fileName;

            //fileName = Path.Combine(Server.MapPath("~/Image/"), fileName);
            //imageModel.ImageFile.SaveAs(fileName);


       

            //var fileKey = Guid.NewGuid();
            //var fileStream = new Byte[Request.Files[0].ContentLength];
            //Request.Files[0].InputStream.Read(fileStream, 0, Request.Files[0].ContentLength);
            //Cache[fileKey.ToString()] = fileStream;
            //string FolderPath = System.Configuration.ConfigurationManager.AppSettings["PATH"].ToString();
            string serverPath = @"http://textdetection.azurewebsites.net/";
            string fileName = file.FileName;
            string assoid = "dawid";
      
            string imagePath = serverPath + assoid;
            file.SaveAs(imagePath);

            HttpCookie imageCookie = new HttpCookie("ImageTestCookie");
            imageCookie.Value = imagePath;
            Response.Cookies.Add(imageCookie);

            //face_crop_original.Src = imagePath + "?" + DateTime.Now;


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