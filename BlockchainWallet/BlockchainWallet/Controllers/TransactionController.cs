using BlockchainWallet.Models.Dto;
using BlockchainWallet.Utils.Globals;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace BlockchainWallet.Controllers
{
    using System;

    [Route("Transaction")]
    public class TransactionController : BaseController
    {
        public TransactionController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

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

            //todo create transaction ...


            var receeivedOn = DateTime.Now;
            var transactionHash = Guid.NewGuid().ToString();

            result.IsSuccess = true;
            result.Messages.Add($"Successfully make transfer!");
            result.Messages.Add($"Amount: {dto.TransferAmount} coins.");
            result.Messages.Add($"To: {dto.ReceiverAccount}");
            result.Messages.Add($"Received on: {receeivedOn}");
            result.Messages.Add($"Transaction hash: {transactionHash}");

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