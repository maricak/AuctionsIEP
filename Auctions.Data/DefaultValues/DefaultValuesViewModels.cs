using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auctions.Data.Models
{
    public class EditDefaultValuesViewModel
    {
        [Required]
        [Display(Name = "Number of auctions per page")]
        [Range(1, Int64.MaxValue, ErrorMessage = "The value must be positive")]
        public long NumberOfAuctionsPerPage { get; set; }

        [Required]
        [Display(Name = "Default auction duration")]
        [Range(1, Int64.MaxValue, ErrorMessage = "The value must be positive")]
        public long AuctionDuration { get; set; }

        [Required]
        [Display(Name = "Number of tokens for Silver package")]
        [Range(1, Int64.MaxValue, ErrorMessage = "The value must be positive")]
        public long SilverTokenNumber { get; set; }

        [Required]
        [Display(Name = "Number of tokens for Gold package")]
        [Range(1, Int64.MaxValue, ErrorMessage = "The value must be positive")]
        public long GoldTokenNumber { get; set; }

        [Required]
        [Display(Name = "Number of tokens for Platinum package")]
        [Range(1, Int64.MaxValue, ErrorMessage = "The value must be positive")]
        public long PlatinuTokenNumber { get; set; }

        [Required]
        [Display(Name = "Value of a token")]
        [Range(0.01, Double.MaxValue, ErrorMessage = "The value must be positive")]
        public decimal TokenValue { get; set; }

        [Required]
        [EnumDataType(typeof(CurrencyType))]
        public CurrencyType Currency { get; set; }
    }

    public class DetailsDefaultValuesViewModel
    {
        [Display(Name = "Number of auctions per page")]
        public long NumberOfAuctionsPerPage { get; set; }

        [Display(Name = "Default auction duration")]
        public long AuctionDuration { get; set; }

        [Display(Name = "Number of tokens for Silver package")]
        public long SilverTokenNumber { get; set; }

        [Display(Name = "Number of tokens for Gold package")]
        public long GoldTokenNumber { get; set; }

        [Display(Name = "Number of tokens for Platinum package")]
        public long PlatinuTokenNumber { get; set; }

        [Display(Name = "Value of a token")]
        public decimal TokenValue { get; set; }

        [Display(Name = "Currency")]
        //[EnumDataType(typeof(CurrencyType))]
        public CurrencyType Currency { get; set; }
    }
}
