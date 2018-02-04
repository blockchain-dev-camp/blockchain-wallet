namespace BlockchainWallet.Models.Dto
{
    public class Balance
    {
        public long Income { get; set; }
        public long Outcome { get; set; }

        public long Current => this.Income - this.Outcome;
    }
}
