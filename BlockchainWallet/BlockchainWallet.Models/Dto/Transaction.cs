using System;

namespace BlockchainWallet.Models.Dto
{
    public class Transaction
    {
        public string TransactionId { get; set; }
        public string FromAddress { get; set; }

        public string ToAddress { get; set; }

        public decimal Value { get; set; }

        public string SenderPubKey { get; set; }

        public string SenderSignature { get; set; }

        public string TransactionHash { get; set; }

        public bool Paid { get; set; }

        public string DateReceived { get; set; }

        public string DateOfSign { get; set; }

        public long MinedInBlockIndex { get; set; }

        public decimal Fee { get; set; }

        public DateTime GetDateReceived()
        {
            return DateTime.UtcNow;
        }

        public DateTime GetDateOfSign()
        {
            
            var a = DateTime.Now.ToString("o");

            return  DateTime.UtcNow;
        }


    }
}
