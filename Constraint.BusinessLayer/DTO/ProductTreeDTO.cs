using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.DTO
{
    public class ProductTreeDTO
    {
        public System.Guid treeID { get; set; }
        public string productCode { get; set; }
        public string productText { get; set; }
        public Nullable<int> UA_klnm { get; set; }
        public Nullable<int> alt_UA { get; set; }
        public string higherCode { get; set; }
        public string higherCodeText { get; set; }
        public string sparePart { get; set; }
        public string materialCode { get; set; }
        public string materialText { get; set; }
        public string amount { get; set; }
        public string mip { get; set; }
        public string tob { get; set; }
        public string mtu { get; set; }
        public string goodGroup { get; set; }
        public string validityDate { get; set; }
        public string finishDate { get; set; }
        public string tdrType { get; set; }

    }
}
