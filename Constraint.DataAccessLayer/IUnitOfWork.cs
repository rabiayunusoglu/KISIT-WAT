using Constraint.DataAccessLayer.Repositories.Abstract;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.DataAccessLayer
{
    public interface IUnitOfWork : IDisposable
    {
        IArchiveConstraintRepository ArchiveConstraintRepository { get;   }

        ICompanyTeamRepository CompanyTeamRepository { get;   }

        IConstraintRepository ConstraintRepository { get;   }
        IDelayHistoryRepository DelayHistoryRepository { get;   }

        IMontageProductPlanRepository MontageProductPlanRepository { get;   }

        IMeetingRepository MeetingRepository { get;   }
        IMeetingTeamRepository MeetingTeamRepository { get; }
        IPostponementReasonRepository PostponementReasonRepository { get;   }

        IProductTreeRepository ProductTreeRepository { get;   }
        IUserRepository UserRepository { get;   }
        IVersionRepository VersionRepository { get;   }




        int Complete();
    }
}
