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
    public class DefaultValuesController : Controller
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

            EditDefaultValuesViewModel model = data.GetDetailsDefaultValues();
            if (model == null)
            {
                return HttpNotFound();
            }
            return View(model);
        }

        // GET: DefaultValues/Edit/5
        public ActionResult Edit()
        {
            EditDefaultValuesViewModel model = data.GetEditDefaultValues();
            if (model == null)
            {
                return HttpNotFound();
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
                    TempData["Message"] = "Couldn't save changes";
                }
                return RedirectToAction("Index");
            }
            return View(model);
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
