using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace Auction.Data.Models
{
    [Table("Order")]
    public class Order
    {
        [Key]
        public Guid Id { get; set; }

        public long NumberOfTokens { get; set; }

        public decimal Price { get; set; }

        public CurrencyType Currency { get; set; }

        public OrderStatus Status { get; set; }

        [Required]
        //public string UserId { get; set; } // guid
        public virtual User User { get; set; }
    }
}