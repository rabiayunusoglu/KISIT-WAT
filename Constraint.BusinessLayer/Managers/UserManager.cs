using Constraint.BusinessLayer.DTO;
using Constraint.BusinessLayer.IManagers;
using Constraint.DataAccessLayer;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Constraint.BusinessLayer.Managers
{
    public class UserManager : IUserManager
    {
        private UnitOfWork _unitOfWork;
        public UserManager()
        {
            _unitOfWork = new UnitOfWork(new ConstraintDBEntities());


        }
        public UserDTO CreateUser(UserDTO user)
        {
            if (user == null)
            {
                return null;
            }
            User newuser = new User();
            newuser.userID = System.Guid.NewGuid();
            newuser.userType = user.userType;
            newuser.password = user.password;
            newuser.email = user.email;
            newuser.isActive = user.isActive;
            newuser.permissionForConstraint = user.permissionForConstraint;
            newuser.createdTime = user.createdTime;
            newuser.userName = user.userName;
            User recordValue = _unitOfWork.UserRepository.Add(newuser);

            UserDTO returnValue = new UserDTO()
            {
                userID = recordValue.userID,
                userType = recordValue.userType,
                password = recordValue.password,
                email = recordValue.email,
                isActive = recordValue.isActive,
                permissionForConstraint = recordValue.permissionForConstraint,
                createdTime = recordValue.createdTime,
                userName = recordValue.userName

            };
            if (_unitOfWork.Complete() > 0)
                return returnValue;
            return null;
        }

        public bool DeleteUser(System.Guid id)
        {
            if (_unitOfWork.UserRepository.Remove(id))
            {
                if (_unitOfWork.Complete() > 0)
                    return true;

            }
            return false;
        }

        public List<UserDTO> GetAllUsers()
        {
            List<User> list = _unitOfWork.UserRepository.GetAll().OrderBy(x => x.email).ToList();
            List<UserDTO> dtoList = new List<UserDTO>();
            if (list == null)
            {
                return null;
            }
            foreach (User recordValue in list)
            {
                UserDTO returnValue = new UserDTO()
                {
                    userID = recordValue.userID,
                    userType = recordValue.userType,
                    password = recordValue.password,
                    email = recordValue.email,
                    isActive = recordValue.isActive,
                    permissionForConstraint = recordValue.permissionForConstraint,
                    createdTime = recordValue.createdTime,
                    userName = recordValue.userName

                };
                dtoList.Add(returnValue);
            }
            return dtoList;
        }

        public UserDTO GetUserById(System.Guid id)
        {
            User recordValue = _unitOfWork.UserRepository.GetById(id);
            if (recordValue == null)
                return null;
            UserDTO returnValue = new UserDTO()
            {
                userID = recordValue.userID,
                userType = recordValue.userType,
                password = recordValue.password,
                email = recordValue.email,
                isActive = recordValue.isActive,
                permissionForConstraint = recordValue.permissionForConstraint,
                createdTime = recordValue.createdTime,
                userName = recordValue.userName

            };
            return returnValue;
        }

        public UserDTO UpdateUser(UserDTO user)
        {
            if (user == null)
            {
                return null;
            }
            User newuser = new User()
            {
                userID = user.userID,
                userType = user.userType,
                password = user.password,
                email = user.email,
                isActive = user.isActive,
                permissionForConstraint = user.permissionForConstraint,
                createdTime = user.createdTime,
                userName = user.userName
            };
            User recordValue = _unitOfWork.UserRepository.Update(newuser);

            UserDTO returnValue = new UserDTO()
            {
                userID = recordValue.userID,
                userType = recordValue.userType,
                password = recordValue.password,
                email = recordValue.email,
                isActive = recordValue.isActive,
                permissionForConstraint = recordValue.permissionForConstraint,
                createdTime = recordValue.createdTime,
                userName = recordValue.userName

            };
            if (_unitOfWork.Complete() > 0)
                return returnValue;
            return null;
        }


        public bool Login(string username, string password)
        {
            using (ConstraintDBEntities entities = new ConstraintDBEntities())
            {
                return entities.Users.Any(u => u.email.Equals(username, StringComparison.OrdinalIgnoreCase) && u.password == password);
            }
        }
        public string GetUserType(string username)
        {
            using (ConstraintDBEntities entities = new ConstraintDBEntities())
            {
                var temp = entities.Users.Where(u => u.email.Equals(username, StringComparison.OrdinalIgnoreCase));
                if (temp != null)
                {
                    User userLogin = temp.First();
                    return userLogin.userType;
                }
                return null;
            }
        }
        public UserDTO GetUserByEmail(string username)
        {
            using (ConstraintDBEntities entities = new ConstraintDBEntities())
            {

                User recordValue = entities.Users.FirstOrDefault(x => x.email == username);
                if (recordValue != null)
                {
                    UserDTO value = new UserDTO()
                    {
                        userID = recordValue.userID,
                        userType = recordValue.userType,
                        password = recordValue.password,
                        email = recordValue.email,
                        isActive = recordValue.isActive,
                        permissionForConstraint = recordValue.permissionForConstraint,
                        createdTime = recordValue.createdTime,
                        userName = recordValue.userName

                    };
                    return value;
                }
                return null;
            }
        }

    }
}
