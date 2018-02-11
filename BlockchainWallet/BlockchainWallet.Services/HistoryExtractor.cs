namespace BlockchainWallet.Services
{
    using BlockchainWallet.Models.Dto;
    using Newtonsoft.Json;
    using System;
    using System.Collections.Generic;
    using System.Linq;

    public class HistoryExtractor : IHistoryExtractor
    {
        private string defaultBlockQueryUrl = "?page={0}&size={1}";

        private IHttpRequestService HttpRequestService { get; set; }

        public HistoryExtractor(IHttpRequestService httpRequestService)
        {
            this.HttpRequestService = httpRequestService;
        }

        public (List<Transaction> transactions, bool success) GetTransactions(string account, string urlNodeAddress, int page, int sizePerPage)
        {
            bool isRunning = true;
            bool success = false;
            List<Transaction> transactions = new List<Transaction>();

            // get blocks from Node
            var blocks = this.GetBlocks(urlNodeAddress, page++, sizePerPage);

            if (blocks == null || !blocks.Any())
            {
                //todo log error
                return (transactions, success);
            }

            while (isRunning)
            {
                // get transactions from blocks and filter those ones that contain current address.
                var neededTransaction = blocks.SelectMany(x => x.Transactions).Where(x => (x.FromAddress == account || x.ToAddress == account) && x.Paid);
                //var neededTransaction = blocks.SelectMany(x => x.Transactions).Where(x => (x.FromAddress == account || x.ToAddress == account));

                foreach (var tran in neededTransaction)
                {
                    if (!transactions.Contains(tran))
                    {
                        transactions.Add(tran);
                    }
                }

                // if blocks count less than sizePerPAge ... these are last blocks ... 
                if (blocks.Count() >= sizePerPage)
                {
                    blocks = this.GetBlocks(urlNodeAddress, page++, sizePerPage);
                }
                else
                {
                    isRunning = false;
                }
            }

            success = true;

            return (transactions, success);
        }

        private IEnumerable<Block> GetBlocks(string nodeAddress, int page, int sizePerPage)
        {
            nodeAddress = nodeAddress + string.Format(defaultBlockQueryUrl, page, sizePerPage);

            var blocksAsJson = string.Empty;
            var success = false;
            (blocksAsJson, success) = this.HttpRequestService.SendRequest(nodeAddress, string.Empty, "GET");

            if (!success)
            {
                return Enumerable.Empty<Block>();
            }

            IEnumerable<Block> blocks = null;

            try
            {
                blocks = JsonConvert.DeserializeObject<IEnumerable<Block>>(blocksAsJson);
            }
            catch (Exception)
            {
                //todo log error
            }

            return blocks;
        }
    }
}
