using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microsoft.AspNet.SignalR;

namespace Auctions.Web
{
    public class AuctionsHub : Hub
    {
        public void Update(string auctionId, string content)
        {
            //Clients.All.update(auctionId, content);
        }
    }
}