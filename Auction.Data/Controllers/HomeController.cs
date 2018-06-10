using Auction.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.Data.Controllers
{
    public class HomeController : Controller
    {
        public ActionResult Index()
        {
            using (AuctionDB db = new AuctionDB())
            {
                var auction = db.Auctions.Include("User").FirstOrDefault();

                return View("Auction", auction);
            }
        }

        public ActionResult About()
        {
            ViewBag.Message = "Your application description page.";

            return View();
        }

        public ActionResult Contact()
        {
            ViewBag.Message = "Your contact page.";

            return View();
        }
    }
}