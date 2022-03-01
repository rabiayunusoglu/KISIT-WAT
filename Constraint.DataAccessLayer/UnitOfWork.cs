using Constraint.DataAccessLayer.Repositories.Abstract;
using Constraint.DataAccessLayer.Repositories.Concrete;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.DataAccessLayer
{
    public class UnitOfWork : IUnitOfWork
    {
        private ConstraintDBEntities _context;
        public UnitOfWork(ConstraintDBEntities context)
        {
            _context = context;
            ArchiveConstraintRepository = new ArchiveConstraintRepository(_context);
            ChargePersonRepository = new ChargePersonRepository(_context);
            CompanyTeamRepository = new CompanyTeamRepository(_context);
            ConstraintRepository = new ConstraintRepository(_context);
            DelayHistoryRepository = new DelayHistoryRepository(_context);
            MontageProductPlanRepository = new MontageProductPlanRepository(_context);
            MeetingRepository = new MeetingRepository(_context);
            MeetingTeamRepository = new MeetingTeamRepository(_context);
            PostponementReasonRepository = new PostponementReasonRepository(_context);
            ProductTreeRepository = new ProductTreeRepository(_context);
            UserRepository = new UserRepository(_context);
            VersionRepository = new VersionRepository(_context);


        }

        public IArchiveConstraintRepository ArchiveConstraintRepository { get; private set; }

        public IChargePersonRepository ChargePersonRepository { get; private set; }

        public ICompanyTeamRepository CompanyTeamRepository { get; private set; }

        public IConstraintRepository ConstraintRepository { get; private set; }

        public IDelayHistoryRepository DelayHistoryRepository { get; private set; }

        public IMontageProductPlanRepository MontageProductPlanRepository { get; private set; }

        public IMeetingRepository MeetingRepository { get; private set; }
        public IMeetingTeamRepository MeetingTeamRepository { get; private set; }

        public IPostponementReasonRepository PostponementReasonRepository { get; private set; }

        public IProductTreeRepository ProductTreeRepository { get; private set; }

        public IUserRepository UserRepository { get; private set; }

        public IVersionRepository VersionRepository { get; private set; }

        public int Complete()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
