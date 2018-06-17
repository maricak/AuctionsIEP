using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auctions.Data.Models
{

    public class IndexOrderViewModel
    {
        [Display(Name = "Number of tokens")]
        public long NumberOfTokens { get; set; }

        [Display(Name = "Price")]
        public decimal Price { get; set; }

        [Display(Name = "Currency")]
        public CurrencyType Currency { get; set; }

        [Display(Name = "Status")]
        public OrderStatus Status { get; set; }
    }

    public class CreateOrderViewModel
    {
        [Required(ErrorMessage = "Select package")]
        [Display(Name = "Package", GroupName = "Package")]
        public string Package { get; set; }
    }
}
