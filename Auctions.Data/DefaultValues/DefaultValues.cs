using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Auctions.Data.Models
{
    [Table("DefaultValues")]
    public class DefaultValues
    {
        [Key]
        public Guid Id { get; set; }
        public long NumberOfAuctionsPerPage { get; set; }
        public long AuctionDuration { get; set; }
        public long SilverTokenNumber { get; set; }
        public long GoldTokenNumber { get; set; }
        public long PlatinuTokenNumber { get; set; }
        public decimal TokenValue { get; set; }
        public CurrencyType Currency { get; set; }

        //public DefaultValues()
        //{
        //    NumberOfAuctionsPerPage = 20;
        //    AuctionDuration = 86400;
        //    SilverTokenNumber = 30;
        //    GoldTokenNumber = 50;
        //    PlatinuTokenNumber = 100;
        //    TokenValue = 1;
        //}
    }
}