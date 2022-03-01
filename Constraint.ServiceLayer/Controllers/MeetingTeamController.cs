using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;
namespace Constraint.ServiceLayer.Controllers
{
    public class MeetingTeamController : Controller
    {
        MeetingTeamManager manager = new MeetingTeamManager();
        // GET: MeetingTeam
        public ActionResult Index()
        {
            return View();
        }
        public JsonResult GetManager()
        {
            var _list = manager.GetAllMeetingTeams();
            var jsonResult = Json(_list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        public JsonResult Details(Guid id)
        {
            var _listManager = manager.GetMeetingTeamById(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Create(MeetingTeamDTO managerDTO)
        {
            manager.CreateMeetingTeam(managerDTO);
            return Json(null);
        }
        [HttpPost]
        public JsonResult Edit(MeetingTeamDTO managerDTO)
        {
            var _listManager = manager.UpdateMeetingTeam(managerDTO);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Delete(Guid id)
        {
            var _listManager = manager.DeleteMeetingTeam(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        public JsonResult GetByDate(DateTime startDate, DateTime endDate)
        {
            List<MeetingTeamDTO> list = manager.GetAllMeetingTeams();
            if(list==null)
                return Json(null, JsonRequestBehavior.AllowGet);
            List<MeetingTeamDTO> archiveByid = list.Where(x => Convert.ToDateTime(x.dateCurrent) <= endDate && Convert.ToDateTime(x.dateCurrent) >= startDate).ToList();
            if (archiveByid == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            return Json(archiveByid, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetByDateTeam(DateTime startDate, DateTime endDate)
        {
            var list = manager.GetTeamNamesAndSumByDate(startDate, endDate);
            if (list == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            return Json(list, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetByDateReason(DateTime startDate, DateTime endDate)
        {
            var list = manager.GetTeamReasonsAndSumByDate(startDate, endDate);
            if (list == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            return Json(list, JsonRequestBehavior.AllowGet);

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
        public JsonResult GetByTeam()
        {
            var teamNameList = manager.GetTeamNamesAndSum();
            if (teamNameList == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            return Json(teamNameList, JsonRequestBehavior.AllowGet);

        }
        public JsonResult GetByReason()
        {
            var teamNameList = manager.GetTeamReasonsAndSum();
            if (teamNameList == null)
            {
                return Json(null, JsonRequestBehavior.AllowGet);

            }
            return Json(teamNameList, JsonRequestBehavior.AllowGet);

        }
        [HttpPost]
        public JsonResult DeleteAll(MeetingTeamDTO[] managerDTO)
        {
            for (int i = 0; i < managerDTO.Length; i++)
            {
                var _listManager = manager.DeleteMeetingTeam(managerDTO[i].constraintID);
                if (!_listManager)
                {
                    return Json(false, JsonRequestBehavior.AllowGet);
                }
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
    }
}