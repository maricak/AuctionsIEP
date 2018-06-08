using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Auction.Data.Models
{
    [Table("Bid")]
    public class Bid
    {
        [Key]
        public Guid Id { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime PlacingTime { get; set; }

        public long NumberOfTokens { get; set; }

        [Required]
       // [Column(name: "User_Id")]
        public string UserId { get; set; } // guid

        [Required]
        public Guid AuctionId { get; set; }
    }
}