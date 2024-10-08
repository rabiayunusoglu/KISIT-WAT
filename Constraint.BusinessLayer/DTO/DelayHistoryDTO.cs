﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.DTO
{
   public  class DelayHistoryDTO
    {
        public bool isMarked { get; set; }
        public bool isArchive { get; set; }
        public System.Guid delayID { get; set; }
        public string productCode { get; set; }
        public string delayAmount { get; set; }
        public string delayCode { get; set; }
        public string delayDate { get; set; }
        public string delayReason { get; set; }
        public string delayDetail { get; set; }
        public string companyTeam { get; set; }
        public string chargePerson { get; set; }
        public string madeDate { get; set; }
        public string boundConstraintID { get; set; }
        public string boundMontageID { get; set; }

    }
}
