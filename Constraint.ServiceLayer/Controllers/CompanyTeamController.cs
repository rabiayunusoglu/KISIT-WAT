using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;
namespace Constraint.ServiceLayer.Controllers
{
    public class CompanyTeamController : Controller
    {
        CompanyTeamManager manager = new CompanyTeamManager();

        // GET: CompanyTeam
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetManager()
        {
            var _list = manager.GetAllTeams();
            return Json(_list, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Details(Guid id)
        {
            var _listManager = manager.GetTeamById(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(CompanyTeamDTO managerDTO)
        {
            manager.CreateTeam(managerDTO);
            return Json(null);
        }
        [HttpPost]
        public JsonResult Edit(CompanyTeamDTO managerDTO)
        {
            var _listManager = manager.UpdateTeam(managerDTO);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var _listManager = manager.DeleteTeam(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
    }
}