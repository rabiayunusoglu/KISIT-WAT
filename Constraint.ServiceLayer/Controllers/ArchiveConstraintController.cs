using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;

namespace Constraint.ServiceLayer.Controllers
{

    public class ArchiveConstraintController : Controller
    {
        ArchiveConstraintManager ConstraintManager = new ArchiveConstraintManager();
        // GET: ArchiveConstraint
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetConstraint()
        {
            var _listPersons = ConstraintManager.GetAllArchives();
            var jsonResult = Json(_listPersons, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult Details(Guid id)
        {
            var _person = ConstraintManager.GetArchiveById(id);
            return Json(_person, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(ArchiveConstraintDTO ConstraintManagerDTO)
        {
            ConstraintManager.CreateArchive(ConstraintManagerDTO);
            return Json(null);
        }
        [HttpPost]
        public JsonResult Edit(ArchiveConstraintDTO ConstraintManagerDTO)
        {
            var _person = ConstraintManager.UpdateArchive(ConstraintManagerDTO);
            return Json(_person, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var _person = ConstraintManager.DeleteArchive(id);
            return Json(_person, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateAll(ArchiveConstraintDTO[] managerDTO)
        {
            for (int i = 0; i < managerDTO.Length; i++)
            {
                var _listManager = ConstraintManager.CreateArchive(managerDTO[i]);
                if (_listManager == null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult DeleteAll(ArchiveConstraintDTO[] managerDTO)
        {
            DelayHistoryManager dh = new DelayHistoryManager();
            for (int i = 0; i < managerDTO.Length; i++)
            {
                var _listManager = ConstraintManager.DeleteArchive(managerDTO[i].constraintID);
                if (!_listManager)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
                var _delay = dh.Deletehistory(new Guid(managerDTO[i].delayID));
                if(!_delay)
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);


        }
        [HttpPost]
        public JsonResult SendToMarked(ConstraintDTO[] managerDTOCT,ArchiveConstraintDTO[] managerDTO)
        {
            ConstraintManager ct = new ConstraintManager();
            DelayHistoryManager dh = new DelayHistoryManager();
            for (int i = 0; i < managerDTO.Length; i++)
            {
                var _constraint = ct.CreateConstraint(managerDTOCT[i]);
                if (_constraint == null)
                    return Json(false, JsonRequestBehavior.AllowGet);
                DelayHistoryDTO d = new DelayHistoryDTO();
                d.isMarked = _constraint.isMarked;
                d.isArchive = false;
                d.delayID = new Guid(_constraint.delayID);
                d.productCode = _constraint.productCode;
                d.delayCode = _constraint.delayCode;
                d.delayAmount = _constraint.delayAmount;
                d.delayDate = _constraint.delayDate;
                d.delayReason = _constraint.delayReason;
                d.delayDetail = _constraint.delayDetail;
                d.companyTeam = _constraint.companyTeam;
                d.chargePerson = _constraint.chargePerson;
                d.madeDate = _constraint.dateCurrent;
                d.boundConstraintID = _constraint.constraintID.ToString();
                var _delay = dh.UpdateHistory(d);
                if (_delay == null)
                    return Json(false, JsonRequestBehavior.AllowGet);
                var _listManager = ConstraintManager.DeleteArchive(managerDTO[i].constraintID);
                if (!_listManager)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);


        }
        public void Export()
        {
            try
            {

                byte[] temp = ConstraintManager.ExporttoExcel();
                if (temp == null)
                    return;
                Response.ClearContent();
                Response.BinaryWrite(temp);
                Response.AddHeader("content-disposition", "attachment; filename=Rabia.xlsx");
                Response.ContentType = "application/vnd.ms-excel";
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                return;
            }

        }
        public void ExportByDate(DateTime startDate, DateTime endDate)
        {
            try
            {

                byte[] temp = ConstraintManager.ExporttoExcelByDate(startDate, endDate);
                if (temp == null)
                    return;
                Response.ClearContent();
                Response.BinaryWrite(temp);
                Response.AddHeader("content-disposition", "attachment; filename=Rabia.xlsx");
                Response.ContentType = "application/vnd.ms-excel";
                Response.Flush();
                Response.End();
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public JsonResult GetByDate(DateTime startDate, DateTime endDate)
        {
            List<ArchiveConstraintDTO> list = ConstraintManager.GetAllArchives();
            List<ArchiveConstraintDTO> archiveByid = list.Where(x => Convert.ToDateTime(x.dateCurrent) <= endDate && Convert.ToDateTime(x.dateCurrent) >= startDate).ToList();
            if (archiveByid == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            return Json(archiveByid, JsonRequestBehavior.AllowGet);

        }
    }
}