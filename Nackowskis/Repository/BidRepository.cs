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
        public BidRepository(HttpClientServices client) => httpClient = client;

        public List<Bid> GetBidsForAuction(int auctionId)
        {
            var response = httpClient.client.GetAsync($"{apiBaseAddress}?id={auctionId}").Result;

            return HttpHelpers.ResponseToModelList(response, new Bid());
        }

        public bool PostNewBid(Bid newBid)
        {
            //var myContent = JsonConvert.SerializeObject(newBid);
            //var buffer = System.Text.Encoding.UTF8.GetBytes(myContent);
            //var byteContent = new ByteArrayContent(buffer);
            //byteContent.Headers.ContentType = new MediaTypeHeaderValue("application/json");
            var json = JsonConvert.SerializeObject(newBid);
            var stringContent = new StringContent(json, UnicodeEncoding.UTF8, "application/json");


            var res = httpClient.client.PostAsync($"{apiBaseAddress}?id={newBid.AuktionID}", stringContent).Result;

            return res.IsSuccessStatusCode;
        }

        
    }
}
