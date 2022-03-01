using ClosedXML.Excel;
using Constraint.BusinessLayer.DTO;
using Constraint.BusinessLayer.IManagers;
using Constraint.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.Managers
{
    public class MeetingTeamManager : IMeetingTeamManager
    {
        private UnitOfWork _unitOfWork;
        public MeetingTeamManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public MeetingTeamDTO CreateMeetingTeam(MeetingTeamDTO meetingValue)
        {
            MeetingTeam newMeeting = new MeetingTeam();
            bool temp = false;
            if (meetingValue == null)
            {
                return null;
            }

            CompanyTeamManager team = new CompanyTeamManager();
            List<CompanyTeamDTO> onlyTeams = team.GetOnlyTeams();
            if (onlyTeams == null)
            {
                return null;
            }
            temp = onlyTeams.Any(t => t.companyName.ToLower().Trim().Equals(meetingValue.companyTeam.ToLower().Trim()));
            if (temp)
            {

                newMeeting.constraintID = System.Guid.NewGuid();
                newMeeting.materialCode = meetingValue.materialCode;
                newMeeting.materialText = meetingValue.materialText;
                newMeeting.productCode = meetingValue.productCode;
                newMeeting.plannedDate = meetingValue.plannedDate;
                newMeeting.amount = meetingValue.amount;
                newMeeting.customer = meetingValue.customer;
                newMeeting.version = meetingValue.version;
                newMeeting.delayID = meetingValue.delayID;
                newMeeting.delayCode = meetingValue.delayCode;
                newMeeting.delayAmount = meetingValue.delayAmount;
                newMeeting.delayDate = meetingValue.delayDate;
                newMeeting.delayReason = meetingValue.delayReason;
                newMeeting.delayDetail = meetingValue.delayDetail;
                newMeeting.companyTeam = meetingValue.companyTeam;
                newMeeting.chargePerson = meetingValue.chargePerson;
                newMeeting.dateCurrent = meetingValue.dateCurrent;

                MeetingTeam recordValue = _unitOfWork.MeetingTeamRepository.Add(newMeeting);
                if (_unitOfWork.Complete() > 0)
                {
                    return null;
                }
            }
            return null;
        }

        public bool DeleteMeetingTeam(Guid id)
        {
            if (_unitOfWork.MeetingTeamRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                {
                    return true;
                }
               
            }
            return false;
        }

        public List<MeetingTeamDTO> GetAllMeetingTeams()
        {
            List<MeetingTeam> list = _unitOfWork.MeetingTeamRepository.GetAll().OrderBy(x => x.plannedDate).ToList();

            if (list == null || list.Count == 0)
                return null;
            list = list.OrderBy(x => x.dateCurrent).ToList();
            List<MeetingTeamDTO> dtoList = new List<MeetingTeamDTO>();
            foreach (MeetingTeam recordValue in list)
            {
                MeetingTeamDTO returnValue = new MeetingTeamDTO()
                {
                    constraintID = recordValue.constraintID,
                    materialCode = recordValue.materialCode,
                    materialText = recordValue.materialText,
                    productCode = recordValue.productCode,
                    plannedDate = recordValue.plannedDate,
                    amount = recordValue.amount,
                    customer = recordValue.customer,
                    version = recordValue.version,
                    delayID = recordValue.delayID,
                    delayCode = recordValue.delayCode,
                    delayAmount = recordValue.delayAmount,
                    delayDate = recordValue.delayDate,
                    delayReason = recordValue.delayReason,
                    delayDetail = recordValue.delayDetail,
                    companyTeam = recordValue.companyTeam,
                    chargePerson = recordValue.chargePerson,
                    dateCurrent = recordValue.dateCurrent,

                };
                dtoList.Add(returnValue);
            }
            return dtoList;
        }

        public MeetingTeamDTO GetMeetingTeamById(Guid id)
        {
            MeetingTeam recordValue = _unitOfWork.MeetingTeamRepository.GetById(id);

            MeetingTeamDTO returnValue = new MeetingTeamDTO()
            {
                constraintID = recordValue.constraintID,
                materialCode = recordValue.materialCode,
                materialText = recordValue.materialText,
                productCode = recordValue.productCode,
                plannedDate = recordValue.plannedDate,
                amount = recordValue.amount,
                customer = recordValue.customer,
                version = recordValue.version,
                delayID = recordValue.delayID,
                delayCode = recordValue.delayCode,
                delayAmount = recordValue.delayAmount,
                delayDate = recordValue.delayDate,
                delayReason = recordValue.delayReason,
                delayDetail = recordValue.delayDetail,
                companyTeam = recordValue.companyTeam,
                chargePerson = recordValue.chargePerson,
                dateCurrent = recordValue.dateCurrent,

            };
            return returnValue;
        }


        public MeetingTeamDTO UpdateMeetingTeam(MeetingTeamDTO meetingValue)
        {
            if (meetingValue == null)
            {
                return null;
            }
            MeetingTeam newMeeting = new MeetingTeam();


            newMeeting.constraintID = meetingValue.constraintID;
            newMeeting.materialCode = meetingValue.materialCode;
            newMeeting.materialText = meetingValue.materialText;
            newMeeting.productCode = meetingValue.productCode;
            newMeeting.plannedDate = meetingValue.plannedDate;
            newMeeting.amount = meetingValue.amount;
            newMeeting.customer = meetingValue.customer;
            newMeeting.version = meetingValue.version;
            newMeeting.delayID = meetingValue.delayID;
            newMeeting.delayCode = meetingValue.delayCode;
            newMeeting.delayAmount = meetingValue.delayAmount;
            newMeeting.delayDate = meetingValue.delayDate;
            newMeeting.delayReason = meetingValue.delayReason;
            newMeeting.delayDetail = meetingValue.delayDetail;
            newMeeting.companyTeam = meetingValue.companyTeam;
            newMeeting.chargePerson = meetingValue.chargePerson;
            newMeeting.dateCurrent = meetingValue.dateCurrent;

            MeetingTeam recordValue = _unitOfWork.MeetingTeamRepository.Update(newMeeting);
            
            MeetingTeamDTO returnValue = new MeetingTeamDTO()
            {
                constraintID = recordValue.constraintID,
                materialCode = recordValue.materialCode,
                materialText = recordValue.materialText,
                productCode = recordValue.productCode,
                plannedDate = recordValue.plannedDate,
                amount = recordValue.amount,
                customer = recordValue.customer,
                version = recordValue.version,
                delayID = recordValue.delayID,
                delayCode = recordValue.delayCode,
                delayAmount = recordValue.delayAmount,
                delayDate = recordValue.delayDate,
                delayReason = recordValue.delayReason,
                delayDetail = recordValue.delayDetail,
                companyTeam = recordValue.companyTeam,
                chargePerson = recordValue.chargePerson,
                dateCurrent = recordValue.dateCurrent,
            };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }

        public List<TeamNameDTO> GetTeamNamesAndSum()
        {
            List<TeamNameDTO> list = GetAllMeetingTeams().GroupBy(t => t.companyTeam).OrderBy(y => y.Key).Select(s => new TeamNameDTO { teamName = s.Key, sumAmount = s.Sum(i => Convert.ToInt32(i.delayAmount)) }).ToList();
            return list;
        }
        public List<TeamReasonDTO> GetTeamReasonsAndSum()
        {
            List<TeamReasonDTO> list = GetAllMeetingTeams().GroupBy(t => new { t.companyTeam, t.delayReason }).OrderBy(y => y.Key.delayReason).Select(s => new TeamReasonDTO { teamName = s.Key.companyTeam, delayReason = s.Key.delayReason, sumAmount = s.Sum(i => Convert.ToInt32(i.delayAmount)) }).ToList();
            return list;
        }

        public List<TeamNameDTO> GetTeamNamesAndSumByDate(DateTime startDate, DateTime endDate)
        {
            List<TeamNameDTO> list = GetAllMeetingTeams().Where(x => Convert.ToDateTime(x.dateCurrent) <= endDate && Convert.ToDateTime(x.dateCurrent) >= startDate).
                GroupBy(t => t.companyTeam).OrderBy(y => y.Key).Select(s => new TeamNameDTO { teamName = s.Key, sumAmount = s.Sum(i => Convert.ToInt32(i.delayAmount)) }).ToList();
            return list;
        }
        public List<TeamReasonDTO> GetTeamReasonsAndSumByDate(DateTime startDate, DateTime endDate)
        {
            List<TeamReasonDTO> list = GetAllMeetingTeams().Where(x => Convert.ToDateTime(x.dateCurrent) <= endDate && Convert.ToDateTime(x.dateCurrent) >= startDate).GroupBy(t => new { t.companyTeam, t.delayReason }).OrderBy(y => y.Key.delayReason).Select(s => new TeamReasonDTO { teamName = s.Key.companyTeam, delayReason = s.Key.delayReason, sumAmount = s.Sum(i => Convert.ToInt32(i.delayAmount)) }).ToList();
            return list;
        }
        public byte[] ExporttoExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("EKİP");

                //ws.ColumnWidth = 25;
                ws.Columns("A").Width = 25;
                ws.Columns("B").Width = 25;
                //header
                ws.Cell(1, 1).Value = "Ekipler";
                ws.Cell(1, 2).Value = "Değişiklik yapılan ürün adeti";
                ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightGray;
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 2).Style.Font.Bold = true;
                //connect to sql here
                System.Data.DataTable dt = new System.Data.DataTable();
                List<TeamNameDTO> list = GetTeamNamesAndSum();
                List<TeamReasonDTO> listReason = GetTeamReasonsAndSum();

                int i = 2;
                int j = 1;
                foreach (TeamNameDTO item in list)
                {
                    ws.Cell(i, 1).Value = Convert.ToString(item.teamName).TrimEnd();
                    ws.Cell(i, 2).Value = Convert.ToString(item.sumAmount);
                    if (j % 2 == 1)
                    {
                        ws.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.NavajoWhite;
                        ws.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.NavajoWhite;
                    }
                    else
                    {
                        ws.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                        ws.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                    }
                    ws.Cell(i, 1).Style.Font.Bold = true;
                    ws.Cell(i, 2).Style.Font.Bold = true;
                    i++;
                    List<TeamReasonDTO> itemList = listReason.Where(x => x.teamName.Equals(item.teamName)).ToList();
                    foreach (TeamReasonDTO itemReason in itemList)
                    {
                        ws.Cell(i, 1).Value = Convert.ToString(itemReason.delayReason).TrimEnd();
                        ws.Cell(i, 2).Value = Convert.ToString(itemReason.sumAmount);
                        if (j % 2 == 1)
                        {
                            //ws.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.PapayaWhip;
                            //ws.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.PapayaWhip;
                            ws.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.NavajoWhite;
                            ws.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.NavajoWhite;
                        }
                        else
                        {
                            ws.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                            ws.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                        }

                        i++;
                    }
                    j++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] content = stream.ToArray();
                    return content;
                    //File(content,"application/vnd.openxmlformats-officedocument-spreadsheetml.sheet","IsaretlenmisKisit.xlsx");
                }
                return null;
            }
        }
        public byte[] ExporttoExcelByDate(DateTime startDate, DateTime endDate)
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("EKİP");

                //ws.ColumnWidth = 25;
                ws.Columns("A").Width = 25;
                ws.Columns("B").Width = 25;
                //header
                ws.Cell(1, 1).Value = "Ekipler";
                ws.Cell(1, 2).Value = "Değişiklik yapılan ürün adeti";
                ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.LightGray;
                ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.LightGray;
                ws.Cell(1, 1).Style.Font.Bold = true;
                ws.Cell(1, 2).Style.Font.Bold = true; ;
                //connect to sql here
                System.Data.DataTable dt = new System.Data.DataTable();
                List<TeamNameDTO> list = GetTeamNamesAndSumByDate(startDate,endDate);
                List<TeamReasonDTO> listReason = GetTeamReasonsAndSumByDate(startDate,endDate);

                int i = 2;
                int j = 1;
                foreach (TeamNameDTO item in list)
                {
                    ws.Cell(i, 1).Value = Convert.ToString(item.teamName).TrimEnd();
                    ws.Cell(i, 2).Value = Convert.ToString(item.sumAmount);

                    if (j % 2 == 1)
                    {
                        ws.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.NavajoWhite;
                        ws.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.NavajoWhite;
                    }
                    else
                    {
                        ws.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                        ws.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                    }
                    ws.Cell(i, 1).Style.Font.Bold = true;
                    ws.Cell(i, 2).Style.Font.Bold = true;
                    i++;
                    List<TeamReasonDTO> itemList = listReason.Where(x => x.teamName.Equals(item.teamName)).ToList();
                    foreach (TeamReasonDTO itemReason in itemList)
                    {
                        ws.Cell(i, 1).Value = Convert.ToString(itemReason.delayReason).TrimEnd();
                        ws.Cell(i, 2).Value = Convert.ToString(itemReason.sumAmount);
                        if (j % 2 == 1)
                        {
                            //ws.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.PapayaWhip;
                            //ws.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.PapayaWhip;
                            ws.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.NavajoWhite;
                            ws.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.NavajoWhite;
                        }
                        else
                        {
                            ws.Cell(i, 1).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                            ws.Cell(i, 2).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                        }

                        i++;
                    }
                    j++;
                }
                using (var stream = new MemoryStream())
                {
                    workbook.SaveAs(stream);
                    byte[] content = stream.ToArray();
                    return content;
                    //File(content,"application/vnd.openxmlformats-officedocument-spreadsheetml.sheet","IsaretlenmisKisit.xlsx");
                }
                return null;
            }
        }
    }
}
