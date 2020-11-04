using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace MTA_AuthAllocationManagementCert.Models
{
    public class AuthModel
    {
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
    public class LogInReq 
    {
        [Required]
        public string UserName { get; set; }
        [Required]
        public string Password { get; set; }
    }
    public class LogInRes 
    {
        public string SystemToken { get; set; }
        public string SystemExpiredDate { get; set; }
        public bool IsFirstLogin { get; set; } = false;
        /// <summary>
        /// Cờ kiểm tra xem nhân viên có thay đổi văn phòng không thì thay đổi các bản ghi theo dataKey
        /// </summary>
        public bool IsChange { get; set; } = false;

        public string AccessToken { get; set; }
        //public string RefreshToken { get; set; }
        //public int ExpiresIn { get; set; }
        public UserData User { get; set; }
    }
}
