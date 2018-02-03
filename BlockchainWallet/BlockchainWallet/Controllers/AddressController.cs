using Microsoft.AspNetCore.Mvc;

namespace BlockchainWallet.Controllers
{
    using System;
    using global::AutoMapper;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Dto;
    using Models.ViewModels;
    using Services;

    [Route("Address")]
    public class AddressController : BaseController
    {
        public AddressController(IServiceProvider serviceProvider) : base(serviceProvider)
        {
        }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            // todo move create to new post
            var service = this.ServiceProvider.GetService<AddressService>();
            var address = service.CreateAddress();
            var model = Mapper.Map<AddressDto, AddressViewModel>(address);
            return View(model);
        }
    }
}