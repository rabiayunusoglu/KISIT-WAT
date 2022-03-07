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
    public class ConstraintManager : IConstraintManager
    {
        private UnitOfWork _unitOfWork;
        public ConstraintManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public ConstraintDTO CreateConstraint(ConstraintDTO constraint)
        {
            Constraint.DataAccessLayer.Constraint newConstraint = new Constraint.DataAccessLayer.Constraint();
            if (constraint == null) { return null; }

            newConstraint.isMarked = constraint.isMarked;
            newConstraint.isDelayEntered = constraint.isDelayEntered;
            newConstraint.constraintID = System.Guid.NewGuid();
            newConstraint.materialCode = constraint.materialCode;
            newConstraint.materialText = constraint.materialText;
            newConstraint.productCode = constraint.productCode;
            newConstraint.plannedDate = constraint.plannedDate;
            newConstraint.amount = constraint.amount;
            newConstraint.customer = constraint.customer;
            newConstraint.version = constraint.version;
            newConstraint.delayID = constraint.delayID;
            newConstraint.delayCode = constraint.delayCode;
            newConstraint.delayAmount = constraint.delayAmount;
            newConstraint.delayDate = constraint.delayDate;
            newConstraint.delayReason = constraint.delayReason;
            newConstraint.delayDetail = constraint.delayDetail;
            newConstraint.companyTeam = constraint.companyTeam;
            newConstraint.chargePerson = constraint.chargePerson;
            newConstraint.dateCurrent = constraint.dateCurrent;
            newConstraint.treeAmount = constraint.treeAmount;
            newConstraint.mip = constraint.mip;
            newConstraint.tob = constraint.tob;
            newConstraint.boundMontageID = constraint.boundMontageID;

            newConstraint.aboveLine = constraint.aboveLine;

            Constraint.DataAccessLayer.Constraint recordValue = _unitOfWork.ConstraintRepository.Add(newConstraint);
            ConstraintDTO returnValue = new ConstraintDTO()
            {
                isMarked = recordValue.isMarked,
                isDelayEntered = recordValue.isDelayEntered,
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
                treeAmount = recordValue.treeAmount,
                mip = recordValue.mip,
                tob = recordValue.tob,
                boundMontageID=recordValue.boundMontageID

            };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }

            return null;
        }

        public bool DeleteConstraint(System.Guid id)
        {
            if (_unitOfWork.ConstraintRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                    return true;
            }
            return false;
        }

        public List<ConstraintDTO> GetAllConstraints()
        {
            List<Constraint.DataAccessLayer.Constraint> list = _unitOfWork.ConstraintRepository.GetAll().OrderBy(x => x.plannedDate).ToList();
            List<ConstraintDTO> dtoList = new List<ConstraintDTO>();
            if (list == null) { return null; }
            foreach (Constraint.DataAccessLayer.Constraint constraint in list)
            {
                ConstraintDTO returnValue = new ConstraintDTO()
                {

                    isMarked = constraint.isMarked,
                    isDelayEntered = constraint.isDelayEntered,
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
                    treeAmount = constraint.treeAmount,
                    mip = constraint.mip,
                    tob = constraint.tob,
                    aboveLine = constraint.aboveLine,
                    boundMontageID = constraint.boundMontageID
                };
                dtoList.Add(returnValue);
            }
            return dtoList;

        }

        public ConstraintDTO GetConstraintById(System.Guid id)
        {
            Constraint.DataAccessLayer.Constraint recordValue = _unitOfWork.ConstraintRepository.GetById(id);
            if (recordValue == null)
                return null;
            ConstraintDTO returnValue = new ConstraintDTO()
            {

                isMarked = recordValue.isMarked,
                isDelayEntered = recordValue.isDelayEntered,
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
                treeAmount = recordValue.treeAmount,
                mip = recordValue.mip,
                tob = recordValue.tob,
                aboveLine = recordValue.aboveLine,
                boundMontageID = recordValue.boundMontageID
            };
            return returnValue;
        }

        public ConstraintDTO UpdateConstraint(ConstraintDTO constraint)
        {
            Constraint.DataAccessLayer.Constraint newConstraint = new Constraint.DataAccessLayer.Constraint();
            if (constraint == null) { return null; }

            newConstraint.isMarked = constraint.isMarked;
            newConstraint.isDelayEntered = constraint.isDelayEntered;
            newConstraint.constraintID = constraint.constraintID;
            newConstraint.materialCode = constraint.materialCode;
            newConstraint.materialText = constraint.materialText;
            newConstraint.productCode = constraint.productCode;
            newConstraint.plannedDate = constraint.plannedDate;
            newConstraint.amount = constraint.amount;
            newConstraint.customer = constraint.customer;
            newConstraint.version = constraint.version;
            newConstraint.delayID = constraint.delayID;
            newConstraint.delayCode = constraint.delayCode;
            newConstraint.delayAmount = constraint.delayAmount;
            newConstraint.delayDate = constraint.delayDate;
            newConstraint.delayReason = constraint.delayReason;
            newConstraint.delayDetail = constraint.delayDetail;
            newConstraint.companyTeam = constraint.companyTeam;
            newConstraint.chargePerson = constraint.chargePerson;
            newConstraint.dateCurrent = constraint.dateCurrent;
            newConstraint.treeAmount = constraint.treeAmount;
            newConstraint.mip = constraint.mip;
            newConstraint.tob = constraint.tob;
            newConstraint.aboveLine = constraint.aboveLine;
            newConstraint.boundMontageID = constraint.boundMontageID;
            Constraint.DataAccessLayer.Constraint recordValue = _unitOfWork.ConstraintRepository.Update(newConstraint);

            ConstraintDTO returnValue = new ConstraintDTO()
            {

                isMarked = recordValue.isMarked,
                isDelayEntered = recordValue.isDelayEntered,
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
                treeAmount = recordValue.treeAmount,
                mip = recordValue.mip,
                tob = recordValue.tob,
                aboveLine = recordValue.aboveLine,
                boundMontageID=recordValue.boundMontageID
            };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }

        public byte[] ExporttoExcel(bool isMark)
        {
            using (var workbook = new XLWorkbook())
            {
                var ws = workbook.Worksheets.Add("KISIT");
                ws.ColumnWidth = 20;
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
                List<ConstraintDTO> list = new List<ConstraintDTO>();
                if (isMark)
                {
                    list = GetAllConstraints();
                    list = list.Where(x => x.isMarked == true && x.isDelayEntered == true).ToList();
                }
                else
                {
                    list = GetAllConstraints();
                    list = list.Where(x => x.isMarked == false && x.isDelayEntered == true).ToList();
                }

                int i = 2;
                foreach (ConstraintDTO item in list)
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
        public bool Save(ConstraintDTO constraint, DelayHistoryDTO delay)
        {
            DelayHistoryManager delayHistoryManager = new DelayHistoryManager();
            DelayHistoryDTO temp = delayHistoryManager.Createhistory(delay);
            if (temp == null)
                return false;
            ConstraintManager constraintManager = new ConstraintManager();
            constraint.delayID = temp.delayID.ToString();
            constraint.isDelayEntered = true;
            constraint.isMarked = false;

            var value = constraintManager.UpdateConstraint(constraint);
            if (value == null)
                return false;
            return true;
        }

    }
}
