using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
        private readonly string LOGIN_PROVIDER = CommonFunction.GetAppSettings("SystemConfig", "LoginProvider");
        private readonly string PASSDEFAULT = CommonFunction.GetAppSettings("SystemConfig", "PasswordDefault");
        private readonly string KEY = CommonFunction.GetAppSettings("SystemConfig", "SecretKey");
        private readonly int USER_EXPIRES_IN = Convert.ToInt32(CommonFunction.GetAppSettings("SystemConfig", "UserTokenExpiresIn"));

        private UserManager<AppUser> _userManager { get; }
        private SignInManager<AppUser> _signInManager { get; }
        private readonly DbContextAuth _dbContextAuth;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager, DbContextAuth dbContextAuth)
        {
            _userManager = userManager;
            _signInManager = signInManager;
            _dbContextAuth = dbContextAuth;
        }
        [Route(ConstantAPI.AuthAPI.Login)]
        [HttpPost]
        [AllowAnonymous]
        public async Task<FormBaseRes<LogInRes>> Login(LogInReq loginReq)
        {
            var objRes = new FormBaseRes<LogInRes>() {
                Data = new LogInRes()
                {
                    User = new UserData()
                }
            };
            try
            {
                var result = await _signInManager.PasswordSignInAsync(loginReq.UserName, loginReq.Password, false, false);
                if (result.Succeeded)
                {
                    var user = await _userManager.FindByNameAsync(loginReq.UserName);
                    if (user == null)
                        throw new BaseException(ConstantCommon.ErrorCode.AccessIsDenied, "Không tìm thấy tài khoản của bạn trong hệ thống!");

                    var roles = await _userManager.GetRolesAsync(user);
                    var role = string.Empty;
                    if (roles != null)
                    {
                        role = string.Join(",", roles);
                    }
                    var claims = await _userManager.GetClaimsAsync(user);
                    claims.Add(new Claim("Id", user.Id));
                    claims.Add(new Claim(ClaimTypes.NameIdentifier, user.Id));
                    claims.Add(new Claim(ClaimTypes.Name, user.UserName.ToString()));
                    claims.Add(new Claim(ClaimTypes.Role, role));
                    var claimIdentity = new ClaimsIdentity(claims);


                    objRes.Data.User.UserId = user.Id;
                    objRes.Data.User.MisaIdKey = user.MisaIdUserId;
                    objRes.Data.User.Email = user.Email;
                    objRes.Data.User.PhoneNumber = user.PhoneNumber;
                    objRes.Data.User.FirstName = user.FirstName;
                    objRes.Data.User.LastName = user.LastName;
                    //Success
                    objRes.Success = true;
                    objRes.Code = ConstantCommon.ErrorCode.Successfully;
                    objRes.Message = ConstantCommon.ErrorMessage.Successfully;


                    //TODO: Lưu lần đăng nhập 

                    //try
                    //{
                    //}
                    //catch (Exception e)
                    //{
                    //}
                }

            }
            catch (BaseException e)
            {
                objRes.Code = e.Code;
                objRes.Message = e.Message;
            }
            catch (Exception e)
            {
                objRes.Code = ConstantCommon.ErrorCode.InternalError;
                objRes.Message = "Lỗi hệ thống!";
            }
            return objRes;

        }
        /// <summary>
        /// Đăng xuất
        /// </summary>
        /// <returns></returns>
        [Route(ConstantAPI.AuthAPI.Logout)]
        [HttpPost]
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
