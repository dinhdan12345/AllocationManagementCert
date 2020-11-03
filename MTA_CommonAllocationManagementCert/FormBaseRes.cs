using System;
using System.Collections.Generic;
using System.Text;

namespace MTA_CommonAllocationManagementCert
{
    public class FormBaseRes<T>
    {
        private readonly int Code;
        private readonly string Message;
        private readonly bool Success;
        private readonly T Data;

        public FormBaseRes()
        {
            Code = -1;
            Message = "Defalut message";
            Success = false;
        }
        public FormBaseRes(int code,string message,bool success)
        {
            Code = code;
            Message = message;
            Success = success;
        }
        public FormBaseRes(int code, string message, bool success,T data)
        {
            Code = code;
            Message = message;
            Success = success;
            Data = data;
        }
    }
}
