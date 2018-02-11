using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainWallet.Models.Dto
{
    public class FaucetDto
    {
        public string ToAddress { get; set; }

        public decimal Amount { get; set; }
    }
}
