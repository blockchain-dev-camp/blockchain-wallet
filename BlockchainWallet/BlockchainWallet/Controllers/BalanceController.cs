using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockchainWallet.Models.Domain;
using BlockchainWallet.Models.Dto;
using BlockchainWallet.Services;
using BlockchainWallet.Utils.Globals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Newtonsoft.Json;

namespace BlockchainWallet.Controllers
{
    public class BalanceController : BaseController
    {
        public BalanceController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet]
        public IActionResult Balance()
        {
            var dtoAsStr = this.TempData[TempDataKeys.BalanceDto] as string;
            BalanceDto dto = null;
            if (string.IsNullOrWhiteSpace(dtoAsStr))
            {
                dto = new BalanceDto();
                
            }
            else
            {
                dto = JsonConvert.DeserializeObject<BalanceDto>(dtoAsStr);
            }

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Balance(BalanceDto dto)
        {
            // if valid ...
            dto.ShouldCheckBalance = true;
            this.TempData[TempDataKeys.BalanceDto] = JsonConvert.SerializeObject(dto);
            return this.RedirectToAction(nameof(this.Balance));
        }

        [HttpPost]
        public JsonResult BalanceAjax(string account)
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
            var result = new { isSuccess = true, balance = balance };
            return this.Json(result);
        }


    }
}