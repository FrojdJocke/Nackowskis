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
    public class AdminController : Controller
    {
        #region //Dependency Injections
        private readonly AuctionMethods _auctionMethods;

        public AdminController(AuctionMethods methods) => _auctionMethods = methods;

        #endregion

        public IActionResult Auctions()
        {
            var model = _auctionMethods.GetAdminAuctions(User.Identity.Name);

            return View(model);
        }

        public IActionResult GetNewAuctionForm(int? id, bool update = false)
        {
            var model = new Auction();
            if (update)
            {
                ViewData["Update"] = true;
                model = _auctionMethods.GetAuctions().FirstOrDefault(x => x.AuktionID == id);
            }
            else
            {
                ViewData["Update"] = false;
            }
            return PartialView("_NewAuction",model);
        }

        //public IActionResult GetUserAuctions(string name)
        //{
        //    return PartialView("_AdminAuctions");
        //}
        public IActionResult DeleteAuction(int auctionId)
        {
            var deleted = _auctionMethods.DeleteAuction(auctionId);

            return RedirectToAction("Auctions");
        }
    }
}