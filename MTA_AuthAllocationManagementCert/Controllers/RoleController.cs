using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using MTA_AuthAllocationManagementCert.Models;
using MTA_AuthAllocationManagementCert.Models.CustomIdentityEFCore;
using MTA_CommonAllocationManagementCert;

namespace MTA_AuthAllocationManagementCert.Controllers
{
    public class RoleController : Controller
    {
        private RoleManager<AppRole> _roleManager { get; }
        private UserManager<AppUser> _userManager { get; }
        public RoleController(RoleManager<AppRole> roleManager, UserManager<AppUser> userManager)
        {
            this._roleManager = roleManager;
            this._userManager = userManager;
        }
        public async Task<IActionResult> CreateRole(AddRoleReq objReq)
        {
            var user = await _userManager.FindByNameAsync(objReq.UserName);
            var roleOfUser = await _userManager.GetRolesAsync(user);
            if (User.IsInRole(ConstantRole.Admin))
            {
                var role = new AppRole()
                {
                    Id = Guid.NewGuid().ToString(),
                    DisplayName = "administrator",
                    Name = "admin"
                };
                var roleExist = await _roleManager.RoleExistsAsync(ConstantRole.Admin);
                if (!roleExist)
                {
                    var result = await _roleManager.CreateAsync(role);
                }
                ViewBag.test = "Có vai trò";
            }
            else
            {
                ViewBag.test = "Không có vai trò";
            }
            return View();
        }

        [HttpPost]
        public async Task<string> AddUserToRole()
        {
            var result = string.Empty;
            var user = await _userManager.FindByNameAsync("dandan");
            var roleOfUser = await _userManager.GetRolesAsync(user);
            var result1 = await _userManager.AddToRoleAsync(user, "admin");
            if (result1.Succeeded)
            {
                result = "Success";
            }
            return result;
        }
    }
}