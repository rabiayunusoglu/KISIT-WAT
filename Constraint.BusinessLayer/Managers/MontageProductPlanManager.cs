using Constraint.BusinessLayer.DTO;
using Constraint.BusinessLayer.IManagers;
using Constraint.DataAccessLayer;
using DocumentFormat.OpenXml.Office2013.Word;
using ExcelDataReader;
using System;
using System.Collections.Generic;
using System.Data;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.Managers
{
    public class MontageProductPlanManager : IMontageProductPlanManager
    {
        private UnitOfWork _unitOfWork;
        public MontageProductPlanManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public MontageProductPlanDTO CreatePlan(MontageProductPlanDTO plan)
        {
            if (plan == null)
            {
                return null;
            }
            MontageProductPlan montage = new MontageProductPlan();
            montage.montageID = System.Guid.NewGuid();
            montage.productCode = plan.productCode;
            montage.version = plan.version;
            montage.plannedDate = plan.plannedDate;
            montage.amount = plan.amount;
            montage.customer = plan.customer;
            montage.customerType = plan.customerType;
            montage.frame = plan.frame;
            montage.relevant_month = plan.relevant_month;
           
            montage.aboveLine = plan.aboveLine;
            MontageProductPlan recordValue = _unitOfWork.MontageProductPlanRepository.Add(montage);
           
            MontageProductPlanDTO returnValue = new MontageProductPlanDTO()
            {
                montageID = recordValue.montageID,
                productCode = recordValue.productCode,
                version = recordValue.version,
                plannedDate = ( recordValue.plannedDate),
                amount = recordValue.amount,
                customer = recordValue.customer,
                customerType = recordValue.customerType,
                frame = recordValue.frame,
                relevant_month = (recordValue.relevant_month),
               
                aboveLine=recordValue.aboveLine,
            };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }

        public bool DeletePlan(System.Guid id)
        {
            if (_unitOfWork.MontageProductPlanRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                {
                    return true;
                }
              
            }
            return false;
        }

        public List<MontageProductPlanDTO> GetAllPlans()
        {
            List<MontageProductPlan> list = _unitOfWork.MontageProductPlanRepository.GetAll().OrderBy(x => x.productCode).ToList();
            if (list == null)
            {
                return null;
            }

            List<MontageProductPlanDTO> dtoList = new List<MontageProductPlanDTO>();
            foreach (MontageProductPlan recordValue in list)
            {
                MontageProductPlanDTO returnValue = new MontageProductPlanDTO()
                {
                    montageID = recordValue.montageID,
                    productCode = recordValue.productCode,
                    version = recordValue.version,
                    plannedDate = recordValue.plannedDate,
                    amount = recordValue.amount,
                    customer = recordValue.customer,
                    customerType = recordValue.customerType,
                    frame = recordValue.frame,
                    relevant_month = recordValue.relevant_month,
                   
                    aboveLine=recordValue.aboveLine,
                };
                dtoList.Add(returnValue);
            }
            return dtoList;
        }

        public MontageProductPlanDTO GetPlanById(System.Guid id)
        {
            MontageProductPlan recordValue = _unitOfWork.MontageProductPlanRepository.GetById(id);
            if (recordValue == null)
            {
                return null;
            }
            MontageProductPlanDTO returnValue = new MontageProductPlanDTO()
            {
                montageID = recordValue.montageID,
                productCode = recordValue.productCode,
                version = recordValue.version,
                plannedDate = recordValue.plannedDate,
                amount = recordValue.amount,
                customer = recordValue.customer,
                customerType = recordValue.customerType,
                frame = recordValue.frame,
                relevant_month = recordValue.relevant_month,
               
                aboveLine=recordValue.aboveLine,
            };
            return returnValue;
        }

        public MontageProductPlanDTO UpdatePlan(MontageProductPlanDTO plan)
        {
            if (plan == null)
            {
                return null;
            }
            MontageProductPlan montage = new MontageProductPlan();
            montage.montageID = plan.montageID;
            montage.productCode = plan.productCode;
            montage.version = plan.version;
            montage.plannedDate = plan.plannedDate;
            montage.amount = plan.amount;
            montage.customer = plan.customer;
            montage.customerType = plan.customerType;
            montage.frame = plan.frame;
            montage.relevant_month = plan.relevant_month;
            
            montage.aboveLine = plan.aboveLine;
            MontageProductPlan recordValue = _unitOfWork.MontageProductPlanRepository.Update(montage);
            _unitOfWork.Complete();
            MontageProductPlanDTO returnValue = new MontageProductPlanDTO()
            {
                montageID = recordValue.montageID,
                productCode = recordValue.productCode,
                version = recordValue.version,
                plannedDate = recordValue.plannedDate,
                amount = recordValue.amount,
                customer = recordValue.customer,
                customerType = recordValue.customerType,
                frame = recordValue.frame,
                relevant_month = recordValue.relevant_month,
                
                aboveLine=recordValue.aboveLine,
            };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }
        
        /// <summary>
        /// Plan ve Ürün tabloları innerjoin yapıldı
        /// Oteleme girilmemis veriler Kisit tablosundan silindi.
        /// Yeni gelen veriler, daha önce oteleme girilmis ama arsivlenmemis olanlarin altina eklendi.
        /// </summary>
        /// <returns></returns>
        public bool UpdateConstraintTable()
        {
            if (_unitOfWork.ConstraintRepository.RemoveNoEnteredDelay())
            {
                
                    if (_unitOfWork.ConstraintRepository.CreateAllConstraintFromTheBegining())
                    {
                        return true;
                    }
               
            }
            return false;
            
        }

        /// <summary>
        /// Excel formatını yenilediklerinde
        /// Tüm montaj planı silinir.
        /// Yeni gelen veriler, montaj plan tablosuna aktarılır.
        /// Yeni gelen montage planına göre kısıt tablosu güncellenir.
        /// Excelden gelen listeyi burada kullanıcaz direk
        /// </summary>
        /// <param name="excelList"></param>
        /// <returns></returns> 
        public bool GetDataFromExcelFile(string filename, Stream stream)
        {
            DataSet dsexcelRecords = new DataSet();
            IExcelDataReader reader = null;

           if(stream != null)
            {
                try
                {
                        if (filename.EndsWith(".xls"))
                            reader = ExcelReaderFactory.CreateBinaryReader(stream);
                        else if (filename.EndsWith(".xlsx"))
                            reader = ExcelReaderFactory.CreateOpenXmlReader(stream);
                        else
                        {
                            Console.WriteLine("The file format is not supported.");
                            return false;
                        }
                        dsexcelRecords = reader.AsDataSet();
                        reader.Close();
                        if (dsexcelRecords != null && dsexcelRecords.Tables.Count > 0)
                        {
                        if (_unitOfWork.MontageProductPlanRepository.RemoveAll("MontageProductPlan"))
                        {
                            DataTable dtMontageRecords = dsexcelRecords.Tables[0];
                            DateTime today = DateTime.Now;
                            for (int i = 1; i < dtMontageRecords.Rows.Count; i++)
                            {
                                MontageProductPlan objStudent = new MontageProductPlan();
                                //if (Convert.ToDateTime(dtMontageRecords.Rows[i][2]) < today)
                                if (Convert.ToDateTime(dtMontageRecords.Rows[i][2]) < today)
                                        objStudent.aboveLine = "Hat Üstü";
                                else
                                    objStudent.aboveLine = "-";
                                objStudent.montageID = System.Guid.NewGuid();
                                objStudent.productCode = (dtMontageRecords.Rows[i][0].ToString());
                                objStudent.version = Convert.ToString(dtMontageRecords.Rows[i][1]);
                                objStudent.plannedDate = (dtMontageRecords.Rows[i][2].ToString());
                                objStudent.amount = Convert.ToString(dtMontageRecords.Rows[i][3]);
                                objStudent.customer = Convert.ToString(dtMontageRecords.Rows[i][4]);
                                objStudent.customerType = Convert.ToString(dtMontageRecords.Rows[i][5]);
                                objStudent.frame = Convert.ToString(dtMontageRecords.Rows[i][6]);
                                objStudent.relevant_month = Convert.ToString(dtMontageRecords.Rows[i][7]);
                                

                                MontageProductPlan recordValue = _unitOfWork.MontageProductPlanRepository.Add(ChangeVersion(objStudent));

                            }
                            if (_unitOfWork.Complete() > 0)
                            {
                                if (UpdateConstraintTable())
                                    return true;
                                else
                                {
                                    return false;
                                }
                            }
                        }

                    }
                    
                }
                catch (Exception)
                {
                    throw;
                }
            }

            return false;
        }
        public MontageProductPlan ChangeVersion(MontageProductPlan list)
        {
            List<Constraint.DataAccessLayer.Version> listVersion = _unitOfWork.VersionRepository.GetAll().ToList();
                var d = listVersion.Where(t => t.versionName.Trim().ToLower().Equals(list.version.Trim().ToLower()));
            if (d != null || d.Count() !=0)
            {
                if (d.ToList().Count!=0)
                {
                    DataAccessLayer.Version f = d.First();
                    list.version = f.versionValue;
                }
                
            }
      
            return list;
        }

    }
}
