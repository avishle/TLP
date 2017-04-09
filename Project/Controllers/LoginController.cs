using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using TLP.DAL;
using TLP.Models;
using TLP.Security;

namespace TLP.Controllers
{
    public class LoginController : Controller
    {
       private DataAccessLayer uDAL = new DataAccessLayer();
        private IUserRepository userRepo;
        private int test = 1;
        public LoginController ()
        {
            this.userRepo = new EFUserRepository();
        }
        public LoginController(IUserRepository userRepo)
        {
            this.userRepo = userRepo;
            this.test = 0;
        }
        // GET: Login
        public ActionResult Index()
        {

            return View("Login", new User());
        }
        [HttpPost]
        public ActionResult Authenticate(User loginUser)
        {
            
            Encryption enc = new Encryption(); //#
            loginUser.uid = 0;
            if (ModelState.IsValid)
            {
                /* List<User> user2check = (from u in userRepo.Users
                                         where (u.username == loginUser.username)                                               
                                         select u).ToList<User>(); //#
                                         */
                var testing = userRepo.Users.ToList<User>(); //not good
                var userListResult = userRepo.Users.Where(user => user.username.Equals(loginUser.username)).ToList<User>();
                if (userListResult.Count != 0) //#
                {
                    if (enc.ValidatePassword( loginUser.password, userListResult.ElementAt(0).password)) //#
                    {
                        
                        setCookies(userListResult.ElementAt(0));
                        var stam = RedirectToRoute("Home");
                        var stamstam = stam.GetType();
                        return stam;
                    }
                    ViewBag.ErrorMessage = "wrong username and/or password";
                    return View("Login", loginUser);
                }
                ViewBag.ErrorMessage = "wrong username and/or password";
                return View("Login", loginUser);
            }
            ViewBag.ErrorMessage = "wrong username and/or password";
            return View("Login", loginUser);

        }
        private void setCookies(User u)
        {
            if (test == 1) //not test
            { 
                FormsAuthentication.SetAuthCookie("loginCookie", true);
                HttpCookie myCookie = new HttpCookie("UserPuzzel");
                myCookie["username"] = u.username;
                myCookie["a"] = u.puzzle_a.ToString();
                myCookie["n"] = u.puzzle_n.ToString();
                myCookie["time"] = u.puzzle_t.ToString();
                myCookie["pKey"] = u.puzzle_key.ToString();
                myCookie.Expires = DateTime.Now.AddHours(24);
                Response.Cookies.Add(myCookie);
            }
        }
        public ActionResult Registrate()
        {
            return View(new User ());
        }

        [HttpPost]
        public ActionResult AddNewUser(User regUser)
        {
            
            
            Encryption enc = new Encryption(); //#
            regUser.puzzle_a = 1;
            regUser.puzzle_t = 1;
            regUser.puzzle_n = 1;
            regUser.puzzle_key = 1;
            if (ModelState.IsValid)
            {
                List<User> userLogin = (from u in userRepo.Users
                                        where (u.username == regUser.username) 
                                        select u).ToList<User>();
                if (userLogin.Count == 0)
                {
                    string hashedPassword = enc.CreateHash(regUser.password); //#
                    regUser.password = hashedPassword; //#
                    userRepo.Save(regUser);                    
                    FormsAuthentication.SetAuthCookie("loginCookie", true);
                    setCookies(regUser);
                    return RedirectToAction("MyHome", "Customer", null);
                }
                return View("Registrate", regUser);
            }
            return View("Registrate", regUser);
        }
    }
    
}