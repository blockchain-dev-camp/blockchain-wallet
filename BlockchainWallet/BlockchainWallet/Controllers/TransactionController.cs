using Microsoft.AspNetCore.Mvc;

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
            return View();
        }
    }
}