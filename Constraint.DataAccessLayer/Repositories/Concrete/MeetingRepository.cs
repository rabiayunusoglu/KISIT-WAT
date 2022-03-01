
using Constraint.DataAccessLayer.Repositories.Abstract;

namespace Constraint.DataAccessLayer.Repositories.Concrete
{
    class MeetingRepository:Repository<Meeting>,IMeetingRepository
    {
        public MeetingRepository(ConstraintDBEntities dBEntities) : base(dBEntities)
        {

        }
        public ConstraintDBEntities ConstraintDBEntities { get { return _context as ConstraintDBEntities; } }

    }
}
