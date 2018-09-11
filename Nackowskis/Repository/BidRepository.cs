using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;
using Nackowskis.Infrastructure;
using Nackowskis.Models;
using Newtonsoft.Json;

namespace Nackowskis.Repository
{
    public class BidRepository : IBidRepository
    {
        private HttpClientServices httpClient;
        private string apiBaseAddress = "/api/Bud/100";
        private string groupNumber = "1120";
        public BidRepository(HttpClientServices client) => httpClient = client;

        public List<Bid> GetBidsForAuction(int auctionId)
        {
            var response = httpClient.client.GetAsync($"{apiBaseAddress}{groupNumber}?id={auctionId}").Result;

            return HttpHelpers.ResponseToModelList(response, new Bid());
        }

        public bool PostBid(Bid newBid)
        {
            var json = JsonConvert.SerializeObject(newBid);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");

            return httpClient.client.PostAsync($"{apiBaseAddress}?id={newBid.AuktionID}", stringContent).Result.IsSuccessStatusCode;
        }

        
    }
}
