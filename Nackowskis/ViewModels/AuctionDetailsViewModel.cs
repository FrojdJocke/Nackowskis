using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nackowskis.Models;

namespace Nackowskis.ViewModels
{
    public class AuctionDetailsViewModel
    {
        public Auction Auction { get; set; }
        public List<Bid> Bids { get; set; }
        public Bid NewBid { get; set; }
        //public int AmountBid { get; set; }
    }
}
