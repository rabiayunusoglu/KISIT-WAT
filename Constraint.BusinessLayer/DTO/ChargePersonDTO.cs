using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.DTO
{
    public class ChargePersonDTO
    {
        public System.Guid personID { get; set; }
        [Required(ErrorMessage="İsim girilmesi zorunludur.")]
        public string personName { get; set; }
    }
}
