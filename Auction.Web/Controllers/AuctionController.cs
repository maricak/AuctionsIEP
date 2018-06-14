using Auction.Data;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class AuctionController : Controller
    {
        private IAuctionData data = AuctionData.Instance;

        [AllowAnonymous]
        public ActionResult Index(AuctionMessageId? message)
        {
            ViewBag.StatusMessage =
                  message == AuctionMessageId.Error ? "An error has occurred."
                 : "";
            return View();
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

        

        public enum AuctionMessageId
        {
            Error
        }
    }
}