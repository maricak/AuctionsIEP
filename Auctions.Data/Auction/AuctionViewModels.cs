using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Web;

namespace Auctions.Data.Models
{
    public class AdminAuctionViewModel
    {
        [Display(Name = "Name")]
        public string Name { get; set; }

        public long Duration { get; set; }

        [Display(Name = "Start price")]
        public decimal StartPrice { get; set; }

        public CurrencyType Currency { get; set; }

        [Display(Name = "Time of creation")]
        [DataType(DataType.DateTime)]
        public DateTime CreatingTime { get; set; }

        public AuctionStatus Status { get; set; }

        public string Id { get; set; }
    }

    public class CreateAuctionViewModel
    {
        [Required(ErrorMessage = "The auction name field is required.")]
        [StringLength(50, ErrorMessage = "Name must be less than {1} characters.")]
        public string Name { get; set; }

        [Required(ErrorMessage = "The auction image file is required.")]
        [Display(Name = "Product Image")]
        [DataType(DataType.Upload)]
        public HttpPostedFileBase Image { get; set; }
        
        [Required(ErrorMessage = "The duration field is required.")]
        [Display(Name = "Auction duration (in seconds)")]
        [Range(1, Int64.MaxValue, ErrorMessage = "Duration must be positive")]
        public long Duration { get; set; }

        [Required(ErrorMessage = "The starting price field is required.")]
        [Display(Name = "Starting Price")]
        public decimal StartPrice { get; set; }

        [Display(Name = "Currency")]
        public CurrencyType Currency { get; set; }
    }

    public class AuctionViewModel
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public byte[] Image { get; set; }

        public long Duration { get; set; }

        public decimal CurrentPrice { get; set; }

        public CurrencyType Currency { get; set; }

        public AuctionStatus Status { get; set; }

        public string LastBidder { get; set; }
    }

    public class DetailsAuctionViewModel
    {
        public string Name { get; set; }

        public byte[] Image { get; set; }

        public long Duration { get; set; }

        public decimal CurrentPrice { get; set; }

        public CurrencyType Currency { get; set; }

        public AuctionStatus Status { get; set; }

        public string LastBidder { get; set; }

        public ICollection<DetailsBidViewModel> Bids { get; set; }
    }
}
