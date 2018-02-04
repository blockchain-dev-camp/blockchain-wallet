using System.ComponentModel.DataAnnotations;

namespace BlockchainWallet.Models.Dto
{
    public class TransactionDto
    {
        //[MinLength(1), MaxLength(2)]
        public string PrivateKey { get; set; }

        //[MinLength(1), MaxLength(2)]
        public string Account { get; set; }

        [Range(0, double.MaxValue)]
        public long Balance { get; set; }

        //[MinLength(1), MaxLength(2)]
        public string ReceiverAccount { get; set; }

        [Range(0, double.MaxValue)]
        public long TransferAmount { get; set; }

        public string Message { get; set; }

        public override bool Equals(object obj)
        {
            var other = obj as TransactionDto;

            var isPrKeyValid = this.PrivateKey == other.PrivateKey;
            var isAccountValid = this.Account == other.Account;
            var isBalanceVAlid = this.Balance == other.Balance;
            var isReceiverValid = this.ReceiverAccount == other.ReceiverAccount;
            var isTransferAmountValid = this.TransferAmount == other.TransferAmount;

            return isPrKeyValid && isAccountValid && isBalanceVAlid && isReceiverValid && isTransferAmountValid;
        }
    }
}
