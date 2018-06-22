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
using X.PagedList;


namespace Auctions.Web.Controllers
{
    public class AuctionsController : Controller
    {
        private IAuctionData data = AuctionData.Instance;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        // GET: Auctions
        public ActionResult Index(AuctionMessageId? message, string searchString, decimal? lowPrice, decimal? highPrice, AuctionStatus? status, int? page)
        {
            logger.InfoFormat("Index: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                message,
                searchString,
                lowPrice,
                highPrice,
                status,
                page
            }));

            ViewBag.StatusMessage =
                message == AuctionMessageId.CreateSuccess ? "Auction was created successfully"
                : message == AuctionMessageId.BidSuccess ? "You made a bid"
                : message == AuctionMessageId.Error ? "An error has occurred."
                : "";

            ViewBag.SearchString = searchString;
            ViewBag.LowPrice = lowPrice;
            ViewBag.HighPrice = highPrice;
            ViewBag.Status = status;

            var auctions = data.GetAllOpenedAuctions(searchString, lowPrice, highPrice, status, page);
            if (auctions == null)
            {
                ViewBag.StatusMessage += "An error has occurred";
                auctions = new PagedList<AuctionViewModel>(new List<AuctionViewModel>(), 1, 10);
            }
            return View(auctions);
        }

        // GET: Auctions/Details/5
        public ActionResult Details(string id, int? page)
        {
            logger.InfoFormat("Details: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                id,
                page
            }));

            var model = data.GetAuctionDetailsById(id, page);
            if (model == null)
            {
                return RedirectToAction("Index", new { message = AuctionMessageId.Error });
            }
            else
            {
                page = page ?? 1;
                ViewBag.Page = page;
                return View(model);
            }
        }

        [Authorize(Roles = "User")]
        // GET: Auctions/Create
        public ActionResult Create()
        {
            logger.InfoFormat("Create: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
            }));
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
            logger.InfoFormat("Create-POST: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                model.Name,
                model.Duration, 
                model.Currency, 
                model.StartPrice
            }));

            if (ModelState.IsValid)
            {
                //if (data.CreateAuction(model, User.Identity.GetUserId()))
                if (data.CreateAuction(model))
                {
                    return RedirectToAction("Index", new { message = AuctionMessageId.CreateSuccess });
                }
                else
                {
                    return RedirectToAction("Index", new { message = AuctionMessageId.Error });

                }
            }
            return View(model);
        }

        [Authorize(Roles = "User")]
        public ActionResult Wins(int? page)
        {
            logger.InfoFormat("Wins: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                page
            }));

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

        [HttpPost]
        [ValidateAntiForgeryToken]
        //[Authorize(Roles = "User")]
        public dynamic Bid(string id, long? offer)
        {
            logger.InfoFormat("Bid: {0}", JsonConvert.SerializeObject(new
            {
                user = User.Identity.GetUserName(),
                id,
                offer
            }));
            if (Request.IsAuthenticated && User.IsInRole("User"))
            {
                DetailsAuctionViewModel auction = null;
                if (offer != null)
                {
                    if (data.MakeBid(id, offer, User.Identity.GetUserId()))
                    {
                        auction = data.GetAuctionDetailsById(id, 1);
                        if (auction != null)
                        {
                            return JsonConvert.SerializeObject(new
                            {
                                id,
                                bidder = auction.LastBidder,
                                currentPrice = auction.CurrentPrice,
                                currency = auction.Currency,
                                tokens = auction.CurrentNumberOfTokens,
                                success = true,
                                message = "You made a bid",
                                lastBid = auction.Bids[0]
                            });
                        }
                    }
                }
                return JsonConvert.SerializeObject(new
                {
                    id,
                    success = false,
                    message = "Error has occured"
                });
            }
            else
            {
                return JsonConvert.SerializeObject(new
                {
                    id,
                    success = false,
                    message = "You have to log in"
                });
            }
        }

        public enum AuctionMessageId
        {
            CreateSuccess,
            BidSuccess,
            Error
        }
    }
}
