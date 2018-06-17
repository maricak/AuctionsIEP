using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auctions.Data.Models
{
    public class AdminIndexViewModel
    {
        public ICollection<AdminAuctionViewModel> Auctions { get; set; }
        public DetailsDefaultValuesViewModel DefaultValues { get; set; }
    }
}
