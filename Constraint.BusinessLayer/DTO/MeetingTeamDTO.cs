﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.DTO
{
    public class MeetingTeamDTO
    {
        public System.Guid constraintID { get; set; }

        public string materialCode { get; set; }

        public string materialText { get; set; }

        public string productCode { get; set; }

        public string plannedDate { get; set; }

        public string amount { get; set; }

        public string customer { get; set; }

        public string version { get; set; }

        public string delayID { get; set; }

        public string delayCode { get; set; }

        public string delayAmount { get; set; }

        public string delayDate { get; set; }

        public string delayReason { get; set; }

        public string delayDetail { get; set; }

        public string companyTeam { get; set; }

        public string chargePerson { get; set; }

        public string dateCurrent { get; set; }
    }
}