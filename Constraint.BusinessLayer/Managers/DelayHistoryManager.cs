using Constraint.BusinessLayer.DTO;
using Constraint.BusinessLayer.IManagers;
using Constraint.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.Managers
{
    public class DelayHistoryManager : IDelayHistoryManager
    {
        private UnitOfWork _unitOfWork;
        public DelayHistoryManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public DelayHistoryDTO Createhistory(DelayHistoryDTO history)
        {
            if (history == null)
            {
                return null;
            }
            DelayHistory newHistory = new DelayHistory();
            newHistory.isMarked = history.isMarked;
            newHistory.isArchive = history.isArchive;
            newHistory.delayID = System.Guid.NewGuid();
            newHistory.productCode = history.productCode;
            newHistory.delayCode = history.delayCode;
            newHistory.delayAmount = history.delayAmount;
            newHistory.delayDate = history.delayDate;
            newHistory.delayReason = history.delayReason;
            newHistory.delayDetail = history.delayDetail;
            newHistory.companyTeam = history.companyTeam;
            newHistory.chargePerson = history.chargePerson;
            newHistory.madeDate = history.madeDate;
            newHistory.boundConstraintID = history.boundConstraintID;
            newHistory.boundMontageID = history.boundMontageID;
            DelayHistory recordValue = _unitOfWork.DelayHistoryRepository.Add(newHistory);

            DelayHistoryDTO returnValue = new DelayHistoryDTO()
            {
                isMarked = recordValue.isMarked,
                isArchive = recordValue.isArchive,
                delayID = recordValue.delayID,
                productCode = recordValue.productCode,
                madeDate = recordValue.madeDate,
                delayCode = recordValue.delayCode,
                delayAmount = recordValue.delayAmount,
                delayDate = recordValue.delayDate,
                delayReason = recordValue.delayReason,
                delayDetail = recordValue.delayDetail,
                companyTeam = recordValue.companyTeam,
                chargePerson = recordValue.chargePerson,
                boundConstraintID = recordValue.boundConstraintID,
                boundMontageID = recordValue.boundMontageID,
            };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }

        public bool Deletehistory(System.Guid id)
        {
            if (_unitOfWork.DelayHistoryRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                    return true;
            }
            return false;
        }

        public List<DelayHistoryDTO> GetAllHistories()
        {
            List<DelayHistory> list = _unitOfWork.DelayHistoryRepository.GetAll().OrderBy(x => x.madeDate).ToList();
            List<DelayHistoryDTO> dtoList = new List<DelayHistoryDTO>();
            if (list == null)
            {
                return null;
            }
            foreach (DelayHistory constraint in list)
            {
                DelayHistoryDTO returnValue = new DelayHistoryDTO()
                {
                    isMarked = constraint.isMarked,
                    isArchive = constraint.isArchive,
                    delayID = constraint.delayID,
                    productCode = constraint.productCode,
                    madeDate = constraint.madeDate,

                    delayCode = constraint.delayCode,
                    delayAmount = constraint.delayAmount,
                    delayDate = constraint.delayDate,
                    delayReason = constraint.delayReason,
                    delayDetail = constraint.delayDetail,
                    companyTeam = constraint.companyTeam,
                    chargePerson = constraint.chargePerson,
                    boundConstraintID = constraint.boundConstraintID,
                    boundMontageID = constraint.boundMontageID

                };
                dtoList.Add(returnValue);
            }
            return dtoList;
        }
        public bool DeleteAllArchiveHistories()
        {
            List<DelayHistory> list = _unitOfWork.DelayHistoryRepository.GetAll().ToList();
            if (list == null)
                return true;
            list = list.Where(x => x.isMarked == true & x.isArchive == true).ToList();
            if (list == null)
            {
                return true;
            }
            DelayHistoryManager delay = new DelayHistoryManager();
            foreach (DelayHistory constraint in list)
            {
                var value = delay.Deletehistory(constraint.delayID);
                if (value == false)
                    return false;
            }
            return true;
        }
        public DelayHistoryDTO GetHistoryById(System.Guid id)
        {
            DelayHistory recordValue = _unitOfWork.DelayHistoryRepository.GetById(id);
            if (recordValue == null)
                return null;
            DelayHistoryDTO returnValue = new DelayHistoryDTO()
            {
                isMarked = recordValue.isMarked,
                isArchive = recordValue.isArchive,
                delayID = recordValue.delayID,
                productCode = recordValue.productCode,
                madeDate = recordValue.madeDate,
                delayCode = recordValue.delayCode,
                delayAmount = recordValue.delayAmount,
                delayDate = recordValue.delayDate,
                delayReason = recordValue.delayReason,
                delayDetail = recordValue.delayDetail,
                companyTeam = recordValue.companyTeam,
                chargePerson = recordValue.chargePerson,
                boundConstraintID = recordValue.boundConstraintID,
                boundMontageID = recordValue.boundMontageID,

            };
            return returnValue;
        }

        public DelayHistoryDTO UpdateHistory(DelayHistoryDTO history)
        {
            if (history == null)
            {
                return null;
            }
            DelayHistory newHistory = new DelayHistory();
            newHistory.isMarked = history.isMarked;
            newHistory.isArchive = history.isArchive;
            newHistory.delayID = history.delayID;
            newHistory.productCode = history.productCode;
            newHistory.delayCode = history.delayCode;
            newHistory.delayAmount = history.delayAmount;
            newHistory.delayDate = history.delayDate;
            newHistory.delayReason = history.delayReason;
            newHistory.delayDetail = history.delayDetail;
            newHistory.companyTeam = history.companyTeam;
            newHistory.chargePerson = history.chargePerson;
            newHistory.madeDate = history.madeDate;
            newHistory.boundConstraintID = history.boundConstraintID;
            newHistory.boundMontageID = history.boundMontageID;
            DelayHistory recordValue = _unitOfWork.DelayHistoryRepository.Update(newHistory);
            DelayHistoryDTO returnValue = new DelayHistoryDTO()
            {
                isMarked = recordValue.isMarked,
                isArchive = recordValue.isArchive,
                delayID = recordValue.delayID,
                productCode = recordValue.productCode,
                madeDate = recordValue.madeDate,
                delayCode = recordValue.delayCode,
                delayAmount = recordValue.delayAmount,
                delayDate = recordValue.delayDate,
                delayReason = recordValue.delayReason,
                delayDetail = recordValue.delayDetail,
                companyTeam = recordValue.companyTeam,
                chargePerson = recordValue.chargePerson,
                boundConstraintID = recordValue.boundConstraintID,
                boundMontageID = recordValue.boundMontageID

            };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }
    }
}
