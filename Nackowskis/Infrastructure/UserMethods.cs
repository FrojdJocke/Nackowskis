using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Nackowskis.Models;
using Nackowskis.Repository;
using Nackowskis.ViewModels;

namespace Nackowskis.Infrastructure
{
    public class UserMethods
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;

        public UserMethods(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
        }

        public async Task<bool> RegisterUser(UserModel model)
        {
            
            if (!_signInManager.UserManager.Users.Any(x => x.UserName == model.UserName))
            {
                var newUser = new ApplicationUser { UserName = model.UserName };

                var result = await _userManager.CreateAsync(newUser, model.Password);

                //If new user is successfully created
                if (result.Succeeded)
                {
                        
                    //Sign in and save in cookie indefinitely
                    await _userManager.AddToRoleAsync(newUser, "Regular");
                    await _signInManager.SignInAsync(newUser, isPersistent: true);

                    return true;
                }
            }
            return false;
        }

        public async Task<bool> SignInUser(UserModel model)
        {
            if (!string.IsNullOrWhiteSpace(model.UserName) && !string.IsNullOrWhiteSpace(model.Password))
            {
                var signIn = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, true, false);
                if (signIn.Succeeded)
                {
                    return true;
                }
            }
            return false;
        }

        public async void SignOut()
        {
            await _signInManager.SignOutAsync();
        }
        /// <summary>
        /// Returns a list of ApplicationUser with matching role
        /// </summary>
        /// <param name="role"></param>
        /// <returns></returns>
        public List<ApplicationUser> GetUsers(string role)
        {
            var users = new List<ApplicationUser>();
            foreach (var user in _userManager.Users.ToList())
            {
                if (_userManager.IsInRoleAsync(user, role).Result)
                {
                    users.Add(user);
                }
            }

            return users;
        }

        public async Task<IdentityResult> UpgradeUser(string toRole, string userName)
        {
            var model = _userManager.FindByNameAsync(userName).Result;

            //var roles = _userManager.GetRolesAsync(model).Result.ToList();

            var remove = await _userManager.RemoveFromRoleAsync(model, "Regular");
            if (remove.Succeeded)
            {
                return _userManager.AddToRoleAsync(model, toRole).Result;
            }

            return remove;
        }
    }
}
