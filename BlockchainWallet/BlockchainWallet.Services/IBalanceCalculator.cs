using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainWallet.Services
{
    public interface IBalanceCalculator
    {
        long GetBalance(string address, string nodeAddress, int page, int sizePerPage)
    }
}
