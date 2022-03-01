
using Constraint.DataAccessLayer.Repositories.Abstract;

namespace Constraint.DataAccessLayer.Repositories.Concrete
{
    class PositionRepository:Repository<Position>,IPositionRepository
    {
        public PositionRepository(ConstraintDBEntities dBEntities) : base(dBEntities)
        {

        }
        public ConstraintDBEntities ConstraintDBEntities { get { return _context as ConstraintDBEntities; } }

    }
}
