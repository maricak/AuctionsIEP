using Auctions.Data.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using X.PagedList;

namespace Auctions.Data
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
        IPagedList<AuctionViewModel> GetAllOpenedAuctions(string searchString, decimal? lowPrice, decimal? highPrice, AuctionStatus? status, int? page);
        bool CreateAuction(CreateAuctionViewModel model);
        AuctionViewModel GetAuctionById(string id);
        DetailsAuctionViewModel GetAuctionDetailsById(string id, int? page);
        IPagedList<AuctionViewModel> GetAuctionsByWinner(string userId, int? page);
        /* Auctions END */

        /* Orders */
        IPagedList<IndexOrderViewModel> GetOrdersByUserId(string id, int?page);
        Guid? CreateOrder(OrderPackage package, string userId);
        bool SetOrderStatus(string orderID, OrderStatus status);

        /* Orders END */

        /* Bids */
        bool MakeBid(string id, long? offer, string userId);
        /* Bids END */
    }
}
