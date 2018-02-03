using System.ComponentModel.DataAnnotations;

namespace BlockchainWallet.Models.Domain
{
    public class NodeInfo
    {
        public int Id { get; set; }

        [MaxLength(100)]
        public string Name { get; set; }

        [MaxLength(1000)]
        public string Description { get; set; }

        [MaxLength(100)]
        public string UrlAddress { get; set; }

        public int MaxBlocksInQuery { get; set; }

        public int StartingPage { get; set; }

        //maybe we will order nodes by some criteria...
        public int Order { get; set; }
    }
}
