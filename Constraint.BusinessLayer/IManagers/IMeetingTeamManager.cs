using System;
using Constraint.BusinessLayer.DTO;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
    public interface IMeetingTeamManager
    {
        MeetingTeamDTO CreateMeetingTeam(MeetingTeamDTO Meeting);
        MeetingTeamDTO GetMeetingTeamById(System.Guid id);
        bool DeleteMeetingTeam(System.Guid id);
        List<MeetingTeamDTO> GetAllMeetingTeams();
        MeetingTeamDTO UpdateMeetingTeam(MeetingTeamDTO Meeting);
        /* bool DeleteAllMeeting(List<MeetingTeamDTO> meetings);
         List<MeetingTeamDTO> GetSubIndustry();*/
        byte[] ExporttoExcel();
        byte[] ExporttoExcelByDate(DateTime startDate, DateTime endDate);
        List<TeamNameDTO> GetTeamNamesAndSum();
        List<TeamReasonDTO> GetTeamReasonsAndSum();


        List<TeamNameDTO> GetTeamNamesAndSumByDate(DateTime startDate, DateTime endDate);

        List<TeamReasonDTO> GetTeamReasonsAndSumByDate(DateTime startDate, DateTime endDate);
      

    }
}
