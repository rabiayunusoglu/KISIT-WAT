
using System.Collections.Generic;
using Constraint.BusinessLayer.DTO;
using System.Linq;

using Constraint.DataAccessLayer;
using System.Collections;
using System;
using Constraint.BusinessLayer.IManagers;

namespace Constraint.BusinessLayer.Managers
{
    public class ChargePersonManager:IChargePersonManager
    {
        private UnitOfWork _unitOfWork;
        public ChargePersonManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public ChargePersonDTO CreatePerson(ChargePersonDTO chargePerson)
        {
            if (chargePerson == null)
            {
                return null;
            }
            ChargePerson newPerson = new ChargePerson();
            newPerson.personID = System.Guid.NewGuid();
            newPerson.personName = chargePerson.personName;

            ChargePerson recordValue = _unitOfWork.ChargePersonRepository.Add(newPerson);
            
            ChargePersonDTO returnValue = new ChargePersonDTO() { personID = recordValue.personID, personName = recordValue.personName };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }

        public bool DeletePerson(System.Guid id)
        {
            if (_unitOfWork.ChargePersonRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                {
                    return true;
                }
               
            }
            return false;
        }

        public List<ChargePersonDTO> GetAllPersons()
        {
           List<ChargePerson> chargePeoplelist=_unitOfWork.ChargePersonRepository.GetAll().OrderBy(x=>x.personName).ToList();
            List<ChargePersonDTO> list = new List<ChargePersonDTO>();
            if (chargePeoplelist == null)
            {
                return null;
            }
            foreach (ChargePerson point in chargePeoplelist)
            {
                ChargePersonDTO returnValue = new ChargePersonDTO() { personID = point.personID, personName = point.personName };
                list.Add(returnValue);
            }
            
            return list;
        }

        public ChargePersonDTO GetPersonById(System.Guid id)
        {
            ChargePerson value = _unitOfWork.ChargePersonRepository.GetById(id);

            if (value == null)
                return null;
            ChargePersonDTO retValue = new ChargePersonDTO() { personID = value.personID, personName = value.personName };

            return retValue;
        }

        public ChargePersonDTO UpdatePerson(ChargePersonDTO chargePerson)
            
        {
            if (chargePerson == null)
            {
                return null;
            }
            ChargePerson updatePerson = new ChargePerson();
            updatePerson.personID = chargePerson.personID;
            updatePerson.personName = chargePerson.personName;
            ChargePerson recordValue =_unitOfWork.ChargePersonRepository.Update(updatePerson);
            
            ChargePersonDTO returnValue = new ChargePersonDTO() { personID = recordValue.personID, personName = recordValue.personName };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }
      

    }

}
