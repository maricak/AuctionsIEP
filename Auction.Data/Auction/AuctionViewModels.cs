using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Data.Models
{
    public class AdminAuctionViewModel
    {
        public string Name { get; set; }

        public long Duration { get; set; }

        [Display(Name = "Start price")]
        public decimal StartPrice { get; set; }

        public CurrencyType Currency { get; set; }

        [Display(Name = "Time of creation")]
        [DataType(DataType.DateTime)]
        public DateTime CreatingTime { get; set; }

        public AuctionStatus Status { get; set; }

        public string Username { get; set; }
    }
}
