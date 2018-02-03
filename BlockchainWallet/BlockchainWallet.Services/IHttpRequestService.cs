﻿using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainWallet.Services
{
    public interface IHttpRequestService
    {
        void SendRequestAsync(string uri, string data, string method, bool useEncodeBase64 = false);
        string SendRequest(string uri, string data, string method, bool useEncodeBase64 = false);
    }
}