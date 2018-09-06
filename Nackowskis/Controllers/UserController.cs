using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Nackowskis.Infrastructure;
using Nackowskis.ViewModels;

namespace Nackowskis.Controllers
{
    public class UserController : Controller
    {
        private readonly UserMethods _userMethods;

        public UserController(UserMethods userMethods)
        {
            _userMethods = userMethods;
        }
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        /////////////////////////////////////////////////////////////////////////////////////////////////////////
        public IActionResult Register()
        {
            var model = new UserModel();
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult RegisterUser(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var registered = _userMethods.RegisterUser(user).Result;
                if (registered == false)
                {
                    user.Unique = false;
                    return View("Register",user);
                }
                return RedirectToAction("Index", "Home");
            }

            return View("Register", user);
        }

        public IActionResult SignIn()
        {
            var model = new UserModel();
            return PartialView("_Login",model);
        }

        public IActionResult Authorize(UserModel user)
        {
            if (ModelState.IsValid)
            {
                var authenticated = _userMethods.SignInUser(user).Result;
                if (authenticated)
                {
                    return RedirectToAction("Index", "Home");
                }
            }
            return View("LoginFailed",user);
        }

        public IActionResult SignOut()
        {
            _userMethods.SignOut();
            return RedirectToAction("Index", "Home");
        }
    }
}