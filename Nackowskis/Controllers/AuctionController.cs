using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nackowskis.Infrastructure;
using Nackowskis.Models;
using Nackowskis.Repository;
using Nackowskis.ViewModels;

namespace Nackowskis.Controllers
{
    public class AuctionController : Controller
    {
        private readonly IAuctionRepository _auctionRepo;
        private readonly IBidRepository _bidRepo;
        private readonly AuctionMethods _auctionMethods;

        public AuctionController(IAuctionRepository repo, IBidRepository bidRepo, AuctionMethods auctionMethods)
        {
            _auctionRepo = repo;
            _bidRepo = bidRepo;
            _auctionMethods = auctionMethods;
        }
        
        public IActionResult Index()
        {
            return View(new AuctionViewModel { Auctions = new List<Auction>(), SearchFilter = "" });
        }

        public IActionResult GetAuctions(AuctionViewModel vm)
        {
            var model = new AuctionViewModel();

            if (string.IsNullOrWhiteSpace(vm.SearchFilter))
            {
                var auctions = _auctionRepo.GetAuctions();
                model.Auctions = _auctionMethods.RemoveExpiredAuctions(auctions);
                model.SearchFilter ="";
            }
            else
            {
                model.Auctions = _auctionRepo.GetFilteredAuctions(vm.SearchFilter);
                model.SearchFilter = vm.SearchFilter;
            }

            if (!string.IsNullOrWhiteSpace(vm.SortOrder))
            {
                if (vm.SortOrder.ToLower() == "estimate")
                {
                    model.Auctions = model.Auctions.OrderBy(x => x.Utropspris).ToList();
                }
                else if (vm.SortOrder.ToLower() == "deadline")
                {
                    model.Auctions = model.Auctions.OrderBy(x => x.SlutDatum).ToList();
                }
            }
            return PartialView("_Auctions", model);
        }

        public IActionResult GetAuctionDetails(int auctionId)
        {
            var model = _auctionMethods.GetDetialsById(auctionId);
            return PartialView("_AuctionDetails", model);
        }

        public IActionResult NewBid(AuctionDetailsViewModel vm)
        {
            var success = _bidRepo.PostNewBid(vm.NewBid);
            if (success)
            {

            }
            return View();
        }
    }
}