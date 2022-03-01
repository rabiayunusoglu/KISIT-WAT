using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Constraint.DataAccessLayer.Repositories.Abstract;

namespace Constraint.DataAccessLayer.Repositories.Concrete
{
    class DelayHistoryRepository:Repository<DelayHistory>,IDelayHistoryRepository
    {
        public DelayHistoryRepository(ConstraintDBEntities dBEntities) : base(dBEntities)
        {

        }
        public ConstraintDBEntities ConstraintDBEntities { get { return _context as ConstraintDBEntities; } }

    }
}
