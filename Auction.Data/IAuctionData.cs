using Auction.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Auction.Data
{
    public interface IAuctionData
    {
        /* DefaultValues */
        DetailsDefaultValuesViewModel GetDetailsDefaultValues();
        EditDefaultValuesViewModel GetEditDefaultValues();
        bool SetDefaultValues(EditDefaultValuesViewModel model);
        /* DefaultValues END */

        /* Auctions */
        ICollection<AdminAuctionViewModel> GetReadyAuctions();
        bool OpenAuction(string id);
        /* Auctions END */

        /* Orders */
        ICollection<IndexOrderViewModel> GetOrdersByUserId(string id);
        /* Orders END */
    }
}
