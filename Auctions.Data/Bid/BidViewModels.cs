using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auctions.Data.Models

{
    public class DetailsBidViewModel
    {
        [Display(Name = "Time")]
        [DataType(DataType.DateTime)]
        public DateTime PlacingTime { get; set; }

        [Display(Name = "Number of tokens")]
        public long NumberOfTokens { get; set; }

        [Display(Name = "Bidder")]
        [Required]
        public string User { get; set; }
    }
}
