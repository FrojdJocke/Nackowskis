using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Nackowskis.Models;
using Nackowskis.Repository;
using Nackowskis.ViewModels;

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

        public AuctionDetailsViewModel GetDetialsById(int auctionId)
        {
            var model = new AuctionDetailsViewModel
            {
                Auction = _auctionRepo.GetAuction(auctionId),
                Bids = _bidRepo.GetBidsForAuction(auctionId)
            };
            return model;
        }

        public List<Auction> RemoveExpiredAuctions(List<Auction> list)
        {
            return list.Where(x => x.SlutDatum > DateTime.Now).ToList();
        }
    }
}
