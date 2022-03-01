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
    public class PositionManager : IPositionManager
    {
        private UnitOfWork _unitOfWork;
        public PositionManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public PositionDTO CreatePosition(PositionDTO position)
        {
            Position newPosition = new Position();
            if (position == null)
            {
                return null;
            }
            newPosition.positionID = position.positionID;
            newPosition.materialCode = position.materialCode;
            newPosition.customerCode = position.customerCode;
            newPosition.customerText = position.customerText;
            newPosition.registerDate = position.registerDate;
            newPosition.delayReason = position.delayReason;
            newPosition.amount = position.amount;
            newPosition.companyTeam = position.companyTeam;
            newPosition.chargePerson = position.chargePerson;

            Position recordValue = _unitOfWork.PositionRepository.Add(newPosition);
            _unitOfWork.Complete();
            PositionDTO returnValue = new PositionDTO()
            {
                positionID = recordValue.positionID,
                materialCode = recordValue.materialCode,
                customerCode = recordValue.customerCode,
                customerText = recordValue.customerText,
                registerDate = recordValue.registerDate,
                amount = recordValue.amount,
                delayReason = recordValue.delayReason,
                companyTeam = recordValue.companyTeam,
                chargePerson = recordValue.chargePerson,

            };
            return returnValue;
        }

        public bool DeletePosition(int id)
        {
            if (_unitOfWork.PositionRepository.Remove(id))
            {
                _unitOfWork.Complete();
                return true;
            }
            return false;
        }

        public List<PositionDTO> GetAllPositons()
        {
            List<Position> list = _unitOfWork.PositionRepository.GetAll().ToList();
            List<PositionDTO> dtoList = new List<PositionDTO>();
            if (list == null)
            {
                return null;
            }
            foreach (Position recordValue in list)
            {
                PositionDTO returnValue = new PositionDTO()
                {
                    positionID = recordValue.positionID,
                    materialCode = recordValue.materialCode,
                    customerCode = recordValue.customerCode,
                    customerText = recordValue.customerText,
                    registerDate = recordValue.registerDate,
                    amount = recordValue.amount,
                    delayReason = recordValue.delayReason,
                    companyTeam = recordValue.companyTeam,
                    chargePerson = recordValue.chargePerson,

                };
                dtoList.Add(returnValue);
            }
            return dtoList;
        }

        public PositionDTO GetPositionById(int id)
        {
            Position recordValue = _unitOfWork.PositionRepository.GetById(id);
           
            PositionDTO returnValue = new PositionDTO()
            {
                positionID = recordValue.positionID,
                materialCode = recordValue.materialCode,
                customerCode = recordValue.customerCode,
                customerText = recordValue.customerText,
                registerDate = recordValue.registerDate,
                amount = recordValue.amount,
                delayReason = recordValue.delayReason,
                companyTeam = recordValue.companyTeam,
                chargePerson = recordValue.chargePerson,

            };
            return returnValue;
        }

        public PositionDTO UpdatePosition(PositionDTO position)
        {
            if (position == null)
            {
                return null;
            }
            Position newPosition = new Position();

            newPosition.positionID = position.positionID;
            newPosition.materialCode = position.materialCode;
            newPosition.customerCode = position.customerCode;
            newPosition.customerText = position.customerText;
            newPosition.registerDate = position.registerDate;
            newPosition.delayReason = position.delayReason;
            newPosition.amount = position.amount;
            newPosition.companyTeam = position.companyTeam;
            newPosition.chargePerson = position.chargePerson;

            Position recordValue = _unitOfWork.PositionRepository.Update(newPosition);
            _unitOfWork.Complete();
            PositionDTO returnValue = new PositionDTO()
            {
                positionID = recordValue.positionID,
                materialCode = recordValue.materialCode,
                customerCode = recordValue.customerCode,
                customerText = recordValue.customerText,
                registerDate = recordValue.registerDate,
                amount = recordValue.amount,
                delayReason = recordValue.delayReason,
                companyTeam = recordValue.companyTeam,
                chargePerson = recordValue.chargePerson,

            };
            return returnValue;
        }
    }
}
