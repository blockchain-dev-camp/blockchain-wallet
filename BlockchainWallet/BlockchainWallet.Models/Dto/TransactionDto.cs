using System.ComponentModel.DataAnnotations;

namespace BlockchainWallet.Models.Dto
{
    public class TransactionDto
    {
        [Required]
        [MinLength(64), MaxLength(64)]
        public string PrivateKey { get; set; }

        [Required]
        [MinLength(40), MaxLength(100)]
        public string Account { get; set; }

        [Range(0.0001, double.MaxValue)]
        public decimal Balance { get; set; }

        [Required]
        [MinLength(40), MaxLength(100)]
        public string ReceiverAccount { get; set; }

        [Range(0.0001, double.MaxValue)]
        public decimal TransferAmount { get; set; }

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

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }
    }
}
