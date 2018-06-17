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

namespace Auctions.Web.Controllers
{
    
    public class AuctionsController : Controller
    {
        private AuctionDB db = new AuctionDB();
        private IAuctionData data = AuctionData.Instance;

        // GET: Auctions
        public ActionResult Index(AuctionMessageId? message)
        {
            ViewBag.StatusMessage =
                message == AuctionMessageId.CreateSuccess ? "Auction was created successfully"
                : message == AuctionMessageId.Error ? "An error has occurred."
                : "";

            var auctions = data.GetAllOpenedAuctions();
            if (auctions == null)
            {
                ViewBag.StatusMessage += "</br> Data was not found.";
            }
            return View(auctions);
        }

        // GET: Auctions/Details/5
        public ActionResult Details(string id)
        {
            var model = data.GetAuctionById(id);
            if (model == null)
            {
                return RedirectToAction("Index", new { message = AuctionMessageId.Error });
            }
             else
            {
                return View(model);
            }
        }

        [Authorize(Roles = "User")]
        // GET: Auctions/Create
        public ActionResult Create()
        {
            var dv = data.GetDetailsDefaultValues();
            if (dv == null)
            {
                RedirectToAction("Index", new { message = AuctionMessageId.Error });
            }

            var model = new CreateAuctionViewModel
            {
                Currency = dv.Currency,
                Duration = dv.AuctionDuration
            };
            return View(model);
        }

        // POST: Auctions/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [Authorize(Roles = "User")]
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Create([Bind(Include = "Name,Image,Duration,StartPrice,Currency")] CreateAuctionViewModel model)
        {
            if (ModelState.IsValid)
            {
                //if (data.CreateAuction(model, User.Identity.GetUserId()))
                if(data.CreateAuction(model))
                {
                    return RedirectToAction("Index", new { message = AuctionMessageId.CreateSuccess });
                } else
                {
                    return RedirectToAction("Index", new { message = AuctionMessageId.Error });

                }
            }

            return View(model);
        }

        // GET: Auctions/Edit/5
        public ActionResult Edit(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        // POST: Auctions/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see https://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Edit([Bind(Include = "Id,Name,Image,Duration,StartPrice,CurrentPrice,Currency,CreatingTime,OpeningTime,ClosingTime,Status")] Auction auction)
        {
            if (ModelState.IsValid)
            {
                db.Entry(auction).State = EntityState.Modified;
                db.SaveChanges();
                return RedirectToAction("Index");
            }
            return View(auction);
        }

        // GET: Auctions/Delete/5
        public ActionResult Delete(Guid? id)
        {
            if (id == null)
            {
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest);
            }
            Auction auction = db.Auctions.Find(id);
            if (auction == null)
            {
                return HttpNotFound();
            }
            return View(auction);
        }

        // POST: Auctions/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public ActionResult DeleteConfirmed(Guid id)
        {
            Auction auction = db.Auctions.Find(id);
            db.Auctions.Remove(auction);
            db.SaveChanges();
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

        public enum AuctionMessageId
        {
            CreateSuccess,
            Error
        }
    }
}
