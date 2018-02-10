using System;

namespace BlockchainWallet.Models.Dto
{
    public class Transaction
    {
        public string From { get; set; }

        public string To { get; set; }

        public long Value { get; set; }

        public string SenderPubKey { get; set; }

        public string SenderSignature { get; set; }

        public string TransactionHash { get; set; }

        public bool Paid { get; set; }

        public DateTime DateReceived { get; set; }

        public long MinedInBlockIndex { get; set; }
    }
}
