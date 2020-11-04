using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MTA_AllocationManagementCert.Common
{
    public class UserSettings
    {
    }
    /// <summary>
    /// Loại claim do người dùng định nghĩa
    /// </summary>
    public class MyClaimTypes
    {
        public const string UserId = "UserId";
        public const string FullName = "FullName";
        public const string IsLoginMisaId = "IsLoginMisaId";
        public const string SystemToken = "SystemToken";
        public const string AccessToken = "AccessToken";
    }
}
