using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nackowskis.ViewModels
{
    public class DashboardStatsModel
    {
        public string AuctionName { get; set; }
        public int EstimatePrice { get; set; }
        public int WinningBid { get; set; }
        public int Difference { get; set; }
    }
}
