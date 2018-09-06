using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nackowskis.Models;
using Nackowskis.ViewModels;

namespace Nackowskis.Repository
{
    public interface IAuctionRepository
    {
        List<Auction> GetAuctions();

        Auction GetAuction(int auctionId);

        List<Auction> GetFilteredAuctions(string filter);
        
    }
}
