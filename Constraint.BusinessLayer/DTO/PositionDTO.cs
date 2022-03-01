using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.DTO
{
    public class PositionDTO
    {
        public int positionID { get; set; }
        public string materialCode { get; set; }
        public string customerCode { get; set; }
        public string customerText { get; set; }
        public Nullable<System.DateTime> registerDate { get; set; }
        public string delayReason { get; set; }
        public string amount { get; set; }
        public string companyTeam { get; set; }
        public string chargePerson { get; set; }
    }
}
