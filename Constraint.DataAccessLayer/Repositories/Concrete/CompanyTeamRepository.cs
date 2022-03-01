using System;
using System.Collections.Generic;
using System.Linq;
using Constraint.DataAccessLayer.Repositories.Abstract;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.DataAccessLayer.Repositories.Concrete
{
    class CompanyTeamRepository:Repository<CompanyTeam>, ICompanyTeamRepository
    {
        public CompanyTeamRepository(ConstraintDBEntities dBEntities) : base(dBEntities)
        {

        }
        public ConstraintDBEntities ConstraintDBEntities { get { return _context as ConstraintDBEntities; } }

    }
}
