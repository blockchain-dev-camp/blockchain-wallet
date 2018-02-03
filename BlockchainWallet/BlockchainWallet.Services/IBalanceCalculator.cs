using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainWallet.Services
{
    public interface IBalanceCalculator
    {
        long GetBalance(string account, string urlNodeAddress, int page, int sizePerPage);
    }
}
