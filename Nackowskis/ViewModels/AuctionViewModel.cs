using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nackowskis.Models;

namespace Nackowskis.ViewModels
{
    public class AuctionViewModel
    {
        public List<Auction> Auctions { get; set; }
        public string SearchFilter { get; set; }
        public string SortOrder { get; set; }
    }
}
