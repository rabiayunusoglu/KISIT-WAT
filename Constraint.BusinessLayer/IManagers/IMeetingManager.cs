using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
    public interface IMeetingManager
    {
        MeetingDTO CreateMeeting(MeetingDTO Meeting);
        MeetingDTO GetMeetingById(System.Guid id);
        bool DeleteMeeting(System.Guid id);
        List<MeetingDTO> GetAllMeetings();
        MeetingDTO UpdateMeeting(MeetingDTO Meeting);
       /* bool DeleteAllMeeting(List<MeetingDTO> meetings);
        List<MeetingDTO> GetSubIndustry();*/
        byte[] ExporttoExcel();
    }
}
