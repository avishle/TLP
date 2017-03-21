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
        [HttpPost]
        public ActionResult Authenticate(User loginUser)
        {
            DataAccessLayer uDAL = new DataAccessLayer();
            if (ModelState.IsValid)
            {
                List<User> userLogin = (from u in uDAL.Users
                                        where (u.username == loginUser.username) &&
                                               (u.password == loginUser.password)
                                        select u).ToList<User>();
                if (userLogin.Count == 1)
                {
                    FormsAuthentication.SetAuthCookie("loginCookie", true);
                    setPuzzleCookie(userLogin.ElementAt(0));
                    
                    return RedirectToRoute("Home");
                }
                return View("Login", loginUser);
            }
            return View("Login", loginUser);

        }
        private void setPuzzleCookie(User u)
        {
            HttpCookie myCookie = new HttpCookie("UserPuzzel");
            myCookie["a"] = u.puzzle_a.ToString();
            myCookie["n"] = u.puzzle_n.ToString();
            myCookie["time"] = u.puzzle_t.ToString();
            myCookie["pKey"] = u.puzzle_key.ToString();
            myCookie.Expires = DateTime.Now.AddHours(24);
            Response.Cookies.Add(myCookie);
        }
       
      
        public ActionResult Registrate()
        {
            return View(new User ());
        }

        [HttpPost]
        public ActionResult AddNewUser(User regUser)
        {
            DataAccessLayer uDAL = new DataAccessLayer();
            regUser.puzzle_a = 1;
            regUser.puzzle_t = 1;
            regUser.puzzle_n = 1;
            regUser.puzzle_key = 1;
            if (ModelState.IsValid)
            {
                List<User> userLogin = (from u in uDAL.Users
                                        where (u.username == regUser.username) 
                                        select u).ToList<User>();
                if (userLogin.Count == 0)
                {
                    DataAccessLayer cDAL = new DataAccessLayer();
                    cDAL.Users.Add(regUser);
                    cDAL.SaveChanges();
                    FormsAuthentication.SetAuthCookie("loginCookie", true);
                    setPuzzleCookie(regUser);
                    return RedirectToRoute("Home");
                }
                return View("Registrate", regUser);
            }
            return View("Registrate", regUser);
        }
    }
    
}