using Constraint.BusinessLayer.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.IManagers
{
    public interface IUserManager
    {
        UserDTO CreateUser(UserDTO User);
        UserDTO GetUserById(System.Guid id);
        bool DeleteUser(System.Guid id);
        List<UserDTO> GetAllUsers();
        UserDTO UpdateUser(UserDTO User);
        bool Login(string username, string password);
        UserDTO GetUserByEmail(string username);
        string GetUserType(string username);
    }
}
