using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
     public interface ICompanyTeamManager
    {
        CompanyTeamDTO CreateTeam(CompanyTeamDTO companyTeam);
        CompanyTeamDTO GetTeamById(System.Guid id);
        bool DeleteTeam(System.Guid id);
        List<CompanyTeamDTO> GetAllTeams();
        CompanyTeamDTO UpdateTeam(CompanyTeamDTO companyTeam);
        List<CompanyTeamDTO> GetHaveCodeTeams();

    }
}
