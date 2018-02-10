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

            //todo create transaction ...

            //var from = "alex";
            //var to = "pesho";
            //var value = "5";

            //var privateKey = "8dba174ee985a494cc913512f68a2bff4b9bc3214ae26c3133ef1065e9ba655e";
            //var publicKey = "2d5367e954d14d2cb889860b9e80ef5e047e4cab7ec570c7ef7656e0e12e2db61";

            #region NotUSed
            /*
            var cm = new CryptoManager();
            var fromByte = cm.GetBytes(from);
            var toByte = cm.GetBytes(to);
            var valBytes = cm.GetBytes(value);

            var fromToBytes = cm.MergeArrays(fromByte, toByte);
            var ftv = cm.MergeArrays(fromToBytes, valBytes);

            var pubKeyBytes = cm.GetBytes(publicKey);
            var publicBytes = cm.MergeArrays(ftv, pubKeyBytes);

            var priKeyBytes = cm.GetBytes(privateKey);
            var privateBytes = cm.MergeArrays(ftv, priKeyBytes);
            */

            /*
            //var a = ECDsa.Create();
            //var pub = a.SignHash(publicBytes);
            //var pri = a.SignData(privateBytes, HashAlgorithmName.SHA256);

            //var aaa = a.VerifyData(privateBytes, pri, HashAlgorithmName.SHA256);

            //var ind0 = pub[0].Equals(pri[0]);
            //var ind1 = pub[1].Equals(pri[1]);
            */

            #endregion

            this.Service = this.ServiceProvider.GetService<AddressService>();

            var publicKey = this.Service.GetPublicKey(dto.PrivateKey);

            var from = dto.Account;
            var to = dto.ReceiverAccount;
            var value = dto.TransferAmount;
            var privateKey = dto.PrivateKey;

            var signature = this.Test(from + to + value, privateKey);
            var isSignValid = this.VerifyMessage(from + to + value, signature, publicKey);

            bool test = true;

            Transaction transaction = null;
            var response = string.Empty;
            var success = false;

            if (isSignValid)
            {
                transaction = new Transaction()
                {
                    From = from,
                    To = to,
                    Value = value,
                    SenderSignature = signature,
                    SenderPubKey = publicKey
                };

                var httpRequestService = this.ServiceProvider.GetService<IHttpRequestService>();
                var data = JsonConvert.SerializeObject(transaction);

                var nodeData = this.nodeSettings.Value;
                
                foreach (var nodeUrl in nodeData.Url)
                {

                    (response, success) = httpRequestService.SendRequest(nodeUrl, data, "POST");

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

        public string Test(string data, string privateKey)
        {
            

            string signedMessage;
            try
            {

                //Initiate a new instanse with 2048 bit key size
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);


                // Load private key
                rsa.FromXmlString(privateKey);
                


                //rsa.SignData( buffer, hash algorithm) - For signed data. Here I used SHA512 for hash. 
                //Encoding.UTF8.GetBytes(string) - convert string to byte messafe 
                //Convert.ToBase64String(string) - convert back to a string.
                signedMessage = Convert.ToBase64String(rsa.SignData(Encoding.UTF8.GetBytes(data), CryptoConfig.MapNameToOID("SHA256")));
                
            }
            catch (Exception e)
            {
                signedMessage = string.Empty;
            }

            return signedMessage;
        }

        private bool VerifyMessage(string originalMessage, string signedMessage, string publicKey)
        {
            bool verified;
            try
            {
                RSACryptoServiceProvider rsa = new RSACryptoServiceProvider(2048);
                rsa.FromXmlString(publicKey);
                // load public key 
                verified = rsa.VerifyData(Encoding.UTF8.GetBytes(originalMessage), CryptoConfig.MapNameToOID("SHA512"), Convert.FromBase64String(signedMessage));
            }
            catch (Exception)
            {
                verified = false;
            }

            return verified;
        }
    }
}