using System;
using System.Collections.Generic;
using System.Text;

namespace BlockchainWallet.Models.Dto
{
    public class NodeData
    {
        public string[] Url { get; set; }
        public int MaxBlocksInQuery { get; set; }
        public int StartingPage { get; set; }

        public Endpoint Endpoints { get; set; }

        public class Endpoint
        {
            public string GetBlocks { get; set; }
            public string PushTransaction { get; set; }
        }
    }
}
