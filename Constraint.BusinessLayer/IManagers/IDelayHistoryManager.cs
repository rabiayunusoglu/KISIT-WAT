using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
    public interface IDelayHistoryManager
    {

        DelayHistoryDTO Createhistory(DelayHistoryDTO history);
        DelayHistoryDTO GetHistoryById(System.Guid id);
        bool Deletehistory(System.Guid id);
        List<DelayHistoryDTO> GetAllHistories();
        DelayHistoryDTO UpdateHistory(DelayHistoryDTO history);
    }
}
