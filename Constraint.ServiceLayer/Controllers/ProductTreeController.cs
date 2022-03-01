using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;
using System.IO;

namespace Constraint.ServiceLayer.Controllers
{
    [Authorize(Roles = "A")]

    public class ProductTreeController : Controller
    {
        ProductTreeManager productTree = new ProductTreeManager();
        // GET: ProductTree
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetManager()
        {
            var _listPersons = productTree.GetAllTrees();
            var jsonResult = Json(_listPersons, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
           

        }
        [HttpPost]
        public ActionResult Upload(FormCollection formCollection)
        {
            ProductTreeManager tree = new ProductTreeManager(); 
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files["UploadedFile"];
                    if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var FileStream = file.InputStream;
                        bool fileData = tree.GetDataFromExcelFile(file.FileName, FileStream);
                        if (fileData)
                            return RedirectToAction("ProductTree", "Home");
                        else
                            return RedirectToAction("ExcelTree", "Home");
                    }
                }

            }
            catch (Exception ex)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}