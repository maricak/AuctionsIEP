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

namespace Auctions.Data
{
    public class AuctionData : IAuctionData
    {
        private static AuctionData Singleton = null;

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

        /* DefaultValues */
        public DetailsDefaultValuesViewModel GetDetailsDefaultValues()
        {
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var defaultValues = db.DefaultValues.SingleOrDefault();
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
            }
            catch
            {
                // TODO : log exception
            }
            // something went wrong
            return null;
        }

        public EditDefaultValuesViewModel GetEditDefaultValues()
        {
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var defaultValues = db.DefaultValues.SingleOrDefault();
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
            }
            catch
            {
                // TODO : log exception
            }
            // something went wrong
            return null;
        }

        public bool SetDefaultValues(EditDefaultValuesViewModel model)
        {
            if (model == null)
            {
                return false;
            }
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    DefaultValues defaultValues = db.DefaultValues.SingleOrDefault();
                    defaultValues.AuctionDuration = model.AuctionDuration;
                    defaultValues.Currency = model.Currency;
                    defaultValues.NumberOfAuctionsPerPage = model.NumberOfAuctionsPerPage;
                    defaultValues.SilverTokenNumber = model.SilverTokenNumber;
                    defaultValues.GoldTokenNumber = model.GoldTokenNumber;
                    defaultValues.PlatinuTokenNumber = model.PlatinuTokenNumber;
                    defaultValues.TokenValue = model.TokenValue;

                    db.Entry(defaultValues).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception e)
            {
                // TODO: log exception
            }

            //something went wrong
            return false;
        }
        /* DefaultValues END */

        /* Auctions */
        public ICollection<AdminAuctionViewModel> GetReadyAuctions()
        {
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
                            CreatingTime = auction.CreatingTime,
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
                // TODO: log exception
            }

            // something went wrong
            return null;
        }

        public bool OpenAuction(string id)
        {
            try
            {
                if (String.IsNullOrEmpty(id))
                {
                    // TODO: log
                    return false;
                }
                var guidId = new Guid(id);
                using (AuctionDB db = new AuctionDB())
                {
                    var auction = db.Auctions.Where(a => a.Id.Equals(guidId)).SingleOrDefault();
                    if (auction == null)
                    {
                        return false;
                    }
                    auction.OpeningTime = DateTime.UtcNow;
                    auction.ClosingTime = DateTime.UtcNow.Add(TimeSpan.FromSeconds(auction.Duration));
                    auction.Status = AuctionStatus.OPENED;

                    db.Entry(auction).State = EntityState.Modified;
                    db.SaveChanges();

                    return true;
                }
            }
            catch (Exception)
            {
                // TODO: log exception
            }
            // something went wrong
            return false;
        }

        //class FiltarComparer : IEqualityComparer<string>
        //{
        //    public bool Equals(string x, string y)
        //    {
        //        return y.Contains(x);
        //    }

        //    public int GetHashCode(string obj)
        //    {
        //        return obj.GetHashCode();
        //    }
        //}

        public IPagedList<AuctionViewModel> GetAllOpenedAuctions(string searchString, decimal? lowPrice, decimal? highPrice, AuctionStatus? status, int? page)
        {
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var auctions = db.Auctions.Include(a => a.User).Where(a => a.Status != AuctionStatus.READY).ToList();
                    
                    if (!String.IsNullOrEmpty(searchString))
                    {
                        page = 1;
                        searchString.Trim();
                        List<Auction> auctionsFiltered = new List<Auction>();
                        var words = searchString.Split(' ');

                        foreach (var word in words)
                        {
                            if (String.IsNullOrEmpty(word))
                            {
                                continue;
                            }
                            var list = auctions.Where(a => a.Name.Contains(word)).ToList();
                            auctionsFiltered = auctionsFiltered.Union(list).ToList();
                        }
                        auctions = auctionsFiltered;
                    }

                    if (lowPrice != null)
                    {
                        page = 1;
                        auctions = auctions.Where(a => a.CurrentPrice >= lowPrice).ToList();
                    }

                    if (highPrice != null)
                    {
                        page = 1;
                        auctions = auctions.Where(a => a.CurrentPrice <= highPrice).ToList();
                    }

                    if(status != null && status != AuctionStatus.READY)
                    {
                        page = 1;
                        if(status == AuctionStatus.OPENED || status == AuctionStatus.COMPLETED)
                        {
                            auctions = auctions.Where(a => a.Status == status).ToList();
                        }
                    }

                    auctions = auctions.OrderByDescending(a => a.OpeningTime).ToList();
                    var result = new List<AuctionViewModel>();
                    foreach (var auction in (auctions))
                    {
                        result.Add(new AuctionViewModel
                        {
                            Id = auction.Id.ToString(),
                            Currency = auction.Currency,
                            Duration = auction.Duration,
                            Name = auction.Name,
                            Status = auction.Status,
                            CurrentPrice = auction.CurrentPrice,
                            Image = auction.Image,
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
                // TODO log exception
            }
            return null;
        }

        //public bool CreateAuction(CreateAuctionViewModel model, string userId)
        public bool CreateAuction(CreateAuctionViewModel model)
        {
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

                    return true;
                }
            }
            catch (Exception e)
            {
                // TODO log exception
            }
            // something went wrong
            return false;
        }

        public DetailsAuctionViewModel GetAuctionById(string id)
        {
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    Auction auction = db.Auctions.Include(a => a.Bids).Where(a => a.Id.ToString().Equals(id)).SingleOrDefault();
                    if (auction == null)
                    {
                        return null;
                    }
                    else
                    {
                        var result = new DetailsAuctionViewModel
                        {
                            Name = auction.Name, 
                            Currency = auction.Currency, 
                            CurrentPrice = auction.CurrentPrice, 
                            Duration = auction.Duration, 
                            Image = auction.Image, 
                            LastBidder = auction.User != null ? (auction.User.Name + " " + auction.User.Surname) : "", 
                            Status = auction.Status
                        };

                        var bids = new List<DetailsBidViewModel>();
                        foreach (var bid in auction.Bids)
                        {
                            bids.Add(new DetailsBidViewModel
                            {
                                NumberOfTokens = bid.NumberOfTokens, 
                                PlacingTime = bid.PlacingTime, 
                                User = bid.User.Name + " " + bid.User.Surname
                            });
                        }
                        result.Bids = bids;
                        return result;
                    }
                }
            }
            catch (Exception e)
            {
                //TODO: log exception
            }
            return null;
        }

        public IPagedList<AuctionViewModel> GetAuctionsByWinner(string userId, int? page)
        {
            try
            {
                if (String.IsNullOrEmpty(userId))
                {
                    return null;
                }

                using (AuctionDB db = new AuctionDB())
                {
                    var auctions = db.Auctions.Where(a => a.User.Id.Equals(userId)).
                        Where(a => a.Status == AuctionStatus.COMPLETED).ToList();
                    var result = new List<AuctionViewModel>();
                    foreach(var auction in auctions)
                    {
                        result.Add(new AuctionViewModel
                        {
                            Id = auction.Id.ToString(),
                            Currency = auction.Currency,
                            Duration = auction.Duration,
                            Name = auction.Name,
                            Status = auction.Status,
                            CurrentPrice = auction.CurrentPrice,
                            Image = auction.Image,
                            LastBidder = auction.User != null ? (auction.User.Name + " " + auction.User.Surname) : ""
                        });
                    }
                    if(page == null)
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
                // TODO: log exception
            }
            return null;
        }
        /* Auctions END */

        /* Orders */
        public IPagedList<IndexOrderViewModel> GetOrdersByUserId(string id, int? page)
        {
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
                // TODO: log exception

            }

            //something went wrong
            return null;
        }

        public Guid? CreateOrder(OrderPackage package, string orderUserId)
        {
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var defaultValues = db.DefaultValues.SingleOrDefault();
                    var orderUser = db.Users.Where(u => u.Id.Equals(orderUserId)).SingleOrDefault();
                    if (orderUser == null || defaultValues == null)
                    {
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

                    return order.Id;
                }
            }
            catch (Exception e)
            {
                // TODO: log exception
            }
            return null;
        }

        public bool SetOrderStatus(string orderID, OrderStatus status)
        {
            try
            {
                using (AuctionDB db = new AuctionDB())
                {
                    var order = db.Orders.Include(a => a.User).Where(o => o.Id.ToString() == orderID).SingleOrDefault();
                    if (order == null)
                    {
                        return false;
                    }
                    order.Status = status;

                    if(status == OrderStatus.COMPLETED)
                    {
                        // add tokens to the user
                        order.User.NumberOfTokens += order.NumberOfTokens;
                        db.Entry(order.User).State = EntityState.Modified;
                    }

                    db.Entry(order).State = EntityState.Modified;


                    db.SaveChanges();
                    return true;
                }
            }
            catch (Exception e)
            {
                // TODO: log exception
            }
            return false;
        }

        /* Orders END*/

    }
}
