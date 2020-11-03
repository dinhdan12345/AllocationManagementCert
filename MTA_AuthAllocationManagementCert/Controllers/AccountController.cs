using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MTA_AuthAllocationManagementCert.Models;
using MTA_AuthAllocationManagementCert.Models.CustomIdentityEFCore;
using MTA_AuthAllocationManagementCert.Models.DBAuthContext;
using MTA_CommonAllocationManagementCert;

namespace MTA_AuthAllocationManagementCert.Controllers
{
    public class AccountController : Controller
    {
        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signInManager { get; }
        private readonly DbContextAuth _dbContextAuth;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DbContextAuth dbContextAuth)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContextAuth = dbContextAuth;
        }
        [HttpPost]
        public async Task<FormBaseRes<LoginRes>> Login(LoginReq loginReq)
        {
            var objRes = new FormBaseRes<LoginRes>();
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginReq.UserName, loginReq.PassWord, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(loginReq.UserName);
                    var roles = await _userManager.GetRolesAsync(user);
                    if (roles != null)
                    {
                        var role = string.Join(",", roles);
                        if (result.Succeeded)
                        {
                            ViewBag.Result = "Đăng nhập thành công";
                        }
                        else
                        {
                            ViewBag.Result = "Đăng nhập không thành công";
                        }
                    }

                }

            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();
            }
            return objRes;

        }
        [HttpGet]
        public async Task<IActionResult> Logout()
        {
            try
            {
                await _signInManager.SignOutAsync();
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                ViewBag.Message = ex.ToString();
            }
            return View();
        }
    }
}
