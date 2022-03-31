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
        [Authorize(Roles = "A,U")]
        public JsonResult GetConstraint()
        {
            var _listPersons = ConstraintManager.GetAllArchives();
            if (_listPersons == null)
                return Json(_listPersons, JsonRequestBehavior.AllowGet);
            var jsonResult = Json(_listPersons, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize(Roles = "A")]
        public JsonResult Details(Guid id)
        {
            if (id == null)
                return Json(null, JsonRequestBehavior.AllowGet);
            var _person = ConstraintManager.GetArchiveById(id);
            return Json(_person, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public JsonResult Create(ArchiveConstraintDTO ConstraintManagerDTO)
        {
            if (ConstraintManagerDTO == null)
                return null;
            var _manager = ConstraintManager.CreateArchive(ConstraintManagerDTO);
            if (_manager != null)
                return Json(_manager, JsonRequestBehavior.AllowGet);
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public JsonResult Edit(ArchiveConstraintDTO ConstraintManagerDTO)
        {
            if (ConstraintManagerDTO == null)
                return null;
            var _manager = ConstraintManager.UpdateArchive(ConstraintManagerDTO);
            if (_manager != null)
                return Json(_manager, JsonRequestBehavior.AllowGet);
            return Json(null, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public JsonResult Delete(Guid id)
        {
            var _person = ConstraintManager.DeleteArchive(id);
            return Json(_person, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public JsonResult CreateAll(ArchiveConstraintDTO[] managerDTO)
        {
            if(managerDTO==null || managerDTO.Length==0)
                return Json(false, JsonRequestBehavior.AllowGet);
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
       
        [Authorize(Roles = "A")]
        public JsonResult DeleteAll()
        {
            DelayHistoryManager df = new DelayHistoryManager();
            var _delay = df.DeleteAllArchiveHistories();
            if (!_delay)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            var _listManager = ConstraintManager.DeleteAllArchive();
            if (!_listManager)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            
            return Json(true, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public JsonResult SendToMarked(ConstraintDTO[] managerDTOCT, ArchiveConstraintDTO[] managerDTO)
        {
            if (managerDTO == null || managerDTO.Length == 0)
                return Json(false, JsonRequestBehavior.AllowGet);
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
                d.boundMontageID = _constraint.boundMontageID;
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
        [Authorize(Roles = "A,U")]
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
        [Authorize(Roles = "A,U")]
        public void ExportByDate(DateTime startDate, DateTime endDate)
        {
            try
            {
                if (startDate == null || endDate == null)
                    return;
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
        [Authorize(Roles = "A,U")]
        public JsonResult GetByDate(DateTime startDate, DateTime endDate)
        {
            if (startDate == null || endDate == null)
                Json(null, JsonRequestBehavior.AllowGet);
            List<ArchiveConstraintDTO> list = ConstraintManager.GetAllArchives();
            if(list==null || list.Count==0)
                Json(null, JsonRequestBehavior.AllowGet);
            List<ArchiveConstraintDTO> archiveByid = list.Where(x => Convert.ToDateTime(x.dateCurrent) <= endDate && Convert.ToDateTime(x.dateCurrent) >= startDate).ToList();
            if (archiveByid == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            var jsonResult = Json(archiveByid, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
    }
}