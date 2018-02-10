using System.Security.Cryptography;
using System.Text;
using BlockchainWallet.Models.Dto;
using BlockchainWallet.Services;
using BlockchainWallet.Utils.Globals;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;

namespace BlockchainWallet.Controllers
{
    using System;

    [Route("Transaction")]
    public class TransactionController : BaseController
    {
        private IOptions<NodeData> nodeSettings;

        public TransactionController(IOptions<NodeData> nodeSettings, IServiceProvider serviceProvider) : base(serviceProvider)
        {
            this.nodeSettings = nodeSettings;
        }

        public AddressService Service { get; set; }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            TransactionDto dto = this.GetDtoFromTempData<TransactionDto>(TempDataKeys.TransactionDto);
            
            if (dto == null)
            {
                dto = new TransactionDto();
            }

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("")]
        public IActionResult Index(TransactionDto transaction)
        {
            if (!ModelState.IsValid)
            {
                //todo show exact error msg/msgs
                transaction.Message = "Invalid Data !!!";
                this.AddDtoToTempData(TempDataKeys.TransactionDto, transaction);
                return this.RedirectToAction(nameof(this.Index));
            }

            this.AddDtoToTempData(TempDataKeys.TransactionDto, transaction);

            return this.RedirectToAction(nameof(this.Confirmation));
        }

        [HttpGet]
        [Route("Confirmation")]
        public IActionResult Confirmation()
        {
            TransactionDto dto = this.GetDtoFromTempData<TransactionDto>(TempDataKeys.TransactionDto);

            if (dto == null)
            {
                //todo show exact error msg/msgs
                dto = new TransactionDto();
                dto.Message = "Missing Data ! Something goes wrong!";
                this.AddDtoToTempData(TempDataKeys.TransactionDto, dto);
                return this.RedirectToAction(nameof(this.Index));
            }

            this.AddDtoToTempData(TempDataKeys.TransactionDto, dto);

            return View(dto);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        [Route("Confirmation")]
        public IActionResult Confirmation(TransactionDto dto)
        {
            Result result = new Result();

            var lastValidTransactionState = this.GetDtoFromTempData<TransactionDto>(TempDataKeys.TransactionDto);

            var areEquals = dto.Equals(lastValidTransactionState);

            if (!areEquals)
            {
                result.Messages.Add("Someone try change data with invalid!");

                this.AddDtoToTempData(TempDataKeys.ResultDto, result);
                return this.RedirectToAction(nameof(this.Details));
            }
            
            this.Service = this.ServiceProvider.GetService<AddressService>();

            var publicKey = this.Service.ToPublicKey(dto.PrivateKey);

            var from = dto.Account;
            var to = dto.ReceiverAccount;
            var value = dto.TransferAmount;
            var privateKey = dto.PrivateKey;

            var message = from + to + value;
            var signature = this.Service.SignData(message, privateKey);
            var isSignValid = this.Service.VerifySignature(publicKey, signature, message);

            var response = string.Empty;
            var success = false;

            if (isSignValid)
            {
                var transaction = new Transaction()
                {
                    FromAddress = @from,
                    ToAddress = to,
                    Value = value,
                    SenderSignature = this.Service.ByteToHex(signature),
                    SenderPubKey = this.Service.GetPublicKey(publicKey),
                    DateOfSign = DateTime.UtcNow.ToString("o"),
                    TransactionId = Guid.NewGuid().ToString()
                };

                var httpRequestService = this.ServiceProvider.GetService<IHttpRequestService>();
                var data = JsonConvert.SerializeObject(transaction);

                var nodeData = this.nodeSettings.Value;
                
                foreach (var nodeUrl in nodeData.Url)
                {
                    var fullUrl = nodeUrl + nodeData.Endpoints.PushTransaction;
                    (response, success) = httpRequestService.SendRequest(fullUrl, data, "POST");

                    if (success)
                    {
                        break;                        
                    }
                }
            }

            

            var receeivedOn = DateTime.Now;
            var transactionHash = Guid.NewGuid().ToString();

            result.IsSuccess = success;
            if (success)
            {
                result.Messages.Add($"Successfully make transfer!");
                result.Messages.Add($"Amount: {dto.TransferAmount} coins.");
                result.Messages.Add($"To: {dto.ReceiverAccount}");
                result.Messages.Add($"Received on: {receeivedOn}");
                result.Messages.Add($"Transaction hash: {transactionHash}");
            }
            else
            {
                result.Messages.Add($"Somethings get wrong!");
                result.Messages.Add($"Cannot make transfer!");
            }
            
            this.AddDtoToTempData(TempDataKeys.ResultDto, result);

            return this.RedirectToAction(nameof(this.Details));
        }

        [HttpGet]
        [Route("Details")]
        public IActionResult Details()
        {
            var result = this.GetDtoFromTempData<Result>(TempDataKeys.ResultDto);

            return this.View(result);
        }
    }
}