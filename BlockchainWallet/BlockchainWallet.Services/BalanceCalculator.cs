﻿using BlockchainWallet.Models.Dto;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Newtonsoft.Json;
using Microsoft.Extensions.Configuration;

namespace BlockchainWallet.Services
{
    public class BalanceCalculator : IBalanceCalculator
    {
        private string defaultBlockQueryUrl = "?page={0}&size={1}";

        private IHttpRequestService httpRequestService;

        public BalanceCalculator(IHttpRequestService httpRequestService)
        {
            this.httpRequestService = httpRequestService;
        }
        //protected string NodeData
        //{
        //    get
        //    {
        //        var builder = new ConfigurationBuilder();
        //        var config = builder.Build();

        //        var settings = new AppSettings();
        //        config.GetSection("App").Bind(settings);
        //        return null;
        //    }
        //}

        public long GetBalance(string account, string urlNodeAddress, int page, int sizePerPage)
        {
            bool isRunning = true;
            Balance balance = new Balance();

            // get blocks from Node
            var blocks = this.GetBlocks(urlNodeAddress, page++, sizePerPage);

            if (blocks == null)
            {
                //todo log error
                return balance.Current;
            }

            while (isRunning)
            {
                // get transactions from blocks and filter those ones that contain current address.
                var neededTransaction = blocks.SelectMany(x => x.Transactions).Where(x => (x.From == account || x.To == account) && x.Paid);

                // calculate balance
                this.CalculateBalanceByTransactions(neededTransaction, balance, account);

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

            return balance.Current;
        }

        private void CalculateBalanceByTransactions(IEnumerable<Transaction> transactions, Balance balance, string account)
        {
            if (transactions == null || balance == null || string.IsNullOrWhiteSpace(account))
            {
                return;
            }
            
            foreach (var transaction in transactions)
            {
                if (transaction.To == account)
                {
                    balance.Income += transaction.Value;
                }
                else if (transaction.From == account)
                {
                    balance.Outcome += transaction.Value;
                }
            }
        }

        private IEnumerable<Block> GetBlocks(string nodeAddress, int page, int sizePerPage)
        {
            nodeAddress = nodeAddress + string.Format(defaultBlockQueryUrl, page, sizePerPage);

            var blocksAsJson = string.Empty;
            var success = false;
            (blocksAsJson, success) = this.httpRequestService.SendRequest(nodeAddress, string.Empty, "GET");

            IEnumerable<Block> blocks = null;

            try
            {
                blocks = JsonConvert.DeserializeObject<IEnumerable<Block>>(blocksAsJson);
            }
            catch (Exception e)
            {
                //todo log error
                Console.WriteLine(e.Message);
                throw;
            }

            return blocks;
        }
    }
}
