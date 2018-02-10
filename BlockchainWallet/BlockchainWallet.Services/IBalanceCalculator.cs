namespace BlockchainWallet.Services
{
    public interface IBalanceCalculator
    {
        decimal GetBalance(string account, string urlNodeAddress, int page, int sizePerPage);
    }
}
