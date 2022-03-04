using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;
namespace Constraint.ServiceLayer.Controllers
{
    public class PostponementReasonController : Controller
    {
        PostponementReasonManager manager = new PostponementReasonManager();
        [Authorize(Roles = "A,U")]
        public JsonResult GetManager()
        {
            var _list = manager.GetAllReasons();
            var jsonResult = Json(_list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize(Roles = "A")]
        public JsonResult Details(Guid id)
        {
            var _listManager = manager.GetReasonById(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public JsonResult Create(PostponementReasonDTO managerDTO)
        {
            manager.CreateReason(managerDTO);
            return Json(null);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public JsonResult Edit(PostponementReasonDTO managerDTO)
        {
            var _listManager = manager.UpdateReason(managerDTO);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public JsonResult Delete(Guid id)
        {
            var _listManager = manager.DeleteReason(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
    }
}