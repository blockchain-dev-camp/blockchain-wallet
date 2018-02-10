namespace BlockchainWallet.Services
{
    public interface IHttpRequestService
    {
        void SendRequestAsync(string uri, string data, string method, bool useEncodeBase64 = false);
        (string response, bool success) SendRequest(string uri, string data, string method, bool useEncodeBase64 = false);
    }
}
