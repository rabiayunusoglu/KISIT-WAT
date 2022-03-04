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
        
        public JsonResult GetManager()
        {
            var _listPersons = productTree.GetAllTrees();
            var jsonResult = Json(_listPersons, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;
           

        }
        [HttpPost]
        public JsonResult Upload(FormCollection formCollection)
        {
            ProductTreeManager tree = new ProductTreeManager(); 
            try
            {
                if (Request != null)
                {
                    HttpPostedFileBase file = Request.Files[0];
                    if ((file != null) && (file.ContentLength != 0) && !string.IsNullOrEmpty(file.FileName))
                    {
                        string fileName = file.FileName;
                        string fileContentType = file.ContentType;
                        byte[] fileBytes = new byte[file.ContentLength];
                        var FileStream = file.InputStream;
                        bool fileData = tree.GetDataFromExcelFile(file.FileName, FileStream);
                        if (fileData)
                            return Json(true, JsonRequestBehavior.AllowGet);
                        else
                            return Json(false, JsonRequestBehavior.AllowGet);
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