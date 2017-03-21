using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TLP.DAL;
using TLP.Models;

namespace TLP.Controllers
{
    public class LoginController : Controller
    {
        // GET: Login
        public ActionResult Index()
        {
            return View("Login", new User());
        }
        public ActionResult Authenticate(User loginUser)
        {
            DataAccessLayer uDAL = new DataAccessLayer();
            if (ModelState.IsValid)
            {
                List<User> userLogin = (from u in uDAL.Users
                                        where (u.userName == loginUser.userName) &&
                                               (u.password == loginUser.password)
                                        select u).ToList<User>();
                if (userLogin.Count == 1)
                {
                    FormsAuthentication.SetAuthCookie("loginCookie", true);
                    return RedirectToRoute("Home");
                }
                return View("Login", loginUser);
            }
            return View("Login", loginUser);

        }
        
        public ActionResult Registrate()
        {
            return View();
        }
    }
}