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
    public class CompanyTeamManager : ICompanyTeamManager
    {
        private UnitOfWork _unitOfWork;
        public CompanyTeamManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public CompanyTeamDTO CreateTeam(CompanyTeamDTO companyTeam)
        {
            if (companyTeam == null)
            {
                return null;
            }
            CompanyTeam newTeam = new CompanyTeam();
            newTeam.companyID = System.Guid.NewGuid();
            newTeam.companyName = companyTeam.companyName;
            newTeam.companyCode = companyTeam.companyCode;
            CompanyTeam recordValue = _unitOfWork.CompanyTeamRepository.Add(newTeam);
            CompanyTeamDTO returnValue = new CompanyTeamDTO() { companyID = recordValue.companyID, companyName = recordValue.companyName, companyCode = recordValue.companyCode };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }

        public bool DeleteTeam(System.Guid id)
        {
            if (id == null)
                return false;
            if (_unitOfWork.CompanyTeamRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                {
                    return true;
                }

            }
            return false;
        }

        public List<CompanyTeamDTO> GetAllTeams()
        {
            List<CompanyTeam> teamList = _unitOfWork.CompanyTeamRepository.GetAll().OrderBy(x => x.companyName).ToList();
            List<CompanyTeamDTO> list = new List<CompanyTeamDTO>();
            if (teamList == null)
            {
                return null;
            }
            foreach (CompanyTeam point in teamList)
            {
                CompanyTeamDTO returnValue = new CompanyTeamDTO() { companyID = point.companyID, companyName = point.companyName, companyCode = point.companyCode };
                list.Add(returnValue);
            }
            return list;
        }

        public CompanyTeamDTO GetTeamById(System.Guid id)
        {
            if (id == null)
                return null;
            CompanyTeam value = _unitOfWork.CompanyTeamRepository.GetById(id);

            if (value == null)
                return null;
            CompanyTeamDTO returnValue = new CompanyTeamDTO() { companyID = value.companyID, companyName = value.companyName, companyCode = value.companyCode };
            return returnValue;
        }

        public CompanyTeamDTO UpdateTeam(CompanyTeamDTO companyTeam)
        {
            if (companyTeam == null)
            {
                return null;
            }
            CompanyTeam updateTeam = new CompanyTeam();
            updateTeam.companyID = companyTeam.companyID;
            updateTeam.companyName = companyTeam.companyName;
            updateTeam.companyCode = companyTeam.companyCode;
            CompanyTeam recordValue = _unitOfWork.CompanyTeamRepository.Update(updateTeam);
            CompanyTeamDTO returnValue = new CompanyTeamDTO() { companyID = recordValue.companyID, companyName = recordValue.companyName, companyCode = recordValue.companyCode };
            if (_unitOfWork.Complete() > 0)
            {
                return returnValue;
            }
            return null;
        }
        public List<CompanyTeamDTO> GetHaveCodeTeams()
        {
            List<CompanyTeam> teamList = _unitOfWork.CompanyTeamRepository.GetAll().Where(t => !t.companyCode.Equals("-")).ToList();
            List<CompanyTeamDTO> list = new List<CompanyTeamDTO>();
            if (teamList == null)
            {
                return null;
            }
            foreach (CompanyTeam point in teamList)
            {
                CompanyTeamDTO returnValue = new CompanyTeamDTO() { companyID = point.companyID, companyName = point.companyName, companyCode = point.companyCode };
                list.Add(returnValue);
            }
            return list;
        }
        public List<CompanyTeamDTO> GetOnlyTeams()
        {
            List<CompanyTeam> teamList = _unitOfWork.CompanyTeamRepository.GetAll().Where(t => t.companyCode.Equals("-")).ToList();
            List<CompanyTeamDTO> list = new List<CompanyTeamDTO>();
            if (teamList == null)
            {
                return null;
            }
            foreach (CompanyTeam point in teamList)
            {
                CompanyTeamDTO returnValue = new CompanyTeamDTO() { companyID = point.companyID, companyName = point.companyName, companyCode = point.companyCode };
                list.Add(returnValue);
            }
            return list;
        }

    }
}
