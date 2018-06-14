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
        public ActionResult Index(OrderMessageId? message)
        {
            ViewBag.StatusMessage =
                  message == OrderMessageId.Error ? "An error has occurred."
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

        public ActionResult Orders()
        {
            var model = data.GetOrdersByUserId(User.Identity.GetUserId());
            if (model == null)
            {
                return RedirectToAction("Index", new { message = OrderMessageId.Error });
            }
            return View(model);
        }

        public enum OrderMessageId
        {
            Error
        }
    }
}