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

    public class MontagePlanController : Controller
    {
        MontageProductPlanManager montage = new MontageProductPlanManager();
        // GET: MontagePlan
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetManager()
        {
            var _listPersons = montage.GetAllPlans();
            return Json(_listPersons, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public ActionResult Upload(FormCollection formCollection)
        {
            MontageProductPlanManager plan = new MontageProductPlanManager();
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
                        bool fileData = plan.GetDataFromExcelFile(file.FileName, FileStream);
                        if (fileData)
                            return RedirectToAction("MontagePlan","Home");
                        else
                            return RedirectToAction("ExcelMontage", "Home");
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