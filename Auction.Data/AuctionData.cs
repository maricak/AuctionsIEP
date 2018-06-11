using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Auction.Data.Models;

namespace Auction.Data
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
                            Status = auction.Status,
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
        /* Auctions END */



    }
}
