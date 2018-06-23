using System;
using System.Collections.Generic;
using System.Data.Entity;
using Microsoft.AspNet.Identity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auctions.Data.Models;
using System.IO;
using System.Drawing;
using System.Collections;
using X.PagedList;
using LinqKit;
using Newtonsoft.Json;

[assembly: log4net.Config.XmlConfigurator(ConfigFile = "App.config", Watch = true)]
namespace Auctions.Data
{
    public class AuctionData : IAuctionData
    {
        private static AuctionData Singleton = null;
        readonly log4net.ILog logger = log4net.LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public static AuctionData Instance
        {
            get
            {
                if (Singleton == null)
                {
                    Singleton = new AuctionData();
                }
                return Singleton;
            }
        }
        private AuctionData() { }

        #region DefaultValues
        public DetailsDefaultValuesViewModel GetDetailsDefaultValues()
        {
            logger.InfoFormat("GetDetailsDefaultValues: {0}", JsonConvert.SerializeObject(new
            {
            }));

            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var defaultValues = db.DefaultValues.SingleOrDefault();
                    if (defaultValues != null)
                    {
                        logger.InfoFormat("GetDetailsDefaultValues: {0}", JsonConvert.SerializeObject(new
                        {
                            defaultValues
                        }));
                        return new DetailsDefaultValuesViewModel
                        {
                            AuctionDuration = defaultValues.AuctionDuration,
                            Currency = defaultValues.Currency,
                            SilverTokenNumber = defaultValues.SilverTokenNumber,
                            GoldTokenNumber = defaultValues.GoldTokenNumber,
                            PlatinuTokenNumber = defaultValues.PlatinuTokenNumber,
                            NumberOfAuctionsPerPage = defaultValues.NumberOfAuctionsPerPage,
                            TokenValue = defaultValues.TokenValue
                        };
                    }
                    else
                    {
                        logger.ErrorFormat("GetDetailsDefaultValues: {0} defaultValues is null", JsonConvert.SerializeObject(new
                        {
                        }));
                        return null;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("GetDetailsDefaultValues: ", e);
            }
            // something went wrong
            return null;
        }
        public EditDefaultValuesViewModel GetEditDefaultValues()
        {
            logger.InfoFormat("GetEditDefaultValues: {0}", JsonConvert.SerializeObject(new
            {
            }));
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var defaultValues = db.DefaultValues.SingleOrDefault();
                    if (defaultValues != null)
                    {
                        logger.InfoFormat("GetDetailsDefaultValues: {0}", JsonConvert.SerializeObject(new
                        {
                            defaultValues
                        }));
                        return new EditDefaultValuesViewModel
                        {
                            AuctionDuration = defaultValues.AuctionDuration,
                            Currency = defaultValues.Currency,
                            SilverTokenNumber = defaultValues.SilverTokenNumber,
                            GoldTokenNumber = defaultValues.GoldTokenNumber,
                            PlatinuTokenNumber = defaultValues.PlatinuTokenNumber,
                            NumberOfAuctionsPerPage = defaultValues.NumberOfAuctionsPerPage,
                            TokenValue = defaultValues.TokenValue
                        };
                    }
                    else
                    {
                        logger.ErrorFormat("GetEditDefaultValues: {0} defaultValues is null", JsonConvert.SerializeObject(new
                        {
                        }));
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("GetDetailsDefaultValues: ", e);
            }
            // something went wrong
            return null;
        }
        public bool SetDefaultValues(EditDefaultValuesViewModel model)
        {
            logger.InfoFormat("SetDefaultValues: {0}", JsonConvert.SerializeObject(new
            {
                model
            }));

            if (model == null)
            {
                logger.ErrorFormat("GetEditDefaultValues: {0} model is null", JsonConvert.SerializeObject(new
                {
                }));
                return false;
            }
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    DefaultValues defaultValues = db.DefaultValues.SingleOrDefault();
                    if (defaultValues == null)
                    {
                        logger.InfoFormat("SetDefaultValues: {0} defaultValues is null", JsonConvert.SerializeObject(new
                        {
                        }));
                        defaultValues = new DefaultValues();
                        db.Entry(defaultValues).State = EntityState.Added;
                    }
                    else
                    {
                        db.Entry(defaultValues).State = EntityState.Modified;
                    }
                    defaultValues.AuctionDuration = model.AuctionDuration;
                    defaultValues.Currency = model.Currency;
                    defaultValues.NumberOfAuctionsPerPage = model.NumberOfAuctionsPerPage;
                    defaultValues.SilverTokenNumber = model.SilverTokenNumber;
                    defaultValues.GoldTokenNumber = model.GoldTokenNumber;
                    defaultValues.PlatinuTokenNumber = model.PlatinuTokenNumber;
                    defaultValues.TokenValue = model.TokenValue;

                    db.SaveChanges();

                    logger.InfoFormat("SetDefaultValues: {0} new defaultValues", JsonConvert.SerializeObject(new
                    {
                    }));

                    return true;
                }
            }
            catch (Exception e)
            {
                logger.Error("SetDefaultValues", e);
            }

            //something went wrong
            return false;
        }
        #endregion


        #region Auctions
        private void CloseAuctions()
        {
            logger.InfoFormat("CloseAuctions: {0}", JsonConvert.SerializeObject(new
            {
            }));
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var now = DateTime.UtcNow;

                    var auctions = db.Auctions.Where(a => a.ClosingTime <= now && a.Status != AuctionStatus.COMPLETED).Include(a => a.Bids.Select(b => b.User)).ToList();
                    foreach (var auction in auctions)
                    {
                        logger.InfoFormat("CloseAuctions: Auction {0} is closed", JsonConvert.SerializeObject(new
                        {
                            auction.Id,
                            auction.Name,
                            auction.CurrentPrice,
                            auction.Currency,
                        }));

                        auction.Status = AuctionStatus.COMPLETED;
                        db.Entry(auction).State = EntityState.Modified;

                        // za svakog korisnika vraca najveci bid
                        var results = auction.Bids.GroupBy(b => b.User).Select(g => new
                        {
                            tokens = g.Max(b => b.NumberOfTokens),
                            user = g.Key
                        });

                        // vratiti tokene svima koji nisu pobedili
                        foreach (var result in results)
                        {
                            // nije pobednik
                            if (auction.User != result.user)
                            {
                                logger.InfoFormat("CloseAuctions: user is getting tokens back {0}", JsonConvert.SerializeObject(new
                                {
                                    result.tokens,
                                    result.user.UserName
                                }));

                                result.user.NumberOfTokens += result.tokens;
                                db.Entry(result.user).State = EntityState.Modified;
                            }
                        }
                    }
                    db.SaveChanges();
                }
            }
            catch (Exception e)
            {
                logger.Error("CloseAuctions", e);
            }
        }
        public ICollection<AdminAuctionViewModel> GetReadyAuctions()
        {
            logger.InfoFormat("GetReadyAuctions: {0}", JsonConvert.SerializeObject(new
            {
            }));
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var auctions = db.Auctions.Where(a => a.Status == AuctionStatus.READY).ToList();
                    var result = new List<AdminAuctionViewModel>();
                    foreach (var auction in auctions)
                    {
                        result.Add(new AdminAuctionViewModel
                        {
                            CreatingTime = auction.CreatingTime.ToLocalTime(),
                            Currency = auction.Currency,
                            Duration = auction.Duration,
                            Name = auction.Name,
                            StartPrice = auction.StartPrice,
                            Image = auction.Image,
                            Id = auction.Id.ToString()
                        });
                    }
                    return result;
                }
            }
            catch (Exception e)
            {
                logger.Error("GetReadyAuctions", e);
            }

            // something went wrong
            return null;
        }
        public bool OpenAuction(string id)
        {
            logger.InfoFormat("OpenAuction: {0}", JsonConvert.SerializeObject(new
            {
                id
            }));
            try
            {
                if (String.IsNullOrEmpty(id))
                {

                    logger.ErrorFormat("OpenAuction: {0} id is null", JsonConvert.SerializeObject(new
                    {
                        id
                    }));
                    return false;
                }
                var guidId = new Guid(id);
                using (AuctionDB db = new AuctionDB())
                {
                    var auction = db.Auctions.Where(a => a.Id.Equals(guidId)).SingleOrDefault();
                    if (auction == null)
                    {
                        logger.ErrorFormat("OpenAuction: Auction {0} doesn't exist", JsonConvert.SerializeObject(new
                        {
                            auction
                        }));
                        return false;
                    }
                    auction.OpeningTime = DateTime.UtcNow;
                    auction.ClosingTime = DateTime.UtcNow.Add(TimeSpan.FromSeconds(auction.Duration));
                    auction.Status = AuctionStatus.OPENED;

                    db.Entry(auction).State = EntityState.Modified;
                    db.SaveChanges();

                    logger.InfoFormat("OpenAuction: Auction is {0} open", JsonConvert.SerializeObject(new
                    {
                        auction.Id,
                        auction.Name,
                        auction.OpeningTime,
                        auction.Duration,
                        auction.ClosingTime,
                        auction.CurrentPrice,
                        auction.Currency
                    }));

                    return true;
                }
            }
            catch (Exception e)
            {
                logger.Error("OpenAuction", e);
            }
            // something went wrong
            return false;
        }
        public IPagedList<AuctionViewModel> GetAllOpenedAuctions(string searchString, decimal? lowPrice, decimal? highPrice, AuctionStatus? status, int? page)
        {
            logger.InfoFormat("GetAllOpenedAuctions: {0}", JsonConvert.SerializeObject(new
            {
                searchString,
                lowPrice,
                highPrice,
                status,
                page
            }));

            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    CloseAuctions();

                    var auctions = db.Auctions.Include(a => a.User).Where(a => a.Status != AuctionStatus.READY);

                    if (!String.IsNullOrEmpty(searchString))
                    {
                        page = 1;

                        searchString.Trim();
                        var words = searchString.Split();

                        var predicate = PredicateBuilder.New<Auction>(false);
                        foreach (var word in words)
                        {
                            if (String.IsNullOrEmpty(word))
                            {
                                continue;
                            }
                            predicate = predicate.Or(a => a.Name.Contains(word));
                        }
                        auctions = auctions.Where(predicate);
                    }

                    if (lowPrice != null)
                    {
                        page = 1;
                        auctions = auctions.Where(a => a.CurrentPrice >= lowPrice);
                    }

                    if (highPrice != null)
                    {
                        page = 1;
                        auctions = auctions.Where(a => a.CurrentPrice <= highPrice);
                    }

                    if (status != null && status != AuctionStatus.READY)
                    {
                        page = 1;
                        if (status == AuctionStatus.OPENED || status == AuctionStatus.COMPLETED)
                        {
                            auctions = auctions.Where(a => a.Status == status);
                        }
                    }

                    auctions = auctions.OrderByDescending(a => a.OpeningTime);

                    var result = new List<AuctionViewModel>();
                    var dv = GetDetailsDefaultValues();
                    if (dv == null)
                    {
                        logger.ErrorFormat("GetAllOpenedAuctions: defaul values {0} is null", JsonConvert.SerializeObject(new
                        {
                            dv
                        }));
                        return null;
                    }

                    foreach (var auction in auctions.Include(a => a.Bids).ToList())
                    {
                        //var span = (auction.ClosingTime - DateTime.UtcNow) ?? new TimeSpan();

                        var token = auction.Bids.Count() == 0 ? 0 : auction.Bids.Max(b => b.NumberOfTokens) + 1;

                        result.Add(new AuctionViewModel
                        {
                            Id = auction.Id.ToString(),
                            Currency = auction.Currency,
                            //Duration = auction.Status == AuctionStatus.COMPLETED ? "00:00:00" : GetDuration(span),
                            ClosingTime = auction.ClosingTime.GetValueOrDefault(),
                            Name = auction.Name,
                            Status = auction.Status,
                            CurrentPrice = auction.CurrentPrice,
                            Image = auction.Image,
                            CurrentNumberOfTokens = Math.Max((long)(Math.Ceiling(auction.CurrentPrice / dv.TokenValue)), token),
                            LastBidder = auction.User != null ? (auction.User.Name + " " + auction.User.Surname) : ""
                        });
                    }

                    long pageSize = GetDetailsDefaultValues().NumberOfAuctionsPerPage;
                    int pageNumber = (page ?? 1);

                    return result.ToPagedList(pageNumber, (int)pageSize);
                }
            }
            catch (Exception e)
            {
                logger.Error("GetAllOpenedAuctions", e);
            }
            return null;
        }
        public bool CreateAuction(CreateAuctionViewModel model)
        {
            logger.InfoFormat("CreateAuction: {0}", JsonConvert.SerializeObject(new
            {
                model.Currency,
                model.Duration,
                model.Name,
                model.StartPrice
            }));

            try
            {
                // make image
                var image = model.Image;
                var imageBytes = new byte[image.ContentLength];
                image.InputStream.Read(imageBytes, 0, imageBytes.Length);
                // check if file is an image
                Image.FromStream(new MemoryStream(imageBytes)).Dispose();


                using (AuctionDB db = new AuctionDB())
                {
                    //DefaultValues defaultValues = db.DefaultValues.SingleOrDefault();
                    //if (defaultValues == null)
                    //{
                    //    logger.ErrorFormat("GetAuctionById: Default valuse {0} is null", JsonConvert.SerializeObject(new
                    //    {
                    //        defaultValues
                    //    }));
                    //    return false;
                    //}

                    Auction auction = new Auction
                    {
                        Id = Guid.NewGuid(),
                        Name = model.Name,
                        Image = imageBytes,
                        Duration = model.Duration,
                        StartPrice = model.StartPrice,
                        CurrentPrice = model.StartPrice,
                        CreatingTime = DateTime.UtcNow,
                        Status = AuctionStatus.READY,
                        Currency = model.Currency,
                    };
                    db.Auctions.Add(auction);
                    db.SaveChanges();

                    logger.InfoFormat("CreateAuction: new Auction {0}", JsonConvert.SerializeObject(new
                    {
                        auction.Id,
                        auction.Name,
                        auction.Duration,
                        auction.StartPrice,
                        auction.CreatingTime,
                        auction.Status,
                    }));

                    return true;
                }
            }
            catch (Exception e)
            {
                logger.Error("CreateAuction", e);
            }
            // something went wrong
            return false;
        }
        public AuctionViewModel GetAuctionById(string id)
        {
            logger.InfoFormat("GetAuctionById: {0}", JsonConvert.SerializeObject(new
            {
                id
            }));

            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    CloseAuctions();

                    Auction auction = db.Auctions.Include(a => a.Bids).Where(a => a.Id.ToString().Equals(id)).SingleOrDefault();
                    if (auction == null)
                    {
                        logger.ErrorFormat("GetAuctionById: Auction {0} is null", JsonConvert.SerializeObject(new
                        {
                            auction
                        }));

                        return null;
                    }
                    else
                    {
                        //var span = (auction.ClosingTime - DateTime.UtcNow) ?? new TimeSpan();
                        var dv = GetDetailsDefaultValues();
                        if (dv == null)
                        {
                            logger.ErrorFormat("GetAuctionById: Default values {0} is null", JsonConvert.SerializeObject(new
                            {
                                dv
                            }));
                            return null;
                        }
                        var token = auction.Bids.Count() == 0 ? 0 : auction.Bids.Max(b => b.NumberOfTokens) + 1;

                        var result = new AuctionViewModel
                        {
                            Id = auction.Id.ToString(),
                            Name = auction.Name,
                            Currency = auction.Currency,
                            CurrentPrice = auction.CurrentPrice,
                            //Duration = auction.Status == AuctionStatus.COMPLETED ? "00:00:00" : GetDuration(span),
                            ClosingTime = auction.ClosingTime.GetValueOrDefault(),
                            Image = auction.Image,
                            CurrentNumberOfTokens = Math.Max((long)(Math.Ceiling(auction.CurrentPrice / dv.TokenValue)), token),
                            LastBidder = auction.User != null ? (auction.User.Name + " " + auction.User.Surname) : "",
                            Status = auction.Status
                        };

                        logger.InfoFormat("GetAuctionById: Auction {0} ", JsonConvert.SerializeObject(new
                        {
                            result.Id,
                            result.Name,
                            result.Currency,
                            result.ClosingTime,
                            result.CurrentPrice,
                            result.CurrentNumberOfTokens,
                            result.LastBidder,
                            result.Status
                        }));

                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("GetAuctionById", e);
            }
            return null;
        }
        public DetailsAuctionViewModel GetAuctionDetailsById(string id, int? page)
        {
            logger.InfoFormat("GetAuctionDetailsById: {0} ", JsonConvert.SerializeObject(new
            {
                id,
                page
            }));
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    CloseAuctions();

                    Auction auction = db.Auctions.Include(a => a.Bids).Where(a => a.Id.ToString().Equals(id)).SingleOrDefault();
                    if (auction == null)
                    {
                        logger.ErrorFormat("GetAuctionDetailsById: auction {0} is null", JsonConvert.SerializeObject(new
                        {
                            auction
                        }));
                        return null;
                    }
                    else
                    {
                        //var span = (auction.ClosingTime - DateTime.UtcNow) ?? new TimeSpan();
                        var dv = GetDetailsDefaultValues();
                        if (dv == null)
                        {
                            logger.ErrorFormat("GetAuctionDetailsById: default values {0} is null", JsonConvert.SerializeObject(new
                            {
                                dv
                            }));
                            return null;
                        }
                        var token = auction.Bids.Count() == 0 ? 0 : auction.Bids.Max(b => b.NumberOfTokens) + 1;

                        var result = new DetailsAuctionViewModel
                        {
                            Id = auction.Id.ToString(),
                            Name = auction.Name,
                            Currency = auction.Currency,
                            CurrentPrice = auction.CurrentPrice,
                            //Duration = auction.Status == AuctionStatus.COMPLETED ? "00:00:00" : GetDuration(span),
                            ClosingTime = auction.ClosingTime.GetValueOrDefault(),
                            Image = auction.Image,
                            CurrentNumberOfTokens = Math.Max((long)(Math.Ceiling(auction.CurrentPrice / dv.TokenValue)), token),
                            LastBidder = auction.User != null ? (auction.User.Name + " " + auction.User.Surname) : "",
                            Status = auction.Status
                        };

                        var bids = new List<DetailsBidViewModel>();
                        foreach (var bid in auction.Bids.OrderByDescending(b => b.PlacingTime))
                        {
                            bids.Add(new DetailsBidViewModel
                            {
                                NumberOfTokens = bid.NumberOfTokens,
                                PlacingTime = bid.PlacingTime.ToLocalTime(),
                                User = bid.User.Name + " " + bid.User.Surname
                            });
                        }
                        long pageSize = 15;
                        int pageNumber = (page ?? 1);

                        result.Bids = bids.ToPagedList(pageNumber, (int)pageSize);

                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                logger.Error("GetAuctionDetailsById", e);
            }
            return null;
        }
        public IPagedList<AuctionViewModel> GetAuctionsByWinner(string userId, int? page)
        {
            logger.InfoFormat("GetAuctionsByWinner: {0} ", JsonConvert.SerializeObject(new
            {
                userId,
                page
            }));
            try
            {
                if (String.IsNullOrEmpty(userId))
                {
                    logger.ErrorFormat("GetAuctionsByWinner: userId {0} is null ", JsonConvert.SerializeObject(new
                    {
                        userId,
                    }));
                    return null;
                }

                using (AuctionDB db = new AuctionDB())
                {
                    CloseAuctions();

                    var auctions = db.Auctions.Where(a => a.User.Id.Equals(userId)).
                        Where(a => a.Status == AuctionStatus.COMPLETED).ToList();
                    var result = new List<AuctionViewModel>();
                    foreach (var auction in auctions)
                    {
                        var span = (auction.ClosingTime - DateTime.UtcNow) ?? new TimeSpan();

                        result.Add(new AuctionViewModel
                        {
                            Id = auction.Id.ToString(),
                            Currency = auction.Currency,
                            Name = auction.Name,
                            Status = auction.Status,
                            CurrentPrice = auction.CurrentPrice,
                            Image = auction.Image,
                            LastBidder = auction.User != null ? (auction.User.Name + " " + auction.User.Surname) : ""
                        });
                    }
                    if (page == null)
                    {
                        page = 1;
                    }
                    long pageSize = GetDetailsDefaultValues().NumberOfAuctionsPerPage;
                    int pageNumber = (page ?? 1);

                    return result.ToPagedList(pageNumber, (int)pageSize);
                }
            }
            catch (Exception e)
            {
                logger.Error("GetAuctionsByWinner", e);
            }
            return null;
        }
        #endregion


        #region Orders
        public IPagedList<IndexOrderViewModel> GetOrdersByUserId(string id, int? page)
        {
            logger.InfoFormat("GetOrdersByUserId: {0} ", JsonConvert.SerializeObject(new
            {
                id,
                page
            }));
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    //Guid.Parse(id); // throws exception if id cannot be converted to guid
                    //var orders = db.Orders.Where(o => o.User.Id.Equals(id)).ToList();
                    var orders = db.Users.Where(u => u.Id.Equals(id)).Include(u => u.Orders).SingleOrDefault().Orders;
                    var result = new List<IndexOrderViewModel>();
                    foreach (var order in orders)
                    {
                        result.Add(new IndexOrderViewModel
                        {
                            Currency = order.Currency,
                            Price = order.Price,
                            Status = order.Status,
                            NumberOfTokens = order.NumberOfTokens
                        });
                    }
                    if (page == null)
                    {
                        page = 1;
                    }
                    long pageSize = 15;
                    int pageNumber = (page ?? 1);

                    return result.ToPagedList(pageNumber, (int)pageSize);
                }
            }
            catch (Exception e)
            {
                logger.Error("GetOrdersByUserId", e);
            }

            //something went wrong
            return null;
        }

        public Guid? CreateOrder(OrderPackage package, string orderUserId)
        {
            logger.InfoFormat("CreateOrder: {0} ", JsonConvert.SerializeObject(new
            {
                package,
                orderUserId
            }));
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var defaultValues = db.DefaultValues.SingleOrDefault();
                    var orderUser = db.Users.Where(u => u.Id.Equals(orderUserId)).SingleOrDefault();
                    if (orderUser == null || defaultValues == null)
                    {
                        logger.ErrorFormat("CreateOrder: Something is null {0} ", JsonConvert.SerializeObject(new
                        {
                            orderUser,
                            defaultValues
                        }));
                        return null;
                    }
                    Order order = new Order
                    {
                        Currency = defaultValues.Currency,
                        Id = Guid.NewGuid(),
                        User = orderUser,
                        NumberOfTokens = (package == OrderPackage.SILVER ? defaultValues.SilverTokenNumber
                            : package == OrderPackage.GOLD ? defaultValues.GoldTokenNumber
                            : defaultValues.PlatinuTokenNumber),
                        Status = OrderStatus.CANCELED
                    };
                    order.Price = order.NumberOfTokens * defaultValues.TokenValue;

                    db.Orders.Add(order);
                    db.SaveChanges();

                    logger.InfoFormat("CreateOrder: new order {0} ", JsonConvert.SerializeObject(new
                    {
                        order.Id,
                        order.Currency,
                        order.NumberOfTokens,
                        order.Status,
                        order.Price,
                    }));

                    return order.Id;
                }
            }
            catch (Exception e)
            {
                logger.Error("CreateOrder", e);
            }
            return null;
        }

        public bool SetOrderStatus(string orderID, OrderStatus status)
        {
            logger.InfoFormat("SetOrderStatus: {0} ", JsonConvert.SerializeObject(new
            {
                orderID,
                status
            }));

            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var order = db.Orders.Include(a => a.User).Where(o => o.Id.ToString() == orderID).SingleOrDefault();
                    if (order == null)
                    {
                        logger.ErrorFormat("SetOrderStatus: order {0} is null ", JsonConvert.SerializeObject(new
                        {
                            order
                        }));
                        return false;
                    }
                    order.Status = status;

                    if (status == OrderStatus.COMPLETED)
                    {
                        // add tokens to the user
                        order.User.NumberOfTokens += order.NumberOfTokens;
                        db.Entry(order.User).State = EntityState.Modified;
                    }

                    db.Entry(order).State = EntityState.Modified;

                    db.SaveChanges();

                    logger.InfoFormat("SetOrderStatus: updated order {0} ", JsonConvert.SerializeObject(new
                    {
                        order.Id,
                        order.NumberOfTokens,
                        order.Status,
                        order.User.UserName
                    }));

                    return true;
                }
            }
            catch (Exception e)
            {
                logger.Error("SetOrderStatus", e);
            }
            return false;
        }
        #endregion

        #region Bids 
        public bool MakeBid(string auctionId, long? offerTokens, string userId)
        {
            logger.InfoFormat("MakeBid: {0} ", JsonConvert.SerializeObject(new
            {
                auctionId,
                offerTokens,
                userId
            }));

            if (auctionId == null || offerTokens == null)
            {
                logger.ErrorFormat("MakeBid: something is null {0} ", JsonConvert.SerializeObject(new
                {
                    auctionId,
                    offerTokens,
                }));
                return false;
            }
            try
            {
                var guidId = new Guid(auctionId);
                using (AuctionDB db = new AuctionDB())
                {
                    CloseAuctions();
                    var auction = db.Auctions.Where(a => a.Id.Equals(guidId)).Include(a => a.Bids).SingleOrDefault();
                    var user = db.Users.Where(u => u.Id.Equals(userId)).SingleOrDefault();
                    var dv = db.DefaultValues.SingleOrDefault();
                    if (auction == null || user == null || dv == null || auction.Status != AuctionStatus.OPENED)
                    {
                        logger.ErrorFormat("MakeBid: something is null or auction is not open {0} ", JsonConvert.SerializeObject(new
                        {
                            auction,
                            user,
                            dv
                        }));
                        return false;
                    }
                    var maxBid = auction.Bids.Max(b => (long?)b.NumberOfTokens);
                    if (maxBid == null)
                    {
                        maxBid = (long?)(Math.Ceiling(auction.CurrentPrice / dv.TokenValue));
                    }
                    else
                    {
                        // offer should be one token more than the current max bid
                        maxBid++;
                    }

                    // offer is too low
                    if (offerTokens < maxBid)
                    {
                        return false;
                    }

                    // max offer of this user for this auction
                    var userMaxBid = auction.Bids.Where(b => b.User.Id.Equals(userId)).Max(b => (long?)b.NumberOfTokens);
                    long tokensToPay = 0;
                    if (userMaxBid == null)
                    {
                        // this is users first bid
                        tokensToPay = (long)offerTokens;
                    }
                    else
                    {
                        // this is not users first bid, should pay only the diffeerence
                        tokensToPay = (long)offerTokens - (long)userMaxBid;
                    }

                    if (user.NumberOfTokens < tokensToPay)
                    {
                        logger.ErrorFormat("MakeBid: user doesn't have enough tokens {0} ", JsonConvert.SerializeObject(new
                        {
                            tokensToPay,
                            user.UserName,
                        }));
                        // there is not enough tokens
                        return false;
                    }

                    // update auction last bidder
                    auction.User = user;
                    auction.CurrentPrice = (long)offerTokens * dv.TokenValue;
                    //db.Entry(auction).State = EntityState.Modified;

                    try
                    {
                        db.SaveChanges();
                    }
                    catch (System.Data.Entity.Infrastructure.DbUpdateException e)
                    {
                        // catch possible concurrency exception
                        logger.Error("Make Bid", e);
                        return false;
                    }

                    // update user token
                    user.NumberOfTokens -= tokensToPay;
                    db.Entry(user).State = EntityState.Modified;

                    // create and add new bid
                    Bid bid = new Bid
                    {
                        Id = Guid.NewGuid(),
                        Auction = auction,
                        NumberOfTokens = (long)offerTokens,
                        PlacingTime = DateTime.UtcNow,
                        User = user
                    };
                    db.Bids.Add(bid);

                    db.SaveChanges();

                    logger.InfoFormat("MakeBid: new bid and updated auction {0} ", JsonConvert.SerializeObject(new
                    {
                        bid.Auction.Name,
                        bid.PlacingTime,
                        bid.NumberOfTokens,
                        bid.User.UserName,
                        auctionUser = auction.User.UserName,
                        auction.CurrentPrice
                    }));

                    return true;
                }
            }
            catch (Exception e)
            {
                logger.Error("MakeBid", e);
            }
            return false;
        }
        #endregion

        #region Helper functions

        private string GetDuration(TimeSpan span)
        {
            int h = span.Days * 24 + span.Hours;
            int m = span.Minutes;
            int s = span.Seconds;
            return (h < 10 ? "0" : "") + h + ":" + (m < 10 ? "0" : "") + m + ":" + (s < 10 ? "0" : "") + s;
        }

        #endregion
    }
}
