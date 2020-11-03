using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MTA_AllocationManagementCert.Models.SystemManagement
{
    public class LoginInfoSend
    {
        [Required(ErrorMessage = "Tên đăng nhập không được để trống.")]
        public string UserName { get; set; }
        [Required(ErrorMessage = "Mật khẩu không được để trống.")]
        public string Password { get; set; }
        //[Required(ErrorMessage = "Vui lòng nhập mã captcha.")]
        public string Captcha { get; set; }
        [Required]
        public bool AutoLogin { get; set; }
    }

    public class AccountInfo
    {
        public string UserName { get; set; }
        public string Password { get; set; }
        public bool IsAdmin { get; set; }
    }
    public class UserData
    {
        public string UserId { get; set; }
        public string MisaIdKey { get; set; }
        public string Email { get; set; }
        public string PhoneNumber { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int Status { get; set; }
    }
    public class LoginRes
    {
        public string SystemToken { get; set; }
        public string SystemExpiredDate { get; set; }
        public bool IsFirstLogin { get; set; } = false;
        public bool IsChange { get; set; } = false;
        public string AccessToken { get; set; }
        public UserData User { get; set; }
    }
}
