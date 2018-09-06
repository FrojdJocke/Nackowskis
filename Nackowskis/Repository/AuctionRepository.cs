using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using Nackowskis.Infrastructure;
using Nackowskis.Models;
using Nackowskis.ViewModels;

namespace Nackowskis.Repository
{
    public class AuctionRepository : IAuctionRepository
    {
        //Inject HttpClient
        private HttpClientServices httpClient;
        private string apiBaseAddress = "/api/Auktion/100";
        public AuctionRepository(HttpClientServices client) => httpClient = client;

        public List<Auction> GetAuctions()
        {
            var response = httpClient.client.GetAsync(apiBaseAddress).Result;

            return HttpHelpers.ResponseToModelList(response, new Auction());
            
        }

        public Auction GetAuction(int auctionId)
        {
            var response = httpClient.client.GetAsync($"{apiBaseAddress}?id={auctionId}").Result;

            return HttpHelpers.ResponseToModel(response, new Auction());
        }

        public List<Auction> GetFilteredAuctions(string filter)
        {
            var response = httpClient.client.GetAsync(apiBaseAddress).Result;

            var auctions = HttpHelpers.ResponseToModelList(response, new Auction());

            return auctions.Where(x => x.Beskrivning.ToLower().Contains(filter.ToLower()) || x.Titel.ToLower().Contains(filter.ToLower())).ToList();
        }

        
    }
}
