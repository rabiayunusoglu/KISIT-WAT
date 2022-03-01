using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Net.Http.Headers;



namespace Constraint.ServiceLayer.Controllers
{
    public class ConstraintEntryController : Controller
    {
        ConstraintManager manager = new ConstraintManager();
        // GET: ConstraintEntry
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetManager()
        {
            var _list = manager.GetAllConstraints();
            var jsonResult = Json(_list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult Details(Guid id)
        {
            var _listManager = manager.GetConstraintById(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(ConstraintDTO managerDTO)
        {
            var value=manager.CreateConstraint(managerDTO);
            return Json(value, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Edit(ConstraintDTO managerDTO)
        {
            var _listManager = manager.UpdateConstraint(managerDTO);
            if (_listManager == null)
            {
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var _listManager = manager.DeleteConstraint(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }

        public JsonResult GetMaterial(string material)
        {
            List<ConstraintDTO> list = manager.GetAllConstraints();
            List<ConstraintDTO> byid = list.Where(x => x.materialCode == material).ToList();
            if (byid == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            return Json(byid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetNoMarkedList()
        {
            List<ConstraintDTO> list = manager.GetAllConstraints();
            List<ConstraintDTO> byid = list.Where(x => x.isMarked == false && x.isDelayEntered == true).ToList();
            if (byid == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            return Json(byid, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetMarkedList()
        {
            List<ConstraintDTO> list = manager.GetAllConstraints();
            List<ConstraintDTO> byid = list.Where(x => x.isMarked == true && x.isDelayEntered == true).ToList();
            if (byid == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            return Json(byid, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult EditList(ConstraintDTO[] managerDTO)
        {
            for (int i = 0; i < managerDTO.Length; i++)
            {
                var _listManager = manager.UpdateConstraint(managerDTO[i]);
                if (_listManager == null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TransferToMeeting(MeetingDTO[] managerDTO)
        {
            MeetingManager meetingManager = new MeetingManager();
            for (int i = 0; i < managerDTO.Length; i++)
            {
                var x = meetingManager.CreateMeeting(managerDTO[i]);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult TransferToMeetingTeam(MeetingTeamDTO[] managerTeamDTO)
        {
            MeetingTeamManager meetingTeamManager = new MeetingTeamManager();
            for (int i = 0; i < managerTeamDTO.Length; i++)
            {
                var t = meetingTeamManager.CreateMeetingTeam(managerTeamDTO[i]);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        public void Export(bool isMark)
        {
            try
            {

                byte[] temp = manager.ExporttoExcel(isMark);
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


            /*  try
              {
                  ConstraintManager constraint = new ConstraintManager();
                  byte[] temp = constraint.ExporttoExcel(isMark);
                  if (temp == null)
                      return null;
                  var filemem = new MemoryStream(temp);
                  HttpRequestMessage result =new HttpRequestMessage(null, "başarılı");
                  result.Content = new StreamContent(filemem);
                  result.Content.Headers.ContentType = new MediaTypeHeaderValue("application/vnd.ms-excel");
                  result.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
                  {
                      FileName = "Report.xlsx"
                  };
                  return result;

              }
              catch (Exception ex)
              {
                  return null;
              }*/

        }
        [HttpPost]
        public JsonResult DeleteAll(ConstraintDTO[] managerDTO)
        {
            for (int i = 0; i < managerDTO.Length; i++)
            {
                var _listManager = manager.DeleteConstraint(managerDTO[i].constraintID);
                if (!_listManager)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult CreateAll(ConstraintDTO[] managerDTO)
        {
            for (int i = 0; i < managerDTO.Length; i++)
            {
                var _listManager = manager.CreateConstraint(managerDTO[i]);
                if (_listManager == null)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }

            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult SaveAll(ConstraintDTO[] managerDTO)
        {
            if (managerDTO == null)
                return Json(false, JsonRequestBehavior.AllowGet);

            for (var i = 0; i < managerDTO.Length; i++)
            {
                if (managerDTO[i].delayDetail == null || managerDTO[i].delayDetail.Length == 0)
                    managerDTO[i].delayDetail = "-";
                DelayHistoryDTO dh = new DelayHistoryDTO();
                dh.productCode = managerDTO[i].productCode;
                dh.delayCode = managerDTO[i].delayCode;
                dh.delayAmount = managerDTO[i].delayAmount;
                dh.delayDate = managerDTO[i].delayDate;
                dh.delayReason = managerDTO[i].delayReason;
                dh.delayDetail = managerDTO[i].delayDetail;
                dh.companyTeam = managerDTO[i].companyTeam;
                dh.chargePerson = managerDTO[i].chargePerson;
                dh.madeDate = managerDTO[i].dateCurrent;
                dh.boundConstraintID = managerDTO[i].constraintID.ToString();
                var _listManager = manager.Save(managerDTO[i], dh);
                if (!_listManager)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }


            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}