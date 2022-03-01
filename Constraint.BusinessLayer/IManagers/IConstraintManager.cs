using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
    public interface IConstraintManager
    {
        ConstraintDTO CreateConstraint(ConstraintDTO constraint);
        ConstraintDTO GetConstraintById(System.Guid id);
        bool DeleteConstraint(System.Guid id);
        List<ConstraintDTO> GetAllConstraints();
        ConstraintDTO UpdateConstraint(ConstraintDTO constraint);

        byte[] ExporttoExcel(bool isMark);
    }
}
