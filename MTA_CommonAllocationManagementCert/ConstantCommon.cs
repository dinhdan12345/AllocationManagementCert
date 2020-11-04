using System;
using System.Collections.Generic;
using System.Text;

namespace MTA_CommonAllocationManagementCert
{
    
    public static class ConstantAPI
    {
        public const string Login = "/Account/Login";

        public class AuthAPI
        {
            public const string Login = "/api/account/login";
            public const string Logout = "/api/account/logout";
        }
    }
    public static class ConstantRole
    {
        public const string Admin = "ADMIN";
        public const string SaleOfficer = "SaleOfficer";
        public const string Approval = "Approval";
        public const string Officer = "Officer";
        public const string Support = "Support";
    }
    public class ConstantCommon {
        /// <summary>
        /// Các kiểu dữ liệu có thể truyển trong API
        /// </summary>
        public class HttpContentType
        {
            public const string TextPlain = "text/plain";
            public const string AppicationJson = "application/json";
            public const string ApplicationXwwwFormUnlencoded = "application/x-www-form-urlencoded";


        }
        /// <summary>
        /// Các kiểu phương thức để gọi API
        /// </summary>
        public class HttpMethod
        {
            public const string Post = "post";
            public const string Get = "get";
            public const string Put = "put";
            public const string Patch = "patch";
            public const string Delete = "delete";

        }
        /// <summary>
        /// Mô tả các mã lỗi
        /// </summary>
        public class ErrorCode
        {
            //Mã lỗi mặc định
            public const int Default = -1;

            //Thành công
            public const int Successfully = 0;

            public const int InternalError = 1;
            public const int AccessIsDenied = 2;
        }
        public class ErrorMessage
        {
            //Mô tả lỗi mặc định
            public const string Default = "Default";

            //Mô tả lỗi thành công
            public const string Successfully = "Successfully";
            // Không thành công
            public const string InternalError = "InternalError";
        }
    }

    public static class ServerConstant
    {
        /// <summary>
        /// Map lỗi server tích hợp trả về do mạng, hoặc quá tải
        /// </summary>
        public static Dictionary<int, string> MapServerError = new Dictionary<int, string>
        {
            {404, "Service Not Found" },
            {500,"Internal Server" },
            {502,"Bad Gateway" },
            {503,"Service Unavailable" },
            {504,"Gateway timeout" },
        };
    }
}
