namespace BlockchainWallet.Models.Dto
{
    using System.Collections.Generic;

    public class HistoryDto
    {
        public string Account { get; set; }

        public List<Transaction> Transactions { get; set; }

        public string Description { get; set; }

        public HistoryDto()
        {
            this.Transactions = new List<Transaction>();
        }

        public HistoryDto(string account)
        {
            this.Account = account;
            this.Transactions = new List<Transaction>();
        }
    }
}
