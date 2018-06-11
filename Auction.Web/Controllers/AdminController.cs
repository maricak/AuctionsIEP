using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Mvc;
using Auction.Data;
using Auction.Data.Models;

namespace Auction.Web.Controllers
{
    [Authorize(Roles = "Admin")]
    public class AdminController : Controller
    {
        private AuctionDB db = new AuctionDB();
        private IAuctionData data = AuctionData.Instance;

        // GET: DefaultValues/
        public ActionResult Index()
        {
            if (TempData["Message"] != null)
            {
                ViewBag.Message = TempData["Message"];
                TempData.Remove("Message");
            }

            DetailsDefaultValuesViewModel dv = data.GetDetailsDefaultValues();
            ICollection<AdminAuctionViewModel> auctions = data.GetReadyAuctions();

            if (dv == null || auctions == null)
            {
                ViewBag.Message = "Data was not found";
            }

            AdminIndexViewModel model = new AdminIndexViewModel
            {
                Auctions = auctions,
                DefaultValues = dv
            };

            return View(model);
        }

        // GET: DefaultValues/Edit/5
        public ActionResult Edit()
        {
            EditDefaultValuesViewModel model = data.GetEditDefaultValues();
            if (model == null)
            {
                TempData["Message"] = "Data was not found";
                RedirectToAction("Index");
            }
            return View(model);
        }

        // POST: DefaultValues/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "NumberOfAuctionsPerPage,AuctionDuration,SilverTokenNumber,GoldTokenNumber,PlatinuTokenNumber,TokenValue,Currency")] EditDefaultValuesViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (data.SetDefaultValues(model))
                {
                    TempData["Message"] = "Changes were made successfully";
                }
                else
                {
                    TempData["Message"] = "Couldn't save the changes";
                }
                return RedirectToAction("Index");
            }
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult OpenAuction(string id)
        {
            if (data.OpenAuction(id))
            {
                TempData["Message"] = "Auction is opened";
            }
            else
            {
                TempData["Message"] = "Couldn't save the changes";
            }
            return RedirectToAction("Index");
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }
    }
}
