using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainWallet.Models.Dto
{
    public class Block
    {
        public Block()
        {
            this.Transactions = new List<Transaction>();
        }

        public uint Index { get; set; }

        public int Difficulty { get; set; }

        public string PrevBlocHash { get; set; }

        public string MinedBy { get; set; }

        public string BlockDataHash { get; set; }

        public long Nonce { get; set; }

        public DateTime DateCreated { get; set; }

        public string BlockHash { get; set; }

        public  IList<Transaction> Transactions { get; set; }
    }
}
