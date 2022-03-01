﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;
namespace Constraint.ServiceLayer.Controllers
{
    public class VersionController : Controller
    {
        VersionManager manager = new VersionManager();
        // GET: Version
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetManager()
        {
            var _list = manager.GetAllVersions();
            var jsonResult = Json(_list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult Details(Guid id)
        {
            var _listManager = manager.GetVersionById(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(VersionDTO managerDTO)
        {
            manager.CreateVersion(managerDTO);
            return Json(null);
        }
        [HttpPost]
        public JsonResult Edit(VersionDTO managerDTO)
        {
            var _listManager = manager.UpdateVersion(managerDTO);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var _listManager = manager.DeleteVersion(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
    }
}