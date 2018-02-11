using System;
using System.Collections.Generic;
using System.Text;
using BlockchainWallet.Models.Dto;

namespace BlockchainWallet.Services
{
    public interface ITransactionManager
    {
        (string response, bool isSuccess) MakeTransaction(AddressService addressService,
            IHttpRequestService httpRequestService, NodeData nodeData, TransactionDto dto);
    }
}
