using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using EnigmaWeb.Models;
using Improved_Enigma.DataPreprocessing;
using Improved_Enigma.Network;


namespace EnigmaWeb.Controllers
{
    public class HomeController : Controller
    {
        // GET: Home
        //[Authorize]
        public ActionResult Index()
        {
            IndexModel indexModel = new IndexModel();
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"));
            string[] fileNames = Directory.GetFiles(path);
            foreach (string filePath in fileNames)
            {
                UploadedFile file = new UploadedFile();
                file.Name = Path.GetFileName(filePath);
                file.Location = filePath;
                indexModel.UploadsList.Add(file);
            }
            return View(indexModel);
        }

        public ActionResult Download()
        {
            string name = "ready.xlsx";
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads/"));
            var data = System.IO.File.ReadAllBytes(path + name);
            var cd = new System.Net.Mime.ContentDisposition
            {
                // for example foo.bak
                FileName = name,

                // always prompt the user for downloading, set to true if you want 
                // the browser to try to show the file inline
                Inline = false,
            };
            Response.AppendHeader("Content-Disposition", cd.ToString());

            return File(data, name + ".xlsx");
        }

        [HttpPost]
        public ActionResult ExecuteAlgorithms()
        {
            var path = Path.Combine(Server.MapPath("~/App_Data/uploads"));

            Data d = new Data(path, "/original");
            Algorithms.RemoveEmptyColumns(d.AllDatax);
            Algorithms.RemoveSameValueColumns(d.AllDatax);
            Algorithms.RemoveLowVarianceColumns(d.AllDatax, 8);

            
            ExportToExcel.Export(d.AllDatax, path + "/ready");

            return RedirectToAction("Index");
        }

        // This action handles the form POST and the upload
        [HttpPost]
        public ActionResult Index(HttpPostedFileBase file)
        {
            // Verify that the user selected a file
            if (file != null && file.ContentLength > 0)
            {
                // extract only the filename
                var fileName = Path.GetFileName(file.FileName);
                // store the file inside ~/App_Data/uploads folder
                var path = Path.Combine(Server.MapPath("~/App_Data/uploads"), "original.csv");
                file.SaveAs(path);
            }
            // redirect back to the index action to show the form once again
            return RedirectToAction("Index");
        }
    }
}