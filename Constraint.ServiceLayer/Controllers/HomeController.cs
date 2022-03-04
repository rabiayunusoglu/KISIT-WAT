using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Constraint.ServiceLayer.Controllers
{
    [Authorize(Roles="A")]
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ArchiveConstraint()
        {
            return View();
        }

        public ActionResult CompanyTeam()
        {
            return View();
        }
        public ActionResult ConstraintEntry()
        {
            return View();
        }
        public ActionResult ConstraintMarked()
        {
            return View();
        }
        public ActionResult ConstraintNoMarked()
        {
            return View();
        }
        public ActionResult ExcelMontage()
        {
            return View();
        }
        public ActionResult ExcelTree()
        {
            return View();
        }
        public ActionResult Meeting()
        {
            return View();
        }
        public ActionResult MeetingTeam()
        {
            return View();
        }
        public ActionResult MontagePlan()
        {
            return View();
        }
        public ActionResult PostponementReason()
        {
            return View();
        }
        public ActionResult ProductTree()
        {
            return View();
        }
        public ActionResult Register()
        {
            return View();
        }
        public ActionResult ReportTeam()
        {
            return View();
        }
        public ActionResult UserList()
        {
            return View();
        }
        public ActionResult Version()
        {
            return View();
        }
        public ActionResult ZKP3()
        {
            return View();
        }
    }
}