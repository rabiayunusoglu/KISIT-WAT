using ClosedXML.Excel;
using Constraint.BusinessLayer.DTO;
using Constraint.BusinessLayer.IManagers;
using Constraint.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.Managers
{
    public class MeetingManager : IMeetingManager
    {
        private UnitOfWork _unitOfWork;
        public MeetingManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public MeetingDTO CreateMeeting(MeetingDTO Meeting)
        {
            Meeting newMeeting = new Meeting();
            if (Meeting == null)
            {
                return null;
            }
           
            CompanyTeamManager team = new CompanyTeamManager();
            List<CompanyTeamDTO> subIndustryTeams = team.GetHaveCodeTeams();
            var c = subIndustryTeams.Where(t => t.companyName.Equals(Meeting.companyTeam)).ToList();
            //Console.WriteLine(c);
            if (c != null && c.Count != 0)
            {
                CompanyTeamDTO t = c.First();
                newMeeting.constraintID = System.Guid.NewGuid();
                newMeeting.materialCode = Meeting.materialCode;
                newMeeting.materialText = Meeting.materialText;
                newMeeting.productCode = Meeting.productCode;
                newMeeting.plannedDate = Meeting.plannedDate;
                newMeeting.amount = Meeting.amount;
                newMeeting.customer = Meeting.customer;
                newMeeting.version = Meeting.version;
                newMeeting.delayID = Meeting.delayID;
                newMeeting.delayCode = Meeting.delayCode;
                newMeeting.delayAmount = Meeting.delayAmount;
                newMeeting.delayDate = Meeting.delayDate;
                newMeeting.delayReason = Meeting.delayReason;
                newMeeting.delayDetail = Meeting.delayDetail;
                newMeeting.companyTeam = Meeting.companyTeam;
                newMeeting.chargePerson = Meeting.chargePerson;
                newMeeting.dateCurrent = Meeting.dateCurrent;
                
                newMeeting.changedAmount = 1;
                newMeeting.companyTeamCode = t.companyCode;

                Meeting recordValue = _unitOfWork.MeetingRepository.Add(newMeeting);
                
                MeetingDTO returnValue = new MeetingDTO()
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
                    
                    changedAmount = recordValue.changedAmount,
                    companyTeamCode = recordValue.companyTeamCode,

                };
                if (_unitOfWork.Complete() > 0)
                {
                    return returnValue;
                }
               
            }
            return null;
        }

        public bool DeleteMeeting(System.Guid id)
        {
            if (_unitOfWork.MeetingRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                {
                    return true;
                }
               
            }
            return false;
        }

        public List<MeetingDTO> GetAllMeetings()
        {
            List<Meeting> list = _unitOfWork.MeetingRepository.GetAll().OrderBy(x => x.plannedDate).ToList();
            if (list == null || list.Count == 0)
                return null;
            list=list.OrderBy(x => x.dateCurrent).ToList();

            List<MeetingDTO> dtoList = new List<MeetingDTO>();
            if (list == null)
            {
                return null;
            }
            foreach (Meeting recordValue in list)
            {
                MeetingDTO returnValue = new MeetingDTO()
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
                  
                    changedAmount = recordValue.changedAmount,
                    companyTeamCode = recordValue.companyTeamCode,

                };
                dtoList.Add(returnValue);
            }
            return dtoList;
        }

        public MeetingDTO GetMeetingById(System.Guid id)
        {
            Meeting recordValue = _unitOfWork.MeetingRepository.GetById(id);

            MeetingDTO returnValue = new MeetingDTO()
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
               
                changedAmount = recordValue.changedAmount,
                companyTeamCode = recordValue.companyTeamCode,

            };
            return returnValue;
        }

        public MeetingDTO UpdateMeeting(MeetingDTO Meeting)
        {
            if (Meeting == null)
            {
                return null;
            }
            Meeting newMeeting = new Meeting();

            newMeeting.constraintID = Meeting.constraintID;
            newMeeting.materialCode = Meeting.materialCode;
            newMeeting.materialText = Meeting.materialText;
            newMeeting.productCode = Meeting.productCode;
            newMeeting.plannedDate = Meeting.plannedDate;
            newMeeting.amount = Meeting.amount;
            newMeeting.customer = Meeting.customer;
            newMeeting.version = Meeting.version;
            newMeeting.delayID = Meeting.delayID;
            newMeeting.delayCode = Meeting.delayCode;
            newMeeting.delayAmount = Meeting.delayAmount;
            newMeeting.delayDate = Meeting.delayDate;
            newMeeting.delayReason = Meeting.delayReason;
            newMeeting.delayDetail = Meeting.delayDetail;
            newMeeting.companyTeam = Meeting.companyTeam;
            newMeeting.chargePerson = Meeting.chargePerson;
            newMeeting.dateCurrent = Meeting.dateCurrent;
           
            newMeeting.changedAmount = Meeting.changedAmount;
            newMeeting.companyTeamCode = Meeting.companyTeamCode;

            Meeting recordValue = _unitOfWork.MeetingRepository.Update(newMeeting);
            
            MeetingDTO returnValue = new MeetingDTO()
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
                
                changedAmount = recordValue.changedAmount,
                companyTeamCode = recordValue.companyTeamCode,

            };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }

        /*public bool DeleteAllMeeting(List<MeetingDTO> meetings)
        {
            if (meetings != null)
            {
                foreach (MeetingDTO t in meetings)
                {
                    DeleteMeeting(t.constraintID);
                }
                return true;
            }
            return false;
        }
        public List<MeetingDTO> GetSubIndustry()
        {
            ConstraintManager constraint = new ConstraintManager();
            CompanyTeamManager team = new CompanyTeamManager();
            List<ConstraintDTO> markedConstraint = constraint.GetMarkedConstraints();
            List<CompanyTeamDTO> subIndustryTeams = team.GetHaveCodeTeams();
            if (markedConstraint != null && markedConstraint.Count != 0)
            {
                DeleteAllMeeting(GetAllMeetings());
                foreach (ConstraintDTO recordValue in markedConstraint)
                {
                    var c = subIndustryTeams.Where(t => t.companyName.Equals(recordValue.companyTeam)).ToList();
                    //Console.WriteLine(c);
                    if (c != null && c.Count != 0)
                    {
                        CompanyTeamDTO t = c.First();
                        CreateMeeting(new MeetingDTO()
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
                            serialNo = recordValue.serialNo,
                            changedAmount = 1,
                            companyTeamCode = t.companyCode,
                        });
                    }

                }
            }
            return GetAllMeetings();
        }*/
        public byte[] ExporttoExcel()
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("ZKP3");

                //ws.ColumnWidth = 25;
                ws.Columns("A").Width = 15;
                ws.Columns("B").Width = 11;
                ws.Columns("C").Width = 15;
                ws.Columns("D").Width = 5;
                ws.Columns("E").Width = 20;
                ws.Columns("F").Width = 25;
                ws.Columns("G").Width = 25;
                //header
                ws.Cell(1, 1).Value = "Malzeme Kodu";
                ws.Cell(1, 2).Value = "Satıcı";
                ws.Cell(1, 3).Value = "Kayıt Tarih";
                ws.Cell(1, 4).Value = "Ür. Plan değişiklik(ADET)";
                ws.Cell(1, 5).Value = "Öteleme Sebebi";
                ws.Cell(1, 6).Value = "Satıcı Tanım";
                ws.Cell(1, 7).Value = "Sorumlu Birey";
                ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                //connect to sql here
                System.Data.DataTable dt = new System.Data.DataTable();
                List<MeetingDTO> list = GetAllMeetings();
                int i = 2;
                foreach (MeetingDTO item in list)
                {
                    // dt.Rows.Add(item.dateCurrent, item.chargePerson, item.companyTeam, item.materialText, item.materialCode, item.productCode,
                    //  item.amount ,item.plannedDate, item.delayAmount,item.delayDate, item.delayReason, item.delayDetail, item.version );
                    ws.Cell(i, 1).Value = Convert.ToString(item.materialCode);
                    ws.Cell(i, 2).Value = Convert.ToString(item.companyTeamCode);
                    ws.Cell(i, 3).Value = Convert.ToString(item.dateCurrent);
                    ws.Cell(i, 4).Value = Convert.ToString(item.changedAmount);
                    ws.Cell(i, 5).Value = Convert.ToString(item.delayReason);
                    ws.Cell(i, 6).Value = Convert.ToString(item.companyTeam).TrimEnd();
                    ws.Cell(i, 7).Value = Convert.ToString(item.chargePerson);
                    
                    i++;
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
                var ws = workbook.Worksheets.Add("ZKP3");
                //ws.ColumnWidth = 25;
                ws.Columns("A").Width = 15;
                ws.Columns("B").Width = 11;
                ws.Columns("C").Width = 15;
                ws.Columns("D").Width = 5;
                ws.Columns("E").Width = 20;
                ws.Columns("F").Width = 25;
                ws.Columns("G").Width = 25;
                //header
                ws.Cell(1, 1).Value = "Malzeme Kodu";
                ws.Cell(1, 2).Value = "Satıcı";
                ws.Cell(1, 3).Value = "Kayıt Tarih";
                ws.Cell(1, 4).Value = "Ür. Plan değişiklik(ADET)";
                ws.Cell(1, 5).Value = "Öteleme Sebebi";
                ws.Cell(1, 6).Value = "Satıcı Tanım";
                ws.Cell(1, 7).Value = "Sorumlu Birey";
                ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                //connect to sql here
                System.Data.DataTable dt = new System.Data.DataTable();
                List<MeetingDTO> list = GetAllMeetings();
                list = list.Where(x => Convert.ToDateTime(x.dateCurrent) <= endDate && Convert.ToDateTime(x.dateCurrent) >= startDate).ToList();
                int i = 2;
                foreach (MeetingDTO item in list)
                {
                    // dt.Rows.Add(item.dateCurrent, item.chargePerson, item.companyTeam, item.materialText, item.materialCode, item.productCode,
                    //  item.amount ,item.plannedDate, item.delayAmount,item.delayDate, item.delayReason, item.delayDetail, item.version );
                    ws.Cell(i, 1).Value = Convert.ToString(item.materialCode);
                    ws.Cell(i, 2).Value = Convert.ToString(item.companyTeamCode);
                    ws.Cell(i, 3).Value = Convert.ToString(item.dateCurrent);
                    ws.Cell(i, 4).Value = Convert.ToString(item.changedAmount);
                    ws.Cell(i, 5).Value = Convert.ToString(item.delayReason);
                    ws.Cell(i, 6).Value = Convert.ToString(item.companyTeam).TrimEnd();
                    ws.Cell(i, 7).Value = Convert.ToString(item.chargePerson);

                    i++;
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
