using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainWallet.Models.Dto
{
    public class BalanceDto
    {
        public string Account { get; set; }

        public long Amount { get; set; }

        public bool ShouldCheckBalance { get; set; }
    }
}
