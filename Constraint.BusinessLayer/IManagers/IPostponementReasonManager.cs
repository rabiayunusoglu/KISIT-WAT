using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
    public interface IPostponementReasonManager
    {
        PostponementReasonDTO CreateReason(PostponementReasonDTO reason);
        PostponementReasonDTO GetReasonById(System.Guid id);
        bool DeleteReason(System.Guid id);
        List<PostponementReasonDTO> GetAllReasons();
        PostponementReasonDTO UpdateReason(PostponementReasonDTO reason);
    }
}
