using Auctions.Data;
using Auctions.Data.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Auctions.Web.Controllers
{
    [Authorize(Roles = "User")]
    public class OrdersController : Controller
    {
        private IAuctionData data = AuctionData.Instance;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Orders       
        public ActionResult Index(OrderMessageId? message, int? page)
        {
            logger.InfoFormat("Index: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                message, 
                page
            }));

            ViewBag.StatusMessage =
               message == OrderMessageId.Error ? "An error has occurred."
               : message == OrderMessageId.CompleteSuccess ? "Order was Completed Successfully"
               : message == OrderMessageId.OrderFail ? "Order has failed"
               : message == OrderMessageId.SubmitSuccess ? "Order was submitted sucessfully"
               : "";

            var model = data.GetOrdersByUserId(User.Identity.GetUserId(), page);
            if (model == null)
            {
                return RedirectToAction("Index", new { message = OrderMessageId.Error });
            }
            return View(model);
        }

        public ActionResult Create()
        {
            logger.InfoFormat("Create: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
            }));

            var dv = data.GetDetailsDefaultValues();

            if(dv == null)
            {
                RedirectToAction("Index", new { message = OrderMessageId.Error });
            }
            ViewBag.platinum = dv.PlatinuTokenNumber;
            ViewBag.silver = dv.SilverTokenNumber;
            ViewBag.gold = dv.GoldTokenNumber;
            ViewBag.tokenPrice = dv.TokenValue;
            ViewBag.currency = dv.Currency;

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create(CreateOrderViewModel model)
        {
            logger.InfoFormat("Create-POST: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                model
            }));

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
                var dv = data.GetDetailsDefaultValues();

                if (dv == null)
                {
                   return RedirectToAction("Index", new { message = OrderMessageId.Error });
                }
                ViewBag.platinum = dv.PlatinuTokenNumber;
                ViewBag.silver = dv.SilverTokenNumber;
                ViewBag.gold = dv.GoldTokenNumber;
                ViewBag.tokenPrice = dv.TokenValue;
                ViewBag.currency = dv.Currency;
            }
            return View(model);
        }

        public ActionResult OrderUpdate(string clientid, string status)
        {
            logger.InfoFormat("OrderUpdate: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                clientid,
                status
            }));

            if (status == "success")
            {
                if (data.SetOrderStatus(clientid, OrderStatus.SUBMITTED))
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.SubmitSuccess });
                } else
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.Error });
                }
            }
            else
            {
                if (data.SetOrderStatus(clientid, OrderStatus.CANCELED))
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.OrderFail });
                }
                else
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.Error });
                }
            }
        }

        public ActionResult OrderCompleted(string clientid, string status)
        {
            logger.InfoFormat("OrderCompleted: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                clientid,
                status
            }));

            if (status == "success")
            {
                if (data.SetOrderStatus(clientid, OrderStatus.COMPLETED))
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.CompleteSuccess });
                }
                else
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.Error });
                }
            }
            else
            {
                if (data.SetOrderStatus(clientid, OrderStatus.CANCELED))
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.OrderFail });
                }
                else
                {
                    return RedirectToAction("Index", new { message = OrderMessageId.Error });
                }
            }
        }

        public enum OrderMessageId
        {
            SubmitSuccess, 
            CompleteSuccess,
            OrderFail,
            Error
        }
    }
}