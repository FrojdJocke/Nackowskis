using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Nackowskis.Infrastructure;
using Nackowskis.Models;
using Nackowskis.ViewModels;

namespace Nackowskis.Controllers
{
    public class UserController : Controller
    {
        private readonly UserMethods _userMethods;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly UserManager<ApplicationUser> _userManager;

        public UserController(UserMethods userMethods,SignInManager<ApplicationUser> signInManager,UserManager<ApplicationUser> userManager)
        {
            _userMethods = userMethods;
            _signInManager = signInManager;
            _userManager = userManager;
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
                    return View("Register", user);
                }

                return RedirectToAction("Index", "Home");
            }

            return View("Register", user);
        }

        public IActionResult SignIn()
        {
            var model = new UserModel();
            return PartialView("_Login", model);
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

            return View("LoginFailed", user);
        }

        public IActionResult SignOut()
        {
            _userMethods.SignOut();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult GoogleLogin(string returnUrl = "https://localhost:44357")
        {
            //string redirectUrl = Url.Action("GoogleResponse", "User", new { ReturnUrl = returnUrl });
            string redirectUrl = "https://localhost:44357/User/GoogleResponse/";
            var properties = _signInManager
                .ConfigureExternalAuthenticationProperties("Google", redirectUrl);
            return new ChallengeResult("Google", properties);
        }

        [AllowAnonymous]
        public async Task<IActionResult> GoogleResponse(string returnUrl = "/")
        {
            ExternalLoginInfo info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                return RedirectToAction("Index", "Home");
            }

            var result = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, false);
            if (result.Succeeded)
            {
                return Redirect(returnUrl);
            }
            else
            {

                var user = new ApplicationUser()
                {
                    Email = info.Principal.FindFirst(ClaimTypes.Email).Value,
                    NormalizedUserName = info.Principal.FindFirst(ClaimTypes.Name).Value != null ? info.Principal.FindFirst(ClaimTypes.Name).Value : info.Principal.FindFirst(ClaimTypes.Email).Value,
                    UserName = info.Principal.FindFirst(ClaimTypes.Email).Value
                };
                IdentityResult identResult = await _userManager.CreateAsync(user);
                if (identResult.Succeeded)
                {
                    identResult = await _userManager.AddLoginAsync(user, info);
                    if (identResult.Succeeded)
                    {
                        await _userManager.AddToRoleAsync(user, "Regular");
                        await _signInManager.SignInAsync(user, false);
                        
                        return Redirect(returnUrl);
                    }
                }

                return View("LoginFailed");
            }
        }
    }
}