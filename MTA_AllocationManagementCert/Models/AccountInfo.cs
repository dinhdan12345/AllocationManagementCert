using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MTA_AllocationManagementCert.Models
{
    public class AccountInfo
    {
        
    }
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
}
