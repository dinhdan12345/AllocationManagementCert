using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Pages.Account.Internal;
using Microsoft.AspNetCore.Mvc;
using MTA_AllocationManagementCert.Models;
using MTA_AllocationManagementCert.Models.SystemManagement;
using Newtonsoft.Json;
using MTA_CommonAllocationManagementCert;
namespace MTA_AllocationManagementCert.Controllers.SystemManagement
{
    public class AccountController : Controller
    {
        private static readonly HttpContextAccessor _httpContextAccessor = new HttpContextAccessor();
        private readonly string BaseAuthURL = CommonFunction.GetAppSettings("SystemConfig", "SystemAuthUrl");
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
        public IActionResult Login(LoginInfoSend loginReq)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return View(loginReq);
                }
                var objReq = new LoginInfoSend();
                objReq.UserName = loginReq.UserName;
                objReq.Password = loginReq.Password;
                var objReqString = JsonConvert.SerializeObject(objReq);
                var urlLogin = "http://localhost:5000/Account/Login";
                //var objRes = CallService.CallRestService<FormBaseRes<LoginRes>>(urlLogin, "POST", objReqString);
                //if (!objRes.Success) throw new BaseException(objRes.Code, objRes.Message);
            }
            catch (Exception ex)
            {
                return RedirectToAction("Login", "Account");
            }
            return View();
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
