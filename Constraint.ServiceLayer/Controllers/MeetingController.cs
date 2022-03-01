using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;
namespace Constraint.ServiceLayer.Controllers
{
    public class MeetingController : Controller
    {
        MeetingManager manager = new MeetingManager();
        // GET: Meeting
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetManager()
        {
            var _list = manager.GetAllMeetings();
            var jsonResult = Json(_list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult Details(Guid id)
        {
            var _listManager = manager.GetMeetingById(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(MeetingDTO managerDTO)
        {
            var man = manager.CreateMeeting(managerDTO);
            if (man == null)
                return Json(false);
            return Json(true);
        }
        [HttpPost]
        public JsonResult Edit(MeetingDTO managerDTO)
        {
            var _listManager = manager.UpdateMeeting(managerDTO);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var _listManager = manager.DeleteMeeting(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetByDate(DateTime startDate, DateTime endDate)
        {
            List<MeetingDTO> list = manager.GetAllMeetings();
            List<MeetingDTO> archiveByid = list.Where(x => Convert.ToDateTime(x.dateCurrent) <= endDate && Convert.ToDateTime(x.dateCurrent) >= startDate).ToList();
            if (archiveByid == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            return Json(archiveByid, JsonRequestBehavior.AllowGet);

        }
        public void Export()
        {
            try
            {

                byte[] temp = manager.ExporttoExcel();
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

                byte[] temp = manager.ExporttoExcelByDate(startDate, endDate);
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
    }
}