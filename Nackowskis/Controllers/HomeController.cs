using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Runtime.Serialization.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nackowskis.Models;
using Nackowskis.Repository;
using Nackowskis.ViewModels;

namespace Nackowskis.Controllers
{
    //Gruppkod 1120
    public class HomeController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public HomeController( SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public IActionResult Index()
        {
            if (_signInManager.IsSignedIn(User))
            {
                return RedirectToAction("Index", "Auction");
            }

            return View("Start");
        }

        

    }
}