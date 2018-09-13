using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Nackowskis.Infrastructure;
using Nackowskis.Models;
using Nackowskis.Repository;
using Nackowskis.ViewModels;

namespace Nackowskis.Controllers
{
    public class AuctionController : Controller
    {
        private readonly AuctionMethods _auctionMethods;

        public AuctionController(AuctionMethods auctionMethods) => _auctionMethods = auctionMethods;

        public IActionResult Index()
        {
            return View(new AuctionViewModel { Auctions = new List<Auction>(), SearchFilter = "" });
        }

        public IActionResult GetAuctions(AuctionViewModel vm)
        {
            var model = new AuctionViewModel();

            if (string.IsNullOrWhiteSpace(vm.SearchFilter))
            {
                var auctions = _auctionMethods.GetAuctions();
                model.Auctions = _auctionMethods.RemoveExpiredAuctions(auctions);
                model.SearchFilter ="";
            }
            else
            {
                model.Auctions = _auctionMethods.GetFilteredAuctions(vm.SearchFilter);
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

        //Fixa callback för info om vad som hänt
        public IActionResult NewBid(AuctionDetailsViewModel vm)
        {
            //var bidValid = _auctionMethods.BidIsValid(vm.Bids.Max(x => x.Summa), vm.NewBid.Summa);
            //if (!bidValid)
            //    TempData["BidInvalid"] = "Bid was lower that the current highest bid. Don't be cheap stupid!";
            bool success = _auctionMethods.PostNewBid(vm.NewBid);
            if (success /*&& bidValid*/)
            {
                return RedirectToAction("Index");
            }

            return RedirectToAction("Index");
        }

        public IActionResult NewAuction(Auction vm)
        {
            if (ModelState.IsValid)
            {
                if (vm.SlutDatum < DateTime.Now)
                {
                    TempData["NewAuctionErrors"] = "Deadline can't be before current date!";

                    return RedirectToAction("Auctions", "Admin");
                }
                var auctionSuccess = _auctionMethods.CreateNewAuction(vm);
                if (auctionSuccess)
                {
                    TempData["NewAuctionCreate"] = "New auction was succefully created!";

                    return RedirectToAction("Auctions", "Admin");
                }
                TempData["NewAuctionCreate"] = "There was a problem creating your auction. Please try again later";
                return RedirectToAction("Auctions", "Admin");
            }

            TempData["NewAuctionErrors"] = "Error: ";
            foreach (var state in ViewData.ModelState.Values)
            {
                foreach (var error in state.Errors)
                {
                    TempData["NewAuctionErrors"] += $"{error.ErrorMessage}, ";
                }
            }
            return RedirectToAction("Auctions", "Admin");
        }

        public IActionResult UpdateAuction(Auction vm, int auctionId)
        {
            if (ModelState.IsValid)
            {
                var auctionSuccess = _auctionMethods.UpdateAuction(vm);
                if (auctionSuccess)
                {
                    TempData["Update"] = "Auction was successfully updated!";
                    return RedirectToAction("Auctions", "Admin");
                }

            }
            TempData["Update"] = "Something went wrong. Try again later";
            return RedirectToAction("Auctions", "Admin");
        }
    }
}