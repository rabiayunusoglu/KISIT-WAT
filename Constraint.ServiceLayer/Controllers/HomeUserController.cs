using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Constraint.ServiceLayer.Controllers
{
    [Authorize(Roles = "U")]

    public class HomeUserController : Controller
    {
        // GET: HomeUser
        public ActionResult Index()
        {
            return View();
        }
        public ActionResult ConstraintEntry()
        {
            return View();
        }
        public ActionResult ArchiveConstraint()
        {
            return View();
        }
    }
}