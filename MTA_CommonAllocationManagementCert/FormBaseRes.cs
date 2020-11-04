using System;
using System.Collections.Generic;
using System.Text;

namespace MTA_CommonAllocationManagementCert
{
    public class FormBaseRes<T>
    {
        /// <summary>
        /// Thành công/thất bại
        /// </summary>
        public bool Success { get; set; }
        /// <summary>
        /// Mã lỗi
        /// </summary>
        public int Code { get; set; }
        /// <summary>
        /// Mô tả lỗi
        /// </summary>
        public string Message { get; set; }
        public T Data { get; set; }
        public FormBaseRes()
        {
            Success = false;
            Code = -1;
            Message = "Xử lý request thất bại";
        }
    }
}
