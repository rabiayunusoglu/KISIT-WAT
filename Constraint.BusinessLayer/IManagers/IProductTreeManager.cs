using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
    public interface IProductTreeManager
    {
        ProductTreeDTO CreateTree(ProductTreeDTO tree);
        ProductTreeDTO GetTreeById(System.Guid id);
        bool DeleteTree(System.Guid id);
        List<ProductTreeDTO> GetAllTrees();
        ProductTreeDTO UpdateTree(ProductTreeDTO tree);
     
        bool UpdateConstraintTable();
        bool GetDataFromExcelFile(string filename, Stream stream);
    }
}
