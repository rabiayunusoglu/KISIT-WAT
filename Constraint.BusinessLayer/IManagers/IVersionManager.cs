using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
   public  interface IVersionManager
    {
        VersionDTO CreateVersion(VersionDTO version);
        VersionDTO GetVersionById(System.Guid id);
        bool DeleteVersion(System.Guid id);
        List<VersionDTO> GetAllVersions();
        VersionDTO UpdateVersion(VersionDTO version);
    }
}
