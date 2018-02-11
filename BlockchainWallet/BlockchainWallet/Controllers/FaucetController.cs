using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using BlockchainWallet.Models.Dto;
using BlockchainWallet.Services;
using BlockchainWallet.Utils.Globals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace BlockchainWallet.Controllers
{
    public class FaucetController : BaseController
    {
        private IOptions<NodeData> nodeSettings;

        public FaucetController(IOptions<NodeData> nodeSettings, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.nodeSettings = nodeSettings;
        }

        [HttpGet]
        public IActionResult GetCoins()
        {
            var dto = this.GetDtoFromTempData<FaucetDto>(TempDataKeys.FaucetDto);

            if (dto == null)
            {
                dto = new FaucetDto();
            }

            return View(dto);
        }

        [HttpPost]
        public IActionResult GetCoins(FaucetDto dto)
        {
            var addressService = this.ServiceProvider.GetService<AddressService>();
            var httpRequestService = this.ServiceProvider.GetService<IHttpRequestService>();
            var nodeData = this.nodeSettings.Value;
            var transactionManager = this.ServiceProvider.GetService<ITransactionManager>();

            var response = string.Empty;
            var success = false;

            var transferAmount = 0.3m;

            var transaction = new TransactionDto()
            {
                TransferAmount = transferAmount,
                ReceiverAccount = dto.ToAddress,
                PrivateKey = "57da87852534fc39cec621550a0b701e18132b92f924172ace529490ebdafb04",
                Account = "6990b6cf60e4d14e6f5a17e787ce2d38564ad0d8"
            };

            (response, success) = transactionManager.MakeTransaction(addressService, httpRequestService, nodeData, transaction);


            if (success)
            {
                dto.Amount = transferAmount;
                this.AddDtoToTempData(TempDataKeys.FaucetDto, dto);
            }
            
            
            return this.RedirectToAction(nameof(this.GetCoins));
        }

        
    }
}