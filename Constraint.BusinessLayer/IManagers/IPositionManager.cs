using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
    public interface IPositionManager
    {
        PositionDTO CreatePosition(PositionDTO position);
        PositionDTO GetPositionById(int id);
        bool DeletePosition(int id);
        List<PositionDTO> GetAllPositons();
        PositionDTO UpdatePosition(PositionDTO position);
    }
}
