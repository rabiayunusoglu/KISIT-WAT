using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;


namespace Constraint.BusinessLayer.IManagers { 
   public interface IArchiveConstraintManager
    {
       ArchiveConstraintDTO CreateArchive(ArchiveConstraintDTO archiveConstraint);
        ArchiveConstraintDTO GetArchiveById(System.Guid id);
        bool DeleteArchive(System.Guid id);
        List<ArchiveConstraintDTO> GetAllArchives();
        ArchiveConstraintDTO UpdateArchive(ArchiveConstraintDTO archiveConstraint);
        byte[] ExporttoExcel();
        byte[] ExporttoExcelByDate(DateTime startDate, DateTime endDate);

    }
}
