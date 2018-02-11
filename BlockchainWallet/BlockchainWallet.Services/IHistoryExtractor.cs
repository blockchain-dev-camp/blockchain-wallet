namespace BlockchainWallet.Services
{
    using System.Collections.Generic;
    using BlockchainWallet.Models.Dto;

    public interface IHistoryExtractor
    {
        (List<Transaction> transactions, bool success) GetTransactions(string account, string urlNodeAddress, int page, int sizePerPage);
    }
}
