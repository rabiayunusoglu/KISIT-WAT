using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;
namespace Constraint.ServiceLayer.Controllers
{
    public class DelayHistoryController : Controller
    {
        DelayHistoryManager manager = new DelayHistoryManager();
        // GET: DelayHistory
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetManager()
        {
            var _list = manager.GetAllHistories();
            var jsonResult = Json(_list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult Details(Guid id)
        {
            var _listManager = manager.GetHistoryById(id);
            if(_listManager==null)
                return Json(false, JsonRequestBehavior.AllowGet);
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(DelayHistoryDTO managerDTO)
        {
            //burayi silme bu delayi icerde kullaniyorum id'si gerek bana
           var delay= manager.Createhistory(managerDTO);
            return Json(delay);
        }
        [HttpPost]
        public JsonResult Edit(DelayHistoryDTO managerDTO)
        {
            var _listManager = manager.UpdateHistory(managerDTO);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var _listManager = manager.Deletehistory(id);
            if (_listManager)
                return Json(_listManager, JsonRequestBehavior.AllowGet);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditList(DelayHistoryDTO[] managerDTO)
        {
            for (int i = 0; i < managerDTO.Length; i++)
            {
                var _listManager = manager.UpdateHistory(managerDTO[i]);
                if (_listManager == null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
       
    }
}