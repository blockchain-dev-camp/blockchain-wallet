namespace BlockchainWallet.Controllers
{
    using System;
    using Microsoft.AspNetCore.Mvc;

    public class BaseController : Controller
    {
        public BaseController(IServiceProvider serviceProvider)
        {
            this.ServiceProvider = serviceProvider;
        }

        public IServiceProvider ServiceProvider { get; set; }
    }
}
