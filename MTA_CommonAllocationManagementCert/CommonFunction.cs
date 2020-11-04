using iTextSharp.text.pdf;
using iTextSharp.text.pdf.security;
using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace MTA_CommonAllocationManagementCert
{
    public static class SysConfig
    {
        public static IConfiguration configuration;
    }
    public class CommonFunction
    {
        private static readonly IConfiguration _configuration;

        static CommonFunction()
        {
            _configuration = SysConfig.configuration;
        }
        public class StringToDate
        {
            public static string DateTimeFormatDefault = "dd/MM/yyyy HH:mm:ss fff";
            public static string DateFormatDefault = "dd/MM/yyyy";
            public static string DateTimeFormat = "dd/MM/yyyy HH:mm:ss";
            public static CultureInfo provider = new CultureInfo("fr-FR");

            public static DateTime GetDefault(string dateString)
            {
                var result = new DateTime();
                try
                {
                    result = DateTime.ParseExact(dateString, DateTimeFormatDefault, CultureInfo.InvariantCulture);
                }
                catch (Exception e)
                {
                    //No anything
                }
                return result;
            }
        }
        public static string GetConnectString()
        {
            return _configuration.GetConnectionString("DefaultConnection");
        }

        public static string GetConnectString(string name)
        {
            return _configuration.GetConnectionString(name);
        }

        public static string GetAppSettings(string name)
        {
            try
            {
                return _configuration.GetValue<string>($"Appsettings:{name}") ?? string.Empty;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }

        public static string GetAppSettings(string name, string subName)
        {
            try
            {
                return _configuration.GetValue<string>($"Appsettings:{name}:{subName}") ?? string.Empty;
            }
            catch (Exception e)
            {
                return string.Empty;
            }
        }
    }
    public class CallService
    {
        private const int TimeOutDefault = 90;
        /// <summary>
        /// Tải file từ một server khác
        /// </summary>
        /// <param name="downloadURL"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static async Task<byte[]> DownloadFileDataByUrl(string downloadURL, Dictionary<string, string> param)
        {
            var result = new byte[0];
            try
            {
                using (WebClient webClient = new WebClient())
                {
                    //2. Header
                    if (param != null && param.Count > 0)
                    {
                        foreach (var item in param)
                        {
                            webClient.Headers.Add(item.Key, item.Value);
                        }
                    }
                    result = await webClient.DownloadDataTaskAsync(downloadURL);
                }
            }
            catch (Exception e)
            {
                throw e;
            }
            return result;
        }
        public static T CallRestService<T>(string url, string action, string jsonData, string contentType, Dictionary<string, string> param, int timeOut = TimeOutDefault)
        {
            //1. Url + Method
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = action;
            if (timeOut != TimeOutDefault)
            {
                httpWebRequest.Timeout = timeOut * 1000;
            }
            //2. Header
            if (param != null && param.Count > 0)
            {
                foreach (var item in param)
                {
                    httpWebRequest.Headers.Add(item.Key, item.Value);
                }
            }

            //3. Data body (không cần dùng cho phương thức get, vậy nên với phương thức get cũng không cần dùng content type)
            if (!action.ToLower().Equals("get"))
            {
                //4.
                httpWebRequest.ContentType = contentType;
                if (timeOut != TimeOutDefault)
                {
                    httpWebRequest.Timeout = timeOut * 1000;
                }
                if (contentType.ToLower().Equals(ConstantCommon.HttpContentType.AppicationJson.ToLower()))
                {
                    using (var streamWRiter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWRiter.Write(jsonData);
                        streamWRiter.Flush();
                        streamWRiter.Close();
                    }
                }
                else if (contentType.ToLower().Equals(ConstantCommon.HttpContentType.ApplicationXwwwFormUnlencoded.ToLower()))
                {
                    using (var streamWRiter = httpWebRequest.GetRequestStream())
                    {
                        var byteArray = System.Text.Encoding.UTF8.GetBytes(jsonData);
                        streamWRiter.Write(byteArray, 0, byteArray.Length);
                        streamWRiter.Flush();
                        streamWRiter.Close();
                    }
                }

            }

            var httpResPonse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResPonse.GetResponseStream()))
            {
                var resultstring = streamReader.ReadToEnd();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resultstring);
            }
        }
        /// <summary>
        /// Goi Rest Service với kiểu dữ liệu dạng JSON, không có header (Vì nhiều API chỉ cần sử dụng đến phương thức này nên bớt 1 số tham số như header, content type là json)
        /// </summary>
        /// <typeparam name="T">Object trả ra</typeparam>
        /// <param name="url">Đường dẫn API</param>
        /// <param name="action">Phương thức thực hiện (Get/Post/...)</param>
        /// <param name="jsonData">Tham số truyền vào dạng String Json</param>
        /// Created by: NVThanh (30/07)
        /// Updated by: NVThanh (10/08)
        /// Updated by: NhThang (13/04/2020) Cập nhật retry pattern khi gặp lỗi 504
        /// <returns></returns>
        public static T CallRestService<T>(string url, string action, string jsonData)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = action;

            //Nếu là phương thức get thì không cần truyền data và stream
            if (!action.ToLower().Equals(ConstantCommon.HttpMethod.Get.ToLower()))
            {
                httpWebRequest.ContentType = ConstantCommon.HttpContentType.AppicationJson;
                using (var streamWRiter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWRiter.Write(jsonData);
                    streamWRiter.Flush();
                    streamWRiter.Close();
                }
            }
            var httpResPonse = (HttpWebResponse)httpWebRequest.GetResponse();
            //Kiểm tra nếu gặp lỗi 500,502,503,504 thì thử lại 
            if (ServerConstant.MapServerError.ContainsKey((int)httpResPonse.StatusCode))
            {
                Retry.Do(() =>
                {
                    //Nếu là phương thức get thì không cần truyền data và stream
                    if (!action.ToLower().Equals(ConstantCommon.HttpMethod.Get.ToLower()))
                    {
                        httpWebRequest.ContentType = ConstantCommon.HttpContentType.AppicationJson;
                        using (var streamWRiter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWRiter.Write(jsonData);
                            streamWRiter.Flush();
                            streamWRiter.Close();
                        }
                    }
                    httpResPonse = (HttpWebResponse)httpWebRequest.GetResponse();
                }, TimeSpan.FromSeconds(1));
            }
            using (var streamReader = new StreamReader(httpResPonse.GetResponseStream()))
            {
                var resultstring = streamReader.ReadToEnd();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resultstring);
            }
        }
        /// <summary>
        /// Custom hàm base sử dụng cho gửi mail (AIMarketing)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <param name="jsonData"></param>
        /// <returns></returns>
        public static string CallRestService(string url, string action, string jsonData)
        {
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = action;

            //Nếu là phương thức get thì không cần truyền data và stream
            if (!action.ToLower().Equals(ConstantCommon.HttpMethod.Get.ToLower()))
            {
                httpWebRequest.ContentType = ConstantCommon.HttpContentType.AppicationJson;
                using (var streamWRiter = new StreamWriter(httpWebRequest.GetRequestStream()))
                {
                    streamWRiter.Write(jsonData);
                    streamWRiter.Flush();
                    streamWRiter.Close();
                }
            }
            var httpResPonse = (HttpWebResponse)httpWebRequest.GetResponse();
            //Kiểm tra nếu gặp lỗi 500,502,503,504 thì thử lại 
            if (ServerConstant.MapServerError.ContainsKey((int)httpResPonse.StatusCode))
            {
                Retry.Do(() =>
                {
                    //Nếu là phương thức get thì không cần truyền data và stream
                    if (!action.ToLower().Equals(ConstantCommon.HttpMethod.Get.ToLower()))
                    {
                        httpWebRequest.ContentType = ConstantCommon.HttpContentType.AppicationJson;
                        using (var streamWRiter = new StreamWriter(httpWebRequest.GetRequestStream()))
                        {
                            streamWRiter.Write(jsonData);
                            streamWRiter.Flush();
                            streamWRiter.Close();
                        }
                    }
                    httpResPonse = (HttpWebResponse)httpWebRequest.GetResponse();
                }, TimeSpan.FromSeconds(1));
            }
            using (var streamReader = new StreamReader(httpResPonse.GetResponseStream()))
            {
                var resultstring = streamReader.ReadToEnd();
                return resultstring;
            }
        }

        /// <summary>
        /// Phương thức chung để gọi các API Restful
        /// Created by: NHTHANG (07/08/2019)
        /// Updated by: NVThanh (10/08/2019)
        /// </summary>
        /// <typeparam name="T">Kiểu object response trả về</typeparam>
        /// <param name="url">Đường dẫn API</param>
        /// <param name="action">Phương thức của API</param>
        /// <param name="jsonData">Data truyền trong body của message request</param>
        /// <param name="contentType">Kiểu dữ liệu của data</param>
        /// <param name="param">Tham số truyền trong header của request</param>
        /// <returns></returns>
        public static T CallRestService<T>(string url, string action, string jsonData, string contentType, Dictionary<string, string> param)
        {
            //1. Url + Method
            var httpWebRequest = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest.Method = action;

            //2. Header
            if (param != null && param.Count > 0)
            {
                foreach (var item in param)
                {
                    httpWebRequest.Headers.Add(item.Key, item.Value);
                }
            }
            httpWebRequest.ContentType = contentType;

            //3. Data body (không cần dùng cho phương thức get, vậy nên với phương thức get cũng không cần dùng content type)
            if (!action.ToLower().Equals(ConstantCommon.HttpMethod.Get.ToLower()))
            {
                //4.
                httpWebRequest.ContentType = contentType;

                //Cần kiểm tra content type để truyền đúng kiểu dữ liệu cho Stream
                //Với "application/json": Json string
                //Với "application/x-www-form-urlencoded": byte[]

                if (contentType.ToLower().Equals(ConstantCommon.HttpContentType.AppicationJson.ToLower()))
                {
                    using (var streamWRiter = new StreamWriter(httpWebRequest.GetRequestStream()))
                    {
                        streamWRiter.Write(jsonData);
                        streamWRiter.Flush();
                        streamWRiter.Close();
                    }
                }
                else if (contentType.ToLower().Equals(ConstantCommon.HttpContentType.ApplicationXwwwFormUnlencoded.ToLower()))
                {
                    using (var streamWRiter = httpWebRequest.GetRequestStream())
                    {
                        var byteArray = System.Text.Encoding.UTF8.GetBytes(jsonData);
                        streamWRiter.Write(byteArray, 0, byteArray.Length);
                        streamWRiter.Flush();
                        streamWRiter.Close();
                    }
                }

            }

            var httpResPonse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (var streamReader = new StreamReader(httpResPonse.GetResponseStream()))
            {
                var resultstring = streamReader.ReadToEnd();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resultstring);
            }
        }

        /// <summary>
        /// Phương thức chung call Http Client API
        /// Created by: NHThang(17/06/2020)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="url"></param>
        /// <param name="action"></param>
        /// <param name="jsonData"></param>
        /// <param name="contentType"></param>
        /// <param name="param"></param>
        /// <returns></returns>
        public static T CallHttpClient<T>(string url, string action, string jsonData, string contentType, Dictionary<string, string> param)
        {
            //1. Url + Method
            var httpRequest = new HttpRequestMessage();
            if (action == "GET")
                httpRequest.Method = HttpMethod.Get;
            else
                httpRequest.Method = HttpMethod.Post;
            httpRequest.RequestUri = new Uri(url);

            //2. Header
            if (param != null && param.Count > 0)
            {
                foreach (var item in param)
                {
                    httpRequest.Headers.Add(item.Key, item.Value);
                }
            }
            httpRequest.Content = new StringContent(jsonData, Encoding.UTF8, contentType);
            var httpResPonse = new HttpResponseMessage();
            using (HttpClient client = new HttpClient() { Timeout = TimeSpan.FromSeconds(5) })
            {
                httpResPonse = client.SendAsync(httpRequest).Result;
            }

            if (httpResPonse.StatusCode == HttpStatusCode.OK)
            {
                var resultstring = httpResPonse.Content.ReadAsStringAsync();
                return Newtonsoft.Json.JsonConvert.DeserializeObject<T>(resultstring.Result);
            }
            else
            {
                return default(T);
            }

        }

    }
   
        /// <summary>
        /// Lớp static để áp dụng Retry Design Patter gọi đến hệ thống khác với những kết nối quan trọng
        /// VD: Gọi sang MISAID để tạo tài khoản, gọi sang eSignCloud để cấp chứng thư số
        /// Created by: NVThanh ()
        /// </summary>
        public static class Retry
    {
        public static void Do(
            Action action,
            TimeSpan retryInterval,
            int maxAttemptCount = 3)
        {
            Do<object>(() =>
            {
                action();
                return null;
            }, retryInterval, maxAttemptCount);
        }

        public static T Do<T>(
            Func<T> action,
            TimeSpan retryInterval,
            int maxAttemptCount = 3)
        {
            var exceptions = new List<Exception>();

            for (int attempted = 0; attempted < maxAttemptCount; attempted++)
            {
                try
                {
                    if (attempted > 0)
                    {
                        Thread.Sleep(retryInterval);
                    }
                    return action();
                }
                catch (Exception ex)
                {
                    exceptions.Add(ex);
                }
            }
            throw new AggregateException(exceptions);
        }
    }

    public static class VerifySignature
    {
        public static Boolean VerifyPDFFile(byte[] file)
        {

            PdfReader reader = new PdfReader(file);
            AcroFields acroFields = reader.AcroFields;
            List<String> signatureNames = acroFields.GetSignatureNames();


            if (signatureNames.Count == 0)
            {
                return false;
                //throw new InvalidOperationException("No Signature present in pdf file.");
            }

            foreach (string name in signatureNames)
            {
                if (!acroFields.SignatureCoversWholeDocument(name))
                {
                    //return false;
                    //throw new InvalidOperationException(string.Format("The signature: {0} does not covers the whole document.", name));
                }


                PdfPKCS7 pk = acroFields.VerifySignature(name);
                var cal = pk.SignDate;
                var pkc = pk.Certificates;

                if (!pk.Verify())
                {
                    Console.WriteLine("The signature is not valid.");
                    return false;
                }
                //if (!pk.VerifyTimestampImprint())
                //{
                //    Console.WriteLine("The timestamp is not valid.");
                //}
            }
            return true;
        }
    }
    public class BaseException : ApplicationException
    {
        public int Code;
        public BaseException(int code, string message) : base(message)
        {
            Code = code;
        }
    }
}
