﻿using Auctions.Data.Models;
using PagedList;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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

        //bool CreateAuction(CreateAuctionViewModel model, string userId);
        bool CreateAuction(CreateAuctionViewModel model);

        DetailsAuctionViewModel GetAuctionById(string id);

        ICollection<AuctionViewModel> GetAuctionsByWinner(string userId);
        /* Auctions END */

        /* Orders */
        ICollection<IndexOrderViewModel> GetOrdersByUserId(string id);
        Guid? CreateOrder(OrderPackage package, string userId);
        bool SetOrderStatus(string orderID, OrderStatus status);
        /* Orders END */
    }
}
