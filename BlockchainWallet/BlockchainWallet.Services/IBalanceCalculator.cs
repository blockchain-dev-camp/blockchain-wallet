namespace BlockchainWallet.Services
{
    public interface IBalanceCalculator
    {
        long GetBalance(string account, string urlNodeAddress, int page, int sizePerPage);
    }
}
