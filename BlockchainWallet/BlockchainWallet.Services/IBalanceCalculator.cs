namespace BlockchainWallet.Services
{
    public interface IBalanceCalculator
    {
        (decimal balance, bool success) GetBalance(string account, string urlNodeAddress, int page, int sizePerPage);
    }
}
