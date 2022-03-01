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
    public class VersionManager : IVersionManager
    {
        private UnitOfWork _unitOfWork;
        public VersionManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public VersionDTO CreateVersion(VersionDTO version)
        {
            if (version == null)
            {
                return null;
            }
            Constraint.DataAccessLayer.Version newversion = new Constraint.DataAccessLayer.Version();

            newversion.versionID = System.Guid.NewGuid();
            newversion.versionName = version.versionName;
            newversion.versionValue = version.versionValue;
            
            Constraint.DataAccessLayer.Version recordValue = _unitOfWork.VersionRepository.Add(newversion);
           
            VersionDTO returnValue = new VersionDTO()
            {
            versionID = recordValue.versionID,
            versionName = recordValue.versionName,
            versionValue = recordValue.versionValue,

        };
            if (_unitOfWork.Complete() > 0)
                return returnValue;
            return null;
        }

        public bool DeleteVersion(System.Guid id)
        {
            if (_unitOfWork.VersionRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                    return true;
             
            }
            return false;
        }

        public List<VersionDTO> GetAllVersions()
        {
        List<Constraint.DataAccessLayer.Version> list = _unitOfWork.VersionRepository.GetAll().OrderBy(x => x.versionName).ToList();
            List<VersionDTO> dtoList = new List<VersionDTO>();
            if (list == null)
            {
                return null;
            }
            foreach (Constraint.DataAccessLayer.Version recordValue in list)
        {
                VersionDTO returnValue = new VersionDTO()
            {
                versionID = recordValue.versionID,
                versionName = recordValue.versionName,
                versionValue = recordValue.versionValue,

            };
            dtoList.Add(returnValue);
        }
        return dtoList;
    }

        public VersionDTO GetVersionById(System.Guid id)
        {
            Constraint.DataAccessLayer.Version recordValue = _unitOfWork.VersionRepository.GetById(id);
            if (recordValue == null)
                return null;
            VersionDTO returnValue = new VersionDTO()
            {
                versionID = recordValue.versionID,
                versionName = recordValue.versionName,
                versionValue = recordValue.versionValue,

            };
            return returnValue;
        }

        public VersionDTO UpdateVersion(VersionDTO version)
        {
            if (version == null)
            {
                return null;
            }
            Constraint.DataAccessLayer.Version newversion = new Constraint.DataAccessLayer.Version();

            newversion.versionID = version.versionID;
            newversion.versionName = version.versionName;
            newversion.versionValue = version.versionValue;

            Constraint.DataAccessLayer.Version recordValue = _unitOfWork.VersionRepository.Update(newversion);
           
            VersionDTO returnValue = new VersionDTO()
            {
                versionID = recordValue.versionID,
                versionName = recordValue.versionName,
                versionValue = recordValue.versionValue,

            };
            if (_unitOfWork.Complete() > 0)
                return returnValue;
            return null;
        }
    }
}
