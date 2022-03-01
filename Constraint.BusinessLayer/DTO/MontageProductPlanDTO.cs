using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.DTO
{
    public class MontageProductPlanDTO
    {
        public System.Guid montageID { get; set; }
        public string productCode { get; set; }
        public string version { get; set; }
        public string plannedDate { get; set; }
        public string amount { get; set; }
        public string customer { get; set; }
        public string customerType { get; set; }
        public string frame { get; set; }
        public string relevant_month { get; set; }
        public string aboveLine { get; set; }
    }
}
