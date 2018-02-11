using System;
using System.Collections.Generic;
using BlockchainWallet.Models.Domain;
using BlockchainWallet.Models.Dto;
using BlockchainWallet.Services;
using BlockchainWallet.Utils.Globals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BlockchainWallet.Controllers
{
    public class HistoryController : BaseController
    {
        private IOptions<NodeData> Settings { get; set; }

        public HistoryController(IOptions<NodeData> settings, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.Settings = settings;
        }

        [HttpGet]
        public IActionResult Index()
        {
            HistoryDto dto = new HistoryDto(); ;

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Index(string account)
        {
            HistoryDto history = new HistoryDto(account);
            var historyExtranctor = this.ServiceProvider.GetService<IHistoryExtractor>();
            var nodeData = this.Settings.Value;

            foreach (var nodeAddress in nodeData.Url)
            {
                List<Transaction> transactions = new List<Transaction>();
                bool success = false;

                NodeInfo nodeInfo = new NodeInfo()
                {
                    UrlAddress = nodeAddress + nodeData.Endpoints.GetBlocks,
                    MaxBlocksInQuery = nodeData.MaxBlocksInQuery,
                    StartingPage = nodeData.StartingPage
                };

                (transactions, success) = historyExtranctor.GetTransactions(account, nodeInfo.UrlAddress, nodeInfo.StartingPage, nodeInfo.MaxBlocksInQuery);

                if (success)
                {
                    foreach (var tran in transactions)
                    {
                        if (!history.Transactions.Contains(tran))
                        {
                            history.Transactions.Add(tran);
                        }
                    }
;
                    break;
                }
            }

            if (history.Transactions.Count == 0)
            {
                history.Description = "The wallet doesn't have any transactions yet!";
            }

            return View(history);
        }
    }
}