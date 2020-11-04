using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using MTA_AllocationManagementCert.Models;
using Newtonsoft.Json;
using MTA_CommonAllocationManagementCert;
using System.Security.Claims;
using MTA_AllocationManagementCert.Common;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Http.Authentication;
using Microsoft.AspNetCore.Authentication;

namespace MTA_AllocationManagementCert.Controllers.SystemManagement
{
    public class AccountController : Controller
    {
        private static readonly HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        private readonly string BaseAuthURL = CommonFunction.GetAppSettings("DisableAuth");
        /// <summary>
        /// Hàm khởi tạo
        /// </summary>
        public AccountController()
        {

        }
        /// <summary>
        /// Màn hình đăng nhập
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }
        /// <summary>
        /// Đăng nhập
        /// </summary>
        /// <param name="loginModel"></param>
        /// <returns></returns>
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginInfoSend loginReq)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(loginReq);
                }
                var objReq = new LogInReq();
                objReq.UserName = loginReq.UserName;
                objReq.Password = loginReq.Password;
                var objReqString = JsonConvert.SerializeObject(objReq);
                var urlLogin = $"{BaseAuthURL}{ConstantAPI.AuthAPI.Login}";
                var objRes = CallService.CallRestService<FormBaseRes<LogInRes>>(urlLogin, "POST", objReqString);
                if (!objRes.Success) throw new BaseException(objRes.Code, objRes.Message);
                //TODO: Kiểm tra nếu đăng nhập lần đầu sẽ lưu thông tin vào db

                var claims = new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Expired, objRes.Data.SystemExpiredDate));
                claims.Add(new Claim(MyClaimTypes.FullName, $"{objRes.Data.User.LastName} {objRes.Data.User.FirstName}"));
                claims.Add(new Claim(MyClaimTypes.UserId, objRes.Data.User.UserId));
                claims.Add(new Claim(MyClaimTypes.SystemToken, objRes.Data.SystemToken));
                claims.Add(new Claim(MyClaimTypes.AccessToken, objRes.Data.AccessToken));
                var claimsPrincipal = new ClaimsPrincipal(new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme));
                await HttpContext.SignInAsync(claimsPrincipal, new Microsoft.AspNetCore.Authentication.AuthenticationProperties
                {
                    IsPersistent = true
                });
                return RedirectToAction("Index", "Home");
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");
            }
            
        }
        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        public IActionResult Logout()
        {

            return View();

        }
    }
}
