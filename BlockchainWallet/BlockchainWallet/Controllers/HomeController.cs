using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlockchainWallet.Models;
using BlockchainWallet.Models.Domain;
using BlockchainWallet.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace BlockchainWallet.Controllers
{
    public class HomeController : BaseController
    {
        public HomeController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        public IActionResult Index()
        {
            
            return View();
        }

        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

        [HttpPost]
        public JsonResult Balance(string account)
        {
            //todo get from DB default Node parametars like : nodeUrl, starting page, block in on query(sizePerPage)
            NodeInfo nodeInfo = new NodeInfo()
            {
                UrlAddress = "http://178.75.234.192:5555/blocks",
                MaxBlocksInQuery = 500,
                StartingPage = 1
            };

            var balanceCalculator = this.ServiceProvider.GetService<IBalanceCalculator>();

            var balance = balanceCalculator.GetBalance(account, nodeInfo.UrlAddress, nodeInfo.StartingPage, nodeInfo.MaxBlocksInQuery);
            var result = new { isSuccess = true, balance = balance};
            return this.Json(result);
        }
    }
}
