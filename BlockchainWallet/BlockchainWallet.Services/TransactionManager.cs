using System;
using System.Collections.Generic;
using System.Text;
using BlockchainWallet.Models.Dto;
using Newtonsoft.Json;

namespace BlockchainWallet.Services
{
    public class TransactionManager : ITransactionManager
    {
        public (string response, bool isSuccess) MakeTransaction(AddressService addressService, IHttpRequestService httpRequestService, NodeData nodeData, TransactionDto dto)
        {
            var response = string.Empty;
            var isSuccess = false;

            if (addressService == null || httpRequestService == null || dto == null)
            {
                return (response, isSuccess);
            }

            // from + to + value
            var message = dto.Account + dto.ReceiverAccount + dto.TransferAmount; 
            
            var publicKey = addressService.ToPublicKey(dto.PrivateKey);

            var signature = addressService.SignData(message, dto.PrivateKey);
            var isSignValid = addressService.VerifySignature(publicKey, signature, message);

            if (!isSignValid)
            {
                return (response, isSuccess);
            }

            var signatureAsString = addressService.ByteToHex(signature);
            var publicKeyAsString = addressService.GetPublicKey(publicKey);

            var transaction = this.CreateTransactionModel(dto, signatureAsString, publicKeyAsString);
            
            (response, isSuccess) = this.SendTransaction(transaction, httpRequestService, nodeData);

            return (response, isSuccess);
        }

        private Transaction CreateTransactionModel(TransactionDto dto, string signature, string publicKey)
        {
            var transaction = new Transaction()
            {
                FromAddress = dto.Account,
                ToAddress = dto.ReceiverAccount,
                Value = dto.TransferAmount,
                SenderSignature = signature, // addressService.ByteToHex(signature),
                SenderPubKey = publicKey, //addressService.GetPublicKey(publicKey),
                DateOfSign = DateTime.UtcNow.ToString("o"),
                TransactionId = Guid.NewGuid().ToString()
            };

            return transaction;
        }

        private (string response, bool isSuccess) SendTransaction(Transaction transaction, IHttpRequestService httpRequestService, NodeData nodeData)
        {
            var response = string.Empty;
            var success = false;
            
            var data = JsonConvert.SerializeObject(transaction);

            foreach (var nodeUrl in nodeData.Url)
            {
                var fullUrl = nodeUrl + nodeData.Endpoints.PushTransaction;
                (response, success) = httpRequestService.SendRequest(fullUrl, data, "POST");

                if (success)
                {
                    break;
                }
            }

            return (response, success);
        }

    }
}
