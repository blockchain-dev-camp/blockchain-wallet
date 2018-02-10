using BlockchainWallet.Models.ViewModels;

namespace BlockchainWallet.Data.Repos
{
    class TransactionRepository : Repository<TransactionModel>
    {
        public TransactionRepository(BlockchainDbContext context) : base(context)
        {
        }
    }
}
