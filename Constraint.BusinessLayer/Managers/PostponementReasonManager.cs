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
    public class PostponementReasonManager : IPostponementReasonManager
    {
        private UnitOfWork _unitOfWork;
        public PostponementReasonManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public PostponementReasonDTO CreateReason(PostponementReasonDTO reason)
        {
            if (reason == null)
            {
                return null;
            }
            Postponement_reasons newReason = new Postponement_reasons();
            newReason.delayID = System.Guid.NewGuid();
            newReason.delayName = reason.delayName;

            Postponement_reasons recordValue = _unitOfWork.PostponementReasonRepository.Add(newReason);

            PostponementReasonDTO returnValue = new PostponementReasonDTO() { delayID = recordValue.delayID, delayName = recordValue.delayName };
            if(_unitOfWork.Complete()>0)
            return returnValue;
            return null;
        }

        public bool DeleteReason(System.Guid id)
        {
            if (_unitOfWork.PostponementReasonRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                    return true;
                
            }
            return false;
        }

        public List<PostponementReasonDTO> GetAllReasons()
        {
            List<Postponement_reasons> reasonlist = _unitOfWork.PostponementReasonRepository.GetAll().OrderBy(x => x.delayName).ToList();
            List<PostponementReasonDTO> list = new List<PostponementReasonDTO>();
            if (reasonlist == null)
            {
                return null;
            }
            foreach (Postponement_reasons recordValue in reasonlist)
            {
                PostponementReasonDTO returnValue = new PostponementReasonDTO() { delayID = recordValue.delayID, delayName = recordValue.delayName };
                list.Add(returnValue);
            }

            return list;
        }

        public PostponementReasonDTO GetReasonById(System.Guid id)
        {
            Postponement_reasons recordValue = _unitOfWork.PostponementReasonRepository.GetById(id);

            PostponementReasonDTO returnValue = new PostponementReasonDTO() { delayID = recordValue.delayID, delayName = recordValue.delayName };
           
            return returnValue;
        }

        public PostponementReasonDTO UpdateReason(PostponementReasonDTO reason)
        {
            if (reason == null)
            {
                return null;
            }
            Postponement_reasons newReason = new Postponement_reasons();
            newReason.delayID = reason.delayID;
            newReason.delayName = reason.delayName;

            Postponement_reasons recordValue = _unitOfWork.PostponementReasonRepository.Update(newReason);

            PostponementReasonDTO returnValue = new PostponementReasonDTO() { delayID = recordValue.delayID, delayName = recordValue.delayName };
            if (_unitOfWork.Complete() > 0)
                return returnValue;
            return null;
        }
    }
}
