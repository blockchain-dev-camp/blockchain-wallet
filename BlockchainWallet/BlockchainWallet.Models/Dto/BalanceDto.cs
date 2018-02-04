namespace BlockchainWallet.Models.Dto
{
    public class BalanceDto
    {
        public string Account { get; set; }

        public long Amount { get; set; }

        public bool ShouldCheckBalance { get; set; }
    }
}
