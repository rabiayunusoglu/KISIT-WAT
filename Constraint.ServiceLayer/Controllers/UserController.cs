using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;
namespace Constraint.ServiceLayer.Controllers
{
    public class UserController : Controller
    {
        UserManager manager = new UserManager();
        [Authorize(Roles = "A,U")]
        public JsonResult GetManager()
        {
            var _list = manager.GetAllUsers();
            var jsonResult = Json(_list, JsonRequestBehavior.AllowGet);
            jsonResult.MaxJsonLength = int.MaxValue;
            return jsonResult;

        }
        [Authorize(Roles = "A,U")]
        public JsonResult Details(Guid id)
        {
            var _listManager = manager.GetUserById(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public JsonResult Create(UserDTO managerDTO)
        {
            var user=manager.CreateUser(managerDTO);

            return Json(user, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "A,U")]
        public JsonResult Edit(UserDTO managerDTO)
        {
            var _listManager = manager.UpdateUser(managerDTO);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult BlockOrUnblock(UserDTO[] managerDTO, bool isBlock)
        {
            if(managerDTO==null || managerDTO.Length==0)
                return Json(false, JsonRequestBehavior.AllowGet);
            for (var i=0;i<managerDTO.Length;i++)
            {
                managerDTO[i].permissionForConstraint = isBlock;
                var _listManager = manager.UpdateUser(managerDTO[i]);
                if(_listManager==null)
                    return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json(true, JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        [Authorize(Roles = "A")]
        public JsonResult Delete(Guid id)
        {
            var _listManager = manager.DeleteUser(id);
            return Json(_listManager, JsonRequestBehavior.AllowGet);
        }
        public JsonResult HasPermissionConstraint(bool hasPermissionConstraint)
        {
            List<UserDTO> userList = manager.GetAllUsers();
            if (userList != null || userList.Count != 0)
            {
                //kullanici sisteme girdiginde hic kisit engeli var mi kontrolu yapiliyor.
                Boolean type = userList.Any(x => x.permissionForConstraint == hasPermissionConstraint);
                if (type)
                {
                    //kisit giris kitlidir.
                    return Json(true, JsonRequestBehavior.AllowGet);
                }
                //yani kisitta bloklama yoksa, kisit giris kitli degildir diyor.
                return Json(false, JsonRequestBehavior.AllowGet);
            }
            return Json("value", JsonRequestBehavior.AllowGet);
        }
        [HttpPost]
        public JsonResult Control(Guid id)
        {
            if (Session["ID"].ToString() == id.ToString())
                return Json(true, JsonRequestBehavior.AllowGet);
            return Json(false, JsonRequestBehavior.AllowGet);
        }
    }
}