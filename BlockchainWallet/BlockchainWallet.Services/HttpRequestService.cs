using System;
using System.IO;
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
                    this.Send(uri, jsonObject, method, useEncodeBase64);
                });
            }
            catch (Exception e)
            {
                // error handler - log error
                //var additionalInfo = $"Url: {uri}; Data: {data}";
            }
        }

        public (string response, bool success) SendRequest(string uri, string data, string method, bool useEncodeBase64 = false)
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
                    result = this.Send(uri, jsonObject, method, useEncodeBase64);
                }).Wait();

                return (result, !string.IsNullOrWhiteSpace(result));
            }
            catch (Exception e)
            {
                result = e.Message;
            }

            return (result, false);
        }
        
        private string Send(string uri, string data, string method, bool useEncodeBase64)
        {
            string result = "";

            if (method.ToLower() == "get")
            {
                return this.SendGet(uri);
            }

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

        private string SendGet(string url)
        {
            string result = string.Empty;

            try
            {
                HttpWebRequest request = (HttpWebRequest)WebRequest.Create(url);
                request.Method = "GET";
                using (HttpWebResponse response = (HttpWebResponse)request.GetResponse())
                {
                    Stream dataStream = response.GetResponseStream();
                    StreamReader reader = new StreamReader(dataStream);
                    result = reader.ReadToEnd();
                    reader.Close();
                    dataStream.Close();
                }
            }
            catch (Exception e)
            {
                //todo log error
                Console.WriteLine(e.Message);
            }

            return result;
        }
    }
}
