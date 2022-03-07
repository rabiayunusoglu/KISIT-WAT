using System.Collections.Generic;
using Constraint.BusinessLayer.DTO;
using System.Linq;

using Constraint.BusinessLayer.IManagers;
using Constraint.DataAccessLayer;
using System.Collections;
using System;
using ClosedXML.Excel;
using System.IO;

namespace Constraint.BusinessLayer.Managers
{

    public class ArchiveConstraintManager : IArchiveConstraintManager
    {
        private UnitOfWork _unitOfWork;
        public ArchiveConstraintManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }

        public ArchiveConstraintDTO CreateArchive(ArchiveConstraintDTO archiveConstraint)
        {
            if (archiveConstraint == null)
            {
                return null;
            }
            ArchiveConstraint newConstraint = new ArchiveConstraint();
            newConstraint.constraintID = System.Guid.NewGuid();
            newConstraint.materialCode = archiveConstraint.materialCode;
            newConstraint.materialText = archiveConstraint.materialText;
            newConstraint.productCode = archiveConstraint.productCode;
            newConstraint.plannedDate = archiveConstraint.plannedDate;
            newConstraint.amount = archiveConstraint.amount;
            newConstraint.customer = archiveConstraint.customer;
            newConstraint.version = archiveConstraint.version;
            newConstraint.delayID = archiveConstraint.delayID;
            newConstraint.delayCode = archiveConstraint.delayCode;
            newConstraint.delayAmount = archiveConstraint.delayAmount;
            newConstraint.delayDate = archiveConstraint.delayDate;
            newConstraint.delayReason = archiveConstraint.delayReason;
            newConstraint.delayDetail = archiveConstraint.delayDetail;
            newConstraint.companyTeam = archiveConstraint.companyTeam;
            newConstraint.chargePerson = archiveConstraint.chargePerson;
            newConstraint.dateCurrent = archiveConstraint.dateCurrent;
            newConstraint.aboveLine = archiveConstraint.aboveLine;
            newConstraint.boundMontageID = archiveConstraint.boundMontageID;


            ArchiveConstraint recordValue = _unitOfWork.ArchiveConstraintRepository.Add(newConstraint);

            ArchiveConstraintDTO returnValue = new ArchiveConstraintDTO()
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
                aboveLine = recordValue.aboveLine,
                boundMontageID = recordValue.boundMontageID,

            };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }

        public bool DeleteArchive(System.Guid id)
        {
            if (id == null)
                return false;
            if (_unitOfWork.ArchiveConstraintRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                {
                    return true;
                }
            }
            return false;
        }
        public bool DeleteAllArchive()
        {
            if (_unitOfWork.ArchiveConstraintRepository.RemoveAll("ArchiveConstraint"))
            {
                return true;
            }
            return false;
        }
        public List<ArchiveConstraintDTO> GetAllArchives()
        {
            List<ArchiveConstraint> archivelist = _unitOfWork.ArchiveConstraintRepository.GetAll().OrderBy(x => x.dateCurrent).ToList();
            List<ArchiveConstraintDTO> list = new List<ArchiveConstraintDTO>();
            if (archivelist == null)
            {
                return null;
            }
            foreach (ArchiveConstraint constraint in archivelist)
            {
                ArchiveConstraintDTO returnValue = new ArchiveConstraintDTO()
                {

                    constraintID = constraint.constraintID,
                    materialCode = constraint.materialCode,
                    materialText = constraint.materialText,
                    productCode = constraint.productCode,
                    plannedDate = constraint.plannedDate,
                    amount = constraint.amount,
                    customer = constraint.customer,
                    version = constraint.version,
                    delayID = constraint.delayID,
                    delayCode = constraint.delayCode,
                    delayAmount = constraint.delayAmount,
                    delayDate = constraint.delayDate,
                    delayReason = constraint.delayReason,
                    delayDetail = constraint.delayDetail,
                    companyTeam = constraint.companyTeam,
                    chargePerson = constraint.chargePerson,
                    dateCurrent = constraint.dateCurrent,
                    aboveLine = constraint.aboveLine,
                    boundMontageID = constraint.boundMontageID,

                };
                list.Add(returnValue);

            }

            return list;
        }

        public ArchiveConstraintDTO GetArchiveById(System.Guid id)
        {
            if (id == null)
                return null;
            ArchiveConstraint recordValue = _unitOfWork.ArchiveConstraintRepository.GetById(id);

            if (recordValue == null)
                return null;
            ArchiveConstraintDTO returnValue = new ArchiveConstraintDTO()
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
                aboveLine = recordValue.aboveLine,
                boundMontageID = recordValue.boundMontageID,
            };

            return returnValue;
        }

        public ArchiveConstraintDTO UpdateArchive(ArchiveConstraintDTO constraint)
        {
            if (constraint == null)
            {
                return null;
            }
            ArchiveConstraint updateArchive = new ArchiveConstraint();
            updateArchive.constraintID = constraint.constraintID;
            updateArchive.materialCode = constraint.materialCode;
            updateArchive.materialText = constraint.materialText;
            updateArchive.productCode = constraint.productCode;
            updateArchive.plannedDate = constraint.plannedDate;
            updateArchive.amount = constraint.amount;
            updateArchive.customer = constraint.customer;
            updateArchive.version = constraint.version;
            updateArchive.delayID = constraint.delayID;
            updateArchive.delayCode = constraint.delayCode;
            updateArchive.delayAmount = constraint.delayAmount;
            updateArchive.delayDate = constraint.delayDate;
            updateArchive.delayReason = constraint.delayReason;
            updateArchive.delayDetail = constraint.delayDetail;
            updateArchive.companyTeam = constraint.companyTeam;
            updateArchive.chargePerson = constraint.chargePerson;
            updateArchive.dateCurrent = constraint.dateCurrent;
            updateArchive.aboveLine = constraint.aboveLine;
            updateArchive.boundMontageID = constraint.boundMontageID;

            ArchiveConstraint recordValue = _unitOfWork.ArchiveConstraintRepository.Update(updateArchive);

            ArchiveConstraintDTO returnValue = new ArchiveConstraintDTO()
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
                aboveLine = recordValue.aboveLine,
                boundMontageID = recordValue.boundMontageID,
            };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }

        public byte[] ExporttoExcel()
        {
            /*using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("ARSIV-KISIT");
                ws.ColumnWidth = 13;
                //header
                ws.Cell(1, 1).Value = "Girildiği Tarih"; 
                ws.Cell(1, 2).Value = "Sorumlu Birey"; 
                ws.Cell(1, 3).Value = "Firma-Takım";
                ws.Cell(1, 4).Value = "Malzeme Tanımı";
                ws.Cell(1, 5).Value = "Müşteri";
                ws.Cell(1, 6).Value = "Öteleme Kodu"; 
                ws.Cell(1, 7).Value = "Ürün Kodu"; 
                ws.Cell(1, 8).Value = "Plandaki Miktar";
                ws.Cell(1, 9).Value = "Plan Tarihi";
                ws.Cell(1, 10).Value = "Ötelenecek Miktar";
                ws.Cell(1, 11).Value = "Ötelenecek Tarih";
                ws.Cell(1, 12).Value = "Öteleme Sebebi";
                ws.Cell(1, 13).Value = "Öteleme Açıklama";
                ws.Cell(1, 14).Value = "Hat Bilgisi";
                ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 10).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 11).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 12).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 13).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 14).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;

                //connect to sql here
                System.Data.DataTable dt = new System.Data.DataTable();
                List<ArchiveConstraintDTO> list = GetAllArchives();
                if (list.Count() == 0)
                    return null;
                int i = 2;
                foreach (ArchiveConstraintDTO item in list)
                {
                    // dt.Rows.Add(item.dateCurrent, item.chargePerson, item.companyTeam, item.materialText, item.materialCode, item.productCode,
                    //  item.amount ,item.plannedDate, item.delayAmount,item.delayDate, item.delayReason, item.delayDetail, item.version );
                    ws.Cell(i, 1).Value = Convert.ToString(item.dateCurrent);
                    ws.Cell(i, 2).Value = Convert.ToString(item.chargePerson);
                    ws.Cell(i, 3).Value = Convert.ToString(item.companyTeam).TrimEnd();
                    ws.Cell(i, 4).Value = Convert.ToString(item.materialText);
                    ws.Cell(i, 5).Value = Convert.ToString(item.customer);
                    ws.Cell(i, 6).Value = Convert.ToString(item.materialCode);
                    ws.Cell(i, 7).Value = Convert.ToString(item.productCode);
                    ws.Cell(i, 8).Value = Convert.ToString(item.amount);
                    ws.Cell(i, 9).Value = Convert.ToString(item.plannedDate);
                    ws.Cell(i, 10).Value = Convert.ToString(item.delayAmount);
                    ws.Cell(i, 11).Value = Convert.ToString(item.delayDate);
                    ws.Cell(i, 12).Value = Convert.ToString(item.delayReason);
                    ws.Cell(i, 13).Value = Convert.ToString(item.delayDetail);
                    ws.Cell(i, 14).Value = Convert.ToString(item.version);
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
            }*/
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("ARSIV-KISIT");
                ws.ColumnWidth = 25;

                //header
                ws.Cell(1, 1).Value = "Tarih";
                ws.Cell(1, 2).Value = "Sorumlu";
                ws.Cell(1, 3).Value = "Malzeme No";
                ws.Cell(1, 4).Value = "Malzeme Tanımı";
                ws.Cell(1, 5).Value = "Firma/Ekip";
                ws.Cell(1, 6).Value = "Ürün";
                ws.Cell(1, 7).Value = "Miktar";
                ws.Cell(1, 8).Value = "Plan Tarih";
                ws.Cell(1, 9).Value = "Ötelenecek Tarih";
                ws.Cell(1, 10).Value = "Öteleme Sebebi";
                ws.Cell(1, 11).Value = "Öteleme Açıklama";
                ws.Cell(1, 12).Value = "Hat Bilgisi";
                ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 10).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 11).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 12).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;

                //connect to sql here
                System.Data.DataTable dt = new System.Data.DataTable();
                List<ArchiveConstraintDTO> list = GetAllArchives();
                if (list.Count() == 0)
                    return null;
                int i = 2;
                foreach (ArchiveConstraintDTO item in list)
                {
                    // dt.Rows.Add(item.dateCurrent, item.chargePerson, item.companyTeam, item.materialText, item.materialCode, item.productCode,
                    //  item.amount ,item.plannedDate, item.delayAmount,item.delayDate, item.delayReason, item.delayDetail, item.version );
                    ws.Cell(i, 1).Value = Convert.ToString(item.dateCurrent);
                    ws.Cell(i, 2).Value = Convert.ToString(item.chargePerson);
                    ws.Cell(i, 3).Value = Convert.ToString(item.materialCode).TrimEnd();
                    ws.Cell(i, 4).Value = Convert.ToString(item.materialText);
                    ws.Cell(i, 5).Value = Convert.ToString(item.companyTeam).TrimEnd();
                    ws.Cell(i, 6).Value = Convert.ToString(item.productCode).TrimEnd();
                    ws.Cell(i, 7).Value = Convert.ToString(item.delayAmount);
                    ws.Cell(i, 8).Value = Convert.ToString(item.plannedDate);
                    ws.Cell(i, 9).Value = Convert.ToString(item.delayDate);
                    ws.Cell(i, 10).Value = Convert.ToString(item.delayReason);
                    ws.Cell(i, 11).Value = Convert.ToString(item.delayDetail);
                    ws.Cell(i, 12).Value = Convert.ToString(item.version);
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
                var ws = workbook.Worksheets.Add("ARSIV-KISIT");
                ws.ColumnWidth = 25;


                //header
                ws.Cell(1, 1).Value = "Girildiği Tarih";
                ws.Cell(1, 2).Value = "Sorumlu";
                ws.Cell(1, 3).Value = "Malzeme No";
                ws.Cell(1, 4).Value = "Malzeme Tanımı";
                ws.Cell(1, 5).Value = "Firma/Ekip";
                ws.Cell(1, 6).Value = "Ürün";
                ws.Cell(1, 7).Value = "Miktar";
                ws.Cell(1, 8).Value = "Plan Tarih";
                ws.Cell(1, 9).Value = "Ötelenecek Tarih";
                ws.Cell(1, 10).Value = "Öteleme Sebebi";
                ws.Cell(1, 11).Value = "Öteleme Açıklama";
                ws.Cell(1, 12).Value = "Hat Bilgisi";
                ws.Cell(1, 1).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 2).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 3).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 4).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 5).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 6).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 7).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 8).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 9).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 10).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 11).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;
                ws.Cell(1, 12).Style.Fill.BackgroundColor = XLColor.BabyBlueEyes;

                //connect to sql here
                System.Data.DataTable dt = new System.Data.DataTable();
                List<ArchiveConstraintDTO> list = GetAllArchives();
                list = list.Where(x => Convert.ToDateTime(x.dateCurrent) <= endDate && Convert.ToDateTime(x.dateCurrent) >= startDate).ToList();
                if (list.Count() == 0)
                    return null;
                int i = 2;
                foreach (ArchiveConstraintDTO item in list)
                {
                    // dt.Rows.Add(item.dateCurrent, item.chargePerson, item.companyTeam, item.materialText, item.materialCode, item.productCode,
                    //  item.amount ,item.plannedDate, item.delayAmount,item.delayDate, item.delayReason, item.delayDetail, item.version );
                    ws.Cell(i, 1).Value = Convert.ToString(item.dateCurrent);
                    ws.Cell(i, 2).Value = Convert.ToString(item.chargePerson);
                    ws.Cell(i, 3).Value = Convert.ToString(item.materialCode).TrimEnd();
                    ws.Cell(i, 4).Value = Convert.ToString(item.materialText);
                    ws.Cell(i, 5).Value = Convert.ToString(item.companyTeam).TrimEnd();
                    ws.Cell(i, 6).Value = Convert.ToString(item.productCode).TrimEnd();
                    ws.Cell(i, 7).Value = Convert.ToString(item.delayAmount);
                    ws.Cell(i, 8).Value = Convert.ToString(item.plannedDate);
                    ws.Cell(i, 9).Value = Convert.ToString(item.delayDate);
                    ws.Cell(i, 10).Value = Convert.ToString(item.delayReason);
                    ws.Cell(i, 11).Value = Convert.ToString(item.delayDetail);
                    ws.Cell(i, 12).Value = Convert.ToString(item.version);
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
