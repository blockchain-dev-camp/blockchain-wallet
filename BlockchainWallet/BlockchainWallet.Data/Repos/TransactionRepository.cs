using System;
using System.Collections.Generic;
using BlockchainWallet.Models;

using System.Text;
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
