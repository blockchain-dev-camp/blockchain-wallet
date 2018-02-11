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
        private const string FaucetPrivateKey = "57da87852534fc39cec621550a0b701e18132b92f924172ace529490ebdafb04";
        private const string FaucetAddress = "44a161dd6354d38eef62e571888a2d8c0d81a73c";

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
                PrivateKey = FaucetPrivateKey,
                Account = FaucetAddress
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