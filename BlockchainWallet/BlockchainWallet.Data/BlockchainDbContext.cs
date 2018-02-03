using BlockchainWallet.Models;
using Microsoft.EntityFrameworkCore;

namespace BlockchainWallet.Data
{
    public class BlockchainDbContext : DbContext
    {
        public BlockchainDbContext(DbContextOptions<BlockchainDbContext> options)
            : base(options)
        {
        }

        public DbSet<TestModel> TestModels { get; set; }

        protected override void OnModelCreating(ModelBuilder builder)
        {
        }
    }
}
