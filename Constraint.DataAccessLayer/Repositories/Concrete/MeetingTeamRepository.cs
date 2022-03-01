
using Constraint.DataAccessLayer.Repositories.Abstract;
namespace Constraint.DataAccessLayer.Repositories.Concrete
{
    class MeetingTeamRepository: Repository<MeetingTeam>, IMeetingTeamRepository
    {
        public MeetingTeamRepository(ConstraintDBEntities dBEntities) : base(dBEntities)
        {

        }
        public ConstraintDBEntities ConstraintDBEntities { get { return _context as ConstraintDBEntities; } }
    }
}
