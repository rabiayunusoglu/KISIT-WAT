using System;
using Constraint.DataAccessLayer.Repositories.Abstract;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.DataAccessLayer.Repositories.Concrete
{
    public class ArchiveConstraintRepository:Repository<ArchiveConstraint>, IArchiveConstraintRepository
    {
        public ArchiveConstraintRepository(ConstraintDBEntities dBEntities) : base(dBEntities)
        {

        }
        public ConstraintDBEntities ConstraintDBEntities { get { return _context as ConstraintDBEntities; } }
   
    }
}
