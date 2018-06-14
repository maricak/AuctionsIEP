using Auction.Data;
using Auction.Data;
using Auction.Data.Models;
using Microsoft.AspNet.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auction.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class OrdersController : Controller
    {
        private IAuctionData data = AuctionData.Instance;

        // GET: Orders       

        public ActionResult Index(OrderMessageId? message)
        {
            ViewBag.StatusMessage =
               message == OrderMessageId.Error ? "An error has occurred."
               : "";

            var model = data.GetOrdersByUserId(User.Identity.GetUserId());
            if (model == null)
            {
                return RedirectToAction("Index", new { message = OrderMessageId.Error });
            }
            return View(model);
        }

        public ActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateOrderViewModel model)
        {
            if (ModelState.IsValid)
            {
                OrderPackage package = model.Package == "SILVER" ? OrderPackage.SILVER
                    : model.Package == "GOLD" ? OrderPackage.GOLD
                    : model.Package == "PLATINUM" ? OrderPackage.PLATINUM
                    : OrderPackage.ERROR;

                string userId = User.Identity.GetUserId();

                if (String.IsNullOrEmpty(userId) || package == OrderPackage.ERROR)
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.Error });
                }

                Guid? orderId = data.CreateOrder(package, userId);              
               
                if (orderId == null)
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.Error });
                }

                
                return Redirect("https://stage.centili.com/widget/WidgetModule?api=564f47646ce7f954ad981d6b1b6166fc&country=RS&countrylock=true&clientid=" + orderId.ToString());
            }
            else
            {
                RedirectToAction("Index", "Auction");
            }

            return View(model);
        }

        public ActionResult Centili(string clientid, string status)
        {
            return View(clientid);
        }



        public enum OrderMessageId
        {
            Error
        }
    }
}