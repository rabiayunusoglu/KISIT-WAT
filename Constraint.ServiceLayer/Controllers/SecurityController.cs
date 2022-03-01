using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Constraint.BusinessLayer.Managers;
using Constraint.BusinessLayer.DTO;
using System.Web.Security;
using System.Threading.Tasks;

namespace Constraint.ServiceLayer.Controllers
{
    public class SecurityController : Controller
    {
        UserManager manager = new UserManager();

        // GET: Login
        [AllowAnonymous]
        public ActionResult Login()
        {
            if (Session["Block"] == null)
                return View();
            return RedirectToAction("Index");
        }
        [HttpPost]
        [AllowAnonymous]
        public ActionResult Login(UserDTO user)
        {
            if(Session["Block"] == null)
            {
                var userInfo = manager.GetAllUsers().Where(x => x.email == user.email && x.password.Trim() == user.password).ToList();
                Console.Write(userInfo);
                if (userInfo.Count != 0)
                {
                    FormsAuthentication.SetAuthCookie(userInfo[0].email, true);
                    HttpContext.Response.SetCookie(new HttpCookie("email", userInfo[0].email));
                    Session["Email"] = userInfo[0].userName;
                    Session["Block"] = userInfo[0].userType;
                    if (userInfo[0].userType == "A")
                        return RedirectToAction("Index", "Home");
                    else if (userInfo[0].userType == "U")
                        return RedirectToAction("Index", "HomeUser");
                    return RedirectToAction("Login");
                }
                else
                    return RedirectToAction("Login");
            }
            return RedirectToAction("Index");
        }
        [AllowAnonymous]
        public ActionResult Index()
        {
            if (Session["Block"] == null)
            {
                return RedirectToAction("Login");
            }
            if (Session["Block"].ToString() == "A")
                return RedirectToAction("Index", "Home");
            else if (Session["Block"].ToString() == "U")
                return RedirectToAction("Index", "HomeUser");
            return RedirectToAction("Login");
        }
        [HttpGet]
        public ActionResult Logout()
        {
            Session["Block"] = null;
            Session["Email"] = null;
            FormsAuthentication.SignOut();
            return RedirectToAction("Login");
        }
       
    }
}