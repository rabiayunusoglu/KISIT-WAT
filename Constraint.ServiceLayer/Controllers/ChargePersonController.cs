using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;

namespace Constraint.ServiceLayer.Controllers
{
    public class ChargePersonController : Controller
    {
        ChargePersonManager chargePerson = new ChargePersonManager();
        // GET: ChargePerson
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetManager()
        {
            var _listPersons = chargePerson.GetAllPersons();
            return Json(_listPersons, JsonRequestBehavior.AllowGet);

        }
        public JsonResult Details(Guid id)
        {
            var _person = chargePerson.GetPersonById(id);
            return Json(_person, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(ChargePersonDTO chargePersonDTO)
        {
            var val = chargePerson.CreatePerson(chargePersonDTO);
            if (val != null)
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Edit(ChargePersonDTO chargePersonDTO)
        {
            var _person = chargePerson.UpdatePerson(chargePersonDTO);
            if (_person != null)
                return Json(_person, JsonRequestBehavior.AllowGet);
            return Json(null, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var _person = chargePerson.DeletePerson(id);
            return Json(_person, JsonRequestBehavior.AllowGet);
        }
    }
}