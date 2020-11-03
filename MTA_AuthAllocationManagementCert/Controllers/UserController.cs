using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MTA_AuthAllocationManagementCert.Models.CustomIdentityEFCore;
using MTA_AuthAllocationManagementCert.Models.DBAuthContext;

namespace MTA_AuthAllocationManagementCert.Controllers
{
    public class UserController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signInManager { get; }
        private readonly DbContextAuth _dbContextAuth;

        public UserController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DbContextAuth dbContextAuth)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContextAuth = dbContextAuth;
        }
        public async Task<IActionResult> AddUser()
        {
            try
            {

                ViewBag.Message = "User already";
                AppUser appUser = await _userManager.FindByNameAsync("fuckyou");
                var test = _userManager.Users.Where(x => x.UserName == "dandan").FirstOrDefault();
                if (appUser == null)
                {
                    appUser = new AppUser();
                    appUser.Email = "dan@gmail.com";
                    appUser.FirstName = "Dần";
                    appUser.UserName = "dandan";
                    appUser.LastName = "Đẹp trai";

                    IdentityResult result = await _userManager.CreateAsync(appUser, "12345689@Abc");
                    ViewBag.Message = "User was created";
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();
            }
            return View();
        }
    }
}