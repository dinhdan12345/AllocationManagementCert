using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTA_AuthAllocationManagementCert.Models
{
    public class AccountModel
    {

    }
    public class LoginRes
    {
        public string SystemToken { get; set; }
        public string SystemExpiredDate { get; set; }
        public bool IsFirstLogin { get; set; } = false;
        public bool IsChange { get; set; } = false;
        public string AccessToken { get; set; }
    }
    public class LoginReq
    {
        public string UserName { get; set; }
        public string PassWord { get; set; }
    }
}
