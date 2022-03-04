using Constraint.BusinessLayer.DTO;
using Constraint.BusinessLayer.IManagers;
using Constraint.DataAccessLayer;
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
    public class ProductTreeManager : IProductTreeManager
    {
        private UnitOfWork _unitOfWork;
        public ProductTreeManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public ProductTreeDTO CreateTree(ProductTreeDTO tree)
        {
            if (tree == null)
            {
                return null;
            }
            ProductTree newtree = new ProductTree();
            newtree.treeID = System.Guid.NewGuid();
            newtree.productText = tree.productText;
            newtree.UA_klnm = tree.UA_klnm;
            newtree.productCode = tree.productCode;
            newtree.alt_UA = tree.alt_UA;
            newtree.higherCode = tree.higherCode;
            newtree.higherCodeText = tree.higherCodeText;
            newtree.sparePart = tree.sparePart;
            newtree.materialCode = tree.materialCode;
            newtree.materialText = tree.materialText;
            newtree.amount = tree.amount;
            newtree.mip = tree.mip;
            newtree.tob = tree.tob;
            newtree.mtu = tree.mtu;
            newtree.goodGroup = tree.goodGroup;
            newtree.validityDate = tree.validityDate;
            newtree.finishDate = tree.finishDate;
            newtree.tdrType = tree.tdrType;
            ProductTree recordValue = _unitOfWork.ProductTreeRepository.Add(newtree);
           
            _unitOfWork.Complete();
            ProductTreeDTO returnValue = new ProductTreeDTO()
            {
                 treeID = recordValue.treeID,
             productText = recordValue.productText,
             UA_klnm = recordValue.UA_klnm,
             productCode = recordValue.productCode,
             alt_UA = recordValue.alt_UA,
             higherCode = recordValue.higherCode,
             higherCodeText = recordValue.higherCodeText,
             sparePart = recordValue.sparePart,
             materialCode = recordValue.materialCode,
             materialText = recordValue.materialText,
             amount = recordValue.amount,
             mip = recordValue.mip,
             tob = recordValue.tob,
             mtu = recordValue.mtu,
             goodGroup = recordValue.goodGroup,
             validityDate = recordValue.validityDate,
             finishDate = recordValue.finishDate,
             tdrType = recordValue.tdrType,

        };
            return returnValue;
        }

        public bool DeleteTree(System.Guid id)
        {
            if (_unitOfWork.ProductTreeRepository.Remove(id))
            {
                _unitOfWork.Complete();
                return true;
            }
            return false;
        }

        public List<ProductTreeDTO> GetAllTrees()
        {
            List<ProductTree> list = _unitOfWork.ProductTreeRepository.GetAll().OrderBy(x => x.productCode).ToList();
            List<ProductTreeDTO> dtoList = new List<ProductTreeDTO>();
            if (list == null)
            {
                return null;
            }
            foreach (ProductTree recordValue in list)
            {
                ProductTreeDTO returnValue = new ProductTreeDTO()
                {
                    treeID = recordValue.treeID,
                    productText = recordValue.productText,
                    UA_klnm = recordValue.UA_klnm,
                    productCode = recordValue.productCode,
                    alt_UA = recordValue.alt_UA,
                    higherCode = recordValue.higherCode,
                    higherCodeText = recordValue.higherCodeText,
                    sparePart = recordValue.sparePart,
                    materialCode = recordValue.materialCode,
                    materialText = recordValue.materialText,
                    amount = recordValue.amount,
                    mip = recordValue.mip,
                    tob = recordValue.tob,
                    mtu = recordValue.mtu,
                    goodGroup = recordValue.goodGroup,
                    validityDate = recordValue.validityDate,
                    finishDate = recordValue.finishDate,
                    tdrType = recordValue.tdrType,

                };
                dtoList.Add(returnValue);
            }
            return dtoList;
        }

        public ProductTreeDTO GetTreeById(System.Guid id)
        {
            ProductTree recordValue = _unitOfWork.ProductTreeRepository.GetById(id);
            if (recordValue == null)
                return null;
            ProductTreeDTO returnValue = new ProductTreeDTO()
            {
                treeID = recordValue.treeID,
                productText = recordValue.productText,
                UA_klnm = recordValue.UA_klnm,
                productCode = recordValue.productCode,
                alt_UA = recordValue.alt_UA,
                higherCode = recordValue.higherCode,
                higherCodeText = recordValue.higherCodeText,
                sparePart = recordValue.sparePart,
                materialCode = recordValue.materialCode,
                materialText = recordValue.materialText,
                amount = recordValue.amount,
                mip = recordValue.mip,
                tob = recordValue.tob,
                mtu = recordValue.mtu,
                goodGroup = recordValue.goodGroup,
                validityDate = recordValue.validityDate,
                finishDate = recordValue.finishDate,
                tdrType = recordValue.tdrType,


            };
            return returnValue;
        }

        public ProductTreeDTO UpdateTree(ProductTreeDTO tree)
        {
            if (tree == null)
            {
                return null;
            }
            ProductTree newtree = new ProductTree();
            newtree.treeID = tree.treeID;
            newtree.productText = tree.productText;
            newtree.UA_klnm = tree.UA_klnm;
            newtree.productCode = tree.productCode;
            newtree.alt_UA = tree.alt_UA;
            newtree.higherCode = tree.higherCode;
            newtree.higherCodeText = tree.higherCodeText;
            newtree.sparePart = tree.sparePart;
            newtree.materialCode = tree.materialCode;
            newtree.materialText = tree.materialText;
            newtree.amount = tree.amount;
            newtree.mip = tree.mip;
            newtree.tob = tree.tob;
            newtree.mtu = tree.mtu;
            newtree.goodGroup = tree.goodGroup;
            newtree.validityDate = tree.validityDate;
            newtree.finishDate = tree.finishDate;
            newtree.tdrType = tree.tdrType;
            ProductTree recordValue = _unitOfWork.ProductTreeRepository.Update(newtree);
          
            ProductTreeDTO returnValue = new ProductTreeDTO()
            {
                treeID = recordValue.treeID,
                productText = recordValue.productText,
                UA_klnm = recordValue.UA_klnm,
                productCode = recordValue.productCode,
                alt_UA = recordValue.alt_UA,
                higherCode = recordValue.higherCode,
                higherCodeText = recordValue.higherCodeText,
                sparePart = recordValue.sparePart,
                materialCode = recordValue.materialCode,
                materialText = recordValue.materialText,
                amount = recordValue.amount,
                mip = recordValue.mip,
                tob = recordValue.tob,
                mtu = recordValue.mtu,
                goodGroup = recordValue.goodGroup,
                validityDate = recordValue.validityDate,
                finishDate = recordValue.finishDate,
                tdrType = recordValue.tdrType,

            };
            if (_unitOfWork.Complete() > 0)
                return returnValue;
            return null;
        }
        //public void deneme()
        //{

        //    using (var context= new ConstraintDBEntities())
        //    {

        //        var data = context.ProductTrees.ToList();

        //        var data2 = (from p in context.ProductTrees
        //                    join m in context.MontageProductPlans on p.productCode equals m.productCode
        //                    select new 
        //                    {
        //                        Code = p.productCode
        //                    }).ToList();


        //    }
        //}

        /// <summary>
        /// Plan ve Ürün tabloları innerjoin yapıldı
        /// Öteleme girilmemiş veriler Kısıt tablosundan silindi.
        /// Yeni gelen veriler, daha önce Öteleme girilmiş ama arşivlenmemiş olanların altına eklendi.
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
        /// Yeni gelen veriler, ürün tablosuna aktarılır.
        /// Yeni gelen ürün ağacına göre kısıt tablosu güncellenir.
        /// Excelden gelen listeyi burada kullanıcaz direk
        /// </summary>
        /// <param name="excelList"></param>
        /// <returns></returns>
        public bool GetDataFromExcelFile(string filename, Stream stream)
        {
            DataSet dsexcelRecords = new DataSet();
            IExcelDataReader reader = null;

            if (stream != null)
            {
                try
                {
                  
                    if (filename.EndsWith(".xls"))
                    {
                        reader = ExcelReaderFactory.CreateBinaryReader(stream);
                    }
                        
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
                       if (_unitOfWork.ProductTreeRepository.RemoveAll("ProductTree"))
                        {
                           
                            DataTable dtStudentRecords = dsexcelRecords.Tables[0];

                            for (int i = 1; i < dtStudentRecords.Rows.Count; i++)
                            {
                               
                                ProductTree objStudent = new ProductTree();
                                objStudent.treeID = System.Guid.NewGuid();
                                objStudent.productCode = (dtStudentRecords.Rows[i][0].ToString());
                                objStudent.productText = Convert.ToString(dtStudentRecords.Rows[i][1]);
                                 objStudent.higherCode = Convert.ToString(dtStudentRecords.Rows[i][2]);
                                 objStudent.higherCodeText = Convert.ToString(dtStudentRecords.Rows[i][3]);
                                objStudent.materialCode = Convert.ToString(dtStudentRecords.Rows[i][4]);
                                objStudent.materialText = Convert.ToString(dtStudentRecords.Rows[i][5]);
                                objStudent.amount = Convert.ToString(dtStudentRecords.Rows[i][6]);
                                 objStudent.tob = Convert.ToString(dtStudentRecords.Rows[i][7]);
                                objStudent.mip = Convert.ToString(dtStudentRecords.Rows[i][8]);
                                objStudent.mtu = Convert.ToString(dtStudentRecords.Rows[i][9]);                               
                                ProductTree recordValue = _unitOfWork.ProductTreeRepository.Add(objStudent);

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
                catch (Exception e)
                {
                    var g = e.Message;
                    Console.WriteLine(e.Message);
                    throw;
                }   
                

            }

            return false;
        }

    }
}
