using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Nackowskis.Models;
using Nackowskis.Repository;
using Nackowskis.ViewModels;
using Newtonsoft.Json;

namespace Nackowskis.Infrastructure
{
    public sealed class AuctionMethods
    {
        private IAuctionRepository _auctionRepo;
        private IBidRepository _bidRepo;

        public AuctionMethods(IAuctionRepository auctionRepo, IBidRepository bidRepo)
        {
            _auctionRepo = auctionRepo;
            _bidRepo = bidRepo;
        }

        public List<Auction> GetAuctions()
        {
            return _auctionRepo.GetAuctions();
        }

        public AuctionDetailsViewModel GetDetialsById(int auctionId)
        {
            var model = new AuctionDetailsViewModel
            {
                Auction = _auctionRepo.GetAuction(auctionId),
                Bids = _bidRepo.GetBidsForAuction(auctionId)
            };
            return model;
        }
        /// <summary>
        /// Returns all auctions for current user with bid history
        /// </summary>
        /// <param name="userName"></param>
        /// <returns></returns>
        public async Task<List<AdminAuctionViewModel>> GetAdminAuctions(string userName)
        {
            var auctions = await _auctionRepo.GetUserAuctions(userName);
            var model = new List<AdminAuctionViewModel>();
            foreach (var a in auctions)
            {
                var entity = new AdminAuctionViewModel
                {
                    Auction = _auctionRepo.GetAuction(a.AuktionID),
                    Bids = _bidRepo.GetBidsForAuction(a.AuktionID)
                };
                model.Add(entity);
            }

            return model;
        }

        public List<Auction> GetFilteredAuctions(string filter)
        {
            var auctions = _auctionRepo.GetAuctions();

            return auctions.Where(x => x.Beskrivning.ToLower().Contains(filter.ToLower()) || x.Titel.ToLower().Contains(filter.ToLower())).ToList();
        }

        public List<Auction> RemoveExpiredAuctions(List<Auction> list)
        {
            return list.Where(x => x.SlutDatum > DateTime.Now).ToList();
        }

        public bool BidIsValid(int leadingBid, int newBid)
        {
            if (newBid < 1) return false;
            return newBid > leadingBid;
        }

        public bool CreateNewAuction(Auction model)
        {
            #region //Model Data Setup

            model.Gruppkod = 1120;
            model.StartDatum = DateTime.Now;
            model.SlutDatum = model.SlutDatum.Date + new TimeSpan(23, 59, 59);
            #endregion
            
            return _auctionRepo.PostAuction(model);
        }

        public bool PostNewBid(Bid model)
        {
            return _bidRepo.PostBid(model);
        }

        public bool DeleteAuction(int auctionId)
        {
            return _auctionRepo.DeleteAuction(auctionId);
        }

        public bool UpdateAuction(Auction vm)
        {
            return _auctionRepo.UpdateAuction(vm);
        }

        public List<DashboardStatsModel> GetAuctionStatistics(DashboardModel vm)
        {
            var list = new List<DashboardStatsModel>();
            var auctions = _auctionRepo.GetAuctions();
            var sortAuction = auctions.Where(x => 
                x.SlutDatum < DateTime.Today && (x.StartDatum.ToString("Y") == vm.Date || x.SlutDatum.ToString("Y") == vm.Date)).ToList();

            if (vm.MyAuctionsOnly)
            {
                sortAuction = sortAuction.Where(x => x.SkapadAv == vm.Username).ToList();
            }

            foreach (var a in sortAuction)
            {
                var bids = _bidRepo.GetBidsForAuction(a.AuktionID);
                var highbid = 0;
                if (bids.Any())
                {
                    highbid = bids.Max(x => x.Summa);
                }
                var model = new DashboardStatsModel
                {
                    AuctionName = a.Titel,
                    EstimatePrice = a.Utropspris,
                    WinningBid = highbid
                };
                model.Difference = Calculate.Difference(model.EstimatePrice, model.WinningBid);

                list.Add(model);
            }

            return list;
        }
    }
}
