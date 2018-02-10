namespace BlockchainWallet.Models.Dto
{
    public class Balance
    {
        public decimal Income { get; set; }
        public decimal Outcome { get; set; }

        public decimal Current => this.Income - this.Outcome;
    }
}
