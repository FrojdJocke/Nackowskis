using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Nackowskis.ViewModels
{
    public class DashboardModel
    {
        public List<string> Dates { get; set; }
        public string Date { get; set; }
        public bool MyAuctionsOnly { get; set; }
        public string Username { get; set; }
    }
}
