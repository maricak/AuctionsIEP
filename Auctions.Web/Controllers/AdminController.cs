using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Auctions.Data;
using Auctions.Data.Models;
using Microsoft.AspNet.Identity;
using Newtonsoft.Json;

namespace Auctions.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private IAuctionData data = AuctionData.Instance;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: DefaultValues/
        public ActionResult Index(AdminMessageId? message)
        {
            logger.InfoFormat("Index: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                message,
            }));

            ViewBag.StatusMessage =
                message == AdminMessageId.AuctionOpenSuccess ? "Auction is opened."
                : message == AdminMessageId.ChangeDefaultValuesSuccess ? "Changes were made successfully."
                : message == AdminMessageId.Error ? "An error has occurred."
                : "";

            DetailsDefaultValuesViewModel dv = data.GetDetailsDefaultValues();
            ICollection<AdminAuctionViewModel> auctions = data.GetReadyAuctions();

            if (dv == null || auctions == null)
            {
                ViewBag.StatusMessage += "</br> Data was not found.";
            }

            AdminIndexViewModel model = new AdminIndexViewModel
            {
                Auctions = auctions,
                DefaultValues = dv
            };

            return View(model);
        }

        // GET: DefaultValues/Edit/
        public ActionResult Edit()
        {
            logger.InfoFormat("Edit: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
            }));

            EditDefaultValuesViewModel model = data.GetEditDefaultValues();
            if (model == null)
            {
                return RedirectToAction("Index", new { Message = AdminMessageId.Error });
            }
            return View(model);
        }

        // POST: DefaultValues/Edit
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NumberOfAuctionsPerPage,AuctionDuration,SilverTokenNumber,GoldTokenNumber,PlatinuTokenNumber,TokenValue,Currency")] EditDefaultValuesViewModel model)
        {
            logger.InfoFormat("Edit-POST: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                model
            }));

            if (ModelState.IsValid)
            {
                if (data.SetDefaultValues(model))
                {
                    return RedirectToAction("Index", new { Message = AdminMessageId.ChangeDefaultValuesSuccess });
                }
                else
                {
                    return RedirectToAction("Index", new { Message = AdminMessageId.Error });
                }
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpenAuction(string id)
        {
            logger.InfoFormat("OpenAuction: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                id
            }));
            if (data.OpenAuction(id))
            {
                return RedirectToAction("Index", new { Message = AdminMessageId.AuctionOpenSuccess });
            }
            else
            {
                return RedirectToAction("Index", new { Message = AdminMessageId.Error });
            }
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                //db.Dispose();
            }
            base.Dispose(disposing);
        }

        public enum AdminMessageId
        {
            AuctionOpenSuccess, 
            ChangeDefaultValuesSuccess,
            Error
        }
    }
}
