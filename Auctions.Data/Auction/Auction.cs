using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Auctions.Data.Models
{
    [Table("Auction")]
    public class Auction
    {
        [Key]
        public Guid Id { get; set; }

        public string Name { get; set; }

        public byte[] Image { get; set; }

        public long Duration { get; set; }

        public decimal StartPrice { get; set; }

        public decimal CurrentPrice { get; set; }

        public CurrencyType Currency { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime CreatingTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? OpeningTime { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? ClosingTime { get; set; }

        public AuctionStatus Status { get; set; }

        //       public string UserId { get; set; } // guid
        public virtual User User { get; set; }
        public ICollection<Bid> Bids { get; set; }

    }
}