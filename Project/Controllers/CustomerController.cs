using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Web.Security;
using System.Web.Http;
using TLP.DAL;
using TLP.Models;
using TLP.ViewModel;

namespace TLP.Controllers
{
    [System.Web.Mvc.Authorize]
    public class CustomerController : Controller
    {
        // GET: Customer
        private DataAccessLayer uDAL = new DataAccessLayer();
        private IUserRepository userRepo;
        private IAuctionRespository auctionRepo;
        public CustomerController()
        {
            this.userRepo = new EFUserRepository();
            this.auctionRepo = new EFAuctionRepository();
        }
        public CustomerController(IAuctionRespository auctionRepo)
        {
            this.userRepo = new EFUserRepository();
            this.auctionRepo = auctionRepo;
        }
        public ActionResult Index()
        {
            return View();
        }
        
        public ActionResult MyHome()
        {
            
            User loggedUser = new User();
            var stam = Request.Cookies;
            if(stam.Keys.Count!=2)//&& stam.Keys.Get(1).ToString() != "UserPuzzel")
            {
                return LogOut();
            }
            loggedUser.username = Request.Cookies["UserPuzzel"]["username"];

            return View(loggedUser);
        }
        
        public ActionResult GetAuctionsByJson()
        {
            var AuctionResult = auctionRepo.Auctions.Where(auction => auction.deadLine.CompareTo(DateTime.Today)>=0);    //returns null--> fixed         
            
            return Json(AuctionResult, JsonRequestBehavior.AllowGet);
        }
        public ActionResult LogOut()
        {
            FormsAuthentication.SignOut();
            return RedirectToAction("Index", "Login", null);
        }
        public ActionResult BidAuction(int aid)
        {

            return View("OnlineSubmission");//send user details and Auction id
        }
        
        [System.Web.Http.HttpPost]
        public ActionResult getBid([FromBody] submission subbid)
        {
           /* string id = "Hello";*/
            if( subbid.username==null)
            {
                submission stam = new submission { username = "kobisa", a_id = 1, time = DateTime.UtcNow };
                return Json(stam, JsonRequestBehavior.AllowGet);
            }
            enterBid2DB(subbid);    
            return Json("Success", JsonRequestBehavior.AllowGet);

        }
        private void enterBid2DB(submission subb)
        {

        }
    }
}