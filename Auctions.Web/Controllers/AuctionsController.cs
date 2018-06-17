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
        public ActionResult Index(AuctionMessageId? message, string searchString, decimal? lowPrice, decimal? highPrice, AuctionStatus? status, int? page)
        {
            ViewBag.StatusMessage =
                message == AuctionMessageId.CreateSuccess ? "Auction was created successfully"
                : message == AuctionMessageId.Error ? "An error has occurred."
                : "";

            ViewBag.SearchString = searchString;
            ViewBag.LowPrice = lowPrice;
            ViewBag.HighPrice = highPrice;
            ViewBag.Status = status;           

            if (status == null)
            {
                ViewBag.Status = 0;
            }
            else
            {
                ViewBag.Status = (int)status;
            }

            var auctions = data.GetAllOpenedAuctions(searchString, lowPrice, highPrice, status, page);
            if (auctions == null)
            {
                ViewBag.StatusMessage += "An error has occurred";
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

        [Authorize(Roles = "User")]
        public ActionResult Wins(int? page)
        {
            var model = data.GetAuctionsByWinner(User.Identity.GetUserId(), page);
            if (model == null)
            {
                return RedirectToAction("Index", new { message = AuctionMessageId.Error });
            }
            else
            {
                return View(model);
            }
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
