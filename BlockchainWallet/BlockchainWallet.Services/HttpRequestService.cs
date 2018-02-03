using System;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace BlockchainWallet.Services
{
    public class HttpRequestService : IHttpRequestService
    {
        public void SendRequestAsync(string uri, string data, string method, bool useEncodeBase64 = false)
        {
            try
            {
                var jsonObject = data;

                if (useEncodeBase64)
                {
                    var strAsBytes = Encoding.UTF8.GetBytes(data);
                    jsonObject = Convert.ToBase64String(strAsBytes);
                }

                Task.Run(() =>
                {
                    this.SendPost(uri, jsonObject, method, useEncodeBase64);
                });
            }
            catch (Exception e)
            {
                // error handler - log error
                //var additionalInfo = $"Url: {uri}; Data: {data}";
            }
        }

        public string SendRequest(string uri, string data, string method, bool useEncodeBase64 = false)
        {
            var result = string.Empty;
            try
            {
                var jsonObject = data;

                if (useEncodeBase64)
                {
                    var strAsBytes = Encoding.UTF8.GetBytes(data);
                    jsonObject = Convert.ToBase64String(strAsBytes);
                }
                
                Task.Run(() =>
                {
                    result = this.SendPost(uri, jsonObject, method, useEncodeBase64);
                }).Wait();

                return result;
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return result;
        }
        
        private string SendPost(string uri, string data, string method, bool useEncodeBase64)
        {
            string result = "";

            using (var client = new WebClient())
            {
                try
                {
                    if (useEncodeBase64)
                    {
                        client.Headers.Add(HttpRequestHeader.ContentType, "text/plain");
                    }
                    else
                    {
                        client.Headers.Add(HttpRequestHeader.ContentType, "application/json");
                    }


                    client.Encoding = Encoding.UTF8;
                    result = client.UploadString(new Uri(uri), method, data);

                }
                catch (WebException e)
                {
                    // error handler - log error
                    //var additionalInfo = $"Url: {uri}; Data: {data}";
                }
            }

            return result;
        }
    }
}
