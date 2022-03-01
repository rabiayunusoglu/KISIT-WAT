using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.DTO
{
   public class UserDTO
    {

        public System.Guid userID { get; set; }

        public string userType { get; set; }

        public string password { get; set; }

        public string email { get; set; }

        public Nullable<bool> isActive { get; set; }
        public Nullable<bool> permissionForConstraint { get; set; }

        public Nullable<System.DateTime> createdTime { get; set; }
        public string userName { get; set; }

    }
}
