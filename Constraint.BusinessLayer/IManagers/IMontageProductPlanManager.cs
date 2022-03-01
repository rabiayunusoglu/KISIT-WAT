using Constraint.BusinessLayer.DTO;
using Constraint.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
    public interface IMontageProductPlanManager
    {
        MontageProductPlanDTO CreatePlan(MontageProductPlanDTO plan);
        MontageProductPlanDTO GetPlanById(System.Guid id);
        bool DeletePlan(System.Guid id);
        List<MontageProductPlanDTO> GetAllPlans();
        MontageProductPlanDTO UpdatePlan(MontageProductPlanDTO plan);
        bool UpdateConstraintTable();
        bool GetDataFromExcelFile(string filename, Stream stream);
        MontageProductPlan ChangeVersion(MontageProductPlan list);
    }
}
