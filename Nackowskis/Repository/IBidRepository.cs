using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nackowskis.Models;

namespace Nackowskis.Repository
{
    public interface IBidRepository
    {
        List<Bid> GetBidsForAuction(int auctionId);

        bool PostNewBid(Bid newBid);
    }
}
