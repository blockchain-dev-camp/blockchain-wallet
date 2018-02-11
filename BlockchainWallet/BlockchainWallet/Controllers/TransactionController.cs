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
    using System.Linq;

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
                var allErrors = ModelState.Values.SelectMany(v => v.Errors.Select(b => b.ErrorMessage));
                transaction.Message = string.Join(". ", allErrors);
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
            
            var addressService = this.ServiceProvider.GetService<AddressService>();
            var httpRequestService = this.ServiceProvider.GetService<IHttpRequestService>();
            var nodeData = this.nodeSettings.Value;
            var transactionManager = this.ServiceProvider.GetService<ITransactionManager>();

            var response = string.Empty;
            var success = false;

            (response, success) = transactionManager.MakeTransaction(addressService, httpRequestService, nodeData, dto);


            result.IsSuccess = success;
            if (success)
            {
                var responseAsTransaction = JsonConvert.DeserializeObject<Transaction>(response);

                var receivedOn = responseAsTransaction.DateReceived;
                var transactionHash = responseAsTransaction.TransactionHash;

                result.Messages.Add($"Successfully make transfer!");
                result.Messages.Add($"Amount: {dto.TransferAmount} coins.");
                result.Messages.Add($"To: {dto.ReceiverAccount}");
                result.Messages.Add($"Received on: {receivedOn}");
                result.Messages.Add($"Transaction hash/id: {transactionHash}");
            }
            else
            {
                result.Messages.Add($"Transaction failed! Somethings get wrong!");
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