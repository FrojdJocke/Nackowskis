using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nackowskis.Infrastructure;
using Nackowskis.Models;
using Nackowskis.ViewModels;
using Newtonsoft.Json;

namespace Nackowskis.Repository
{
    public class AuctionRepository : IAuctionRepository
    {
        //Inject HttpClient
        private HttpClientServices httpClient;
        private string apiBaseAddress = "/api/Auktion/";
        private string groupNumber = "1120";
        public AuctionRepository(HttpClientServices client) => httpClient = client;

        public List<Auction> GetAuctions()
        {
            var response = httpClient.client.GetAsync(apiBaseAddress+groupNumber).Result;

            return HttpHelpers.ResponseToModelList(response, new Auction());
            
        }

        public Auction GetAuction(int auctionId)
        {
            var response = httpClient.client.GetAsync($"{apiBaseAddress}{groupNumber}?id={auctionId}").Result;

            return HttpHelpers.ResponseToModel(response, new Auction());
        }
        
        public List<Auction> GetUserAuctions(string name)
        {
            var response = httpClient.client.GetAsync(apiBaseAddress + groupNumber).Result;

            var auctions = HttpHelpers.ResponseToModelList(response, new Auction());

            return auctions.Where(x => x.SkapadAv == name).ToList();
        }

        public bool PostAuction(Auction model)
        {
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var res = httpClient.client.PostAsync($"{apiBaseAddress}", stringContent).Result;

            return res.IsSuccessStatusCode;
        }

        public bool DeleteAuction(int id)
        {
            var response = httpClient.client.DeleteAsync($"{apiBaseAddress}{groupNumber}?id={id}").Result;

            return response.IsSuccessStatusCode;
        }

        public bool UpdateAuction(Auction model)
        {
            var json = JsonConvert.SerializeObject(model);
            var stringContent = new StringContent(json, Encoding.UTF8, "application/json");
            var response = httpClient.client.PutAsync(apiBaseAddress, stringContent).Result;

            return response.IsSuccessStatusCode;
        }
    }
}
