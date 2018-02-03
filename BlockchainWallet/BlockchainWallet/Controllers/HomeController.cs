using System;
using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using BlockchainWallet.Models;
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

        public JsonResult Balance(string nodeAddress)
        {
            var balanceCalculator = this.ServiceProvider.GetService<IBalanceCalculator>();

            var balance = balanceCalculator.GetBalance("url", nodeAddress, 1, 500);
            var result = new {balance = balance};
            return this.Json(result);
        }
    }
}
