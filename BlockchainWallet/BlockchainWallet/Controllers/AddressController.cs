using Microsoft.AspNetCore.Mvc;

namespace BlockchainWallet.Controllers
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using System.Linq;
    using global::AutoMapper;
    using Invoicer.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Dto;
    using Models.ViewModels;
    using Services;

    [Route("Address")]
    public class AddressController : BaseController
    {
        public AddressController(IServiceProvider serviceProvider) : base(serviceProvider)
        {

            this.Service = this.ServiceProvider.GetService<AddressService>();
        }

        public AddressService Service { get; set; }

        [HttpGet]
        [Route("")]
        public IActionResult Index()
        {
            return this.View();
        }
        
        [HttpPost]
        [Route("Recover")]
        [ValidateModel("Index")]
        public IActionResult Recover(AddressViewModel model)
        {
            var words = model.Mnemonic.Split(" ", StringSplitOptions.RemoveEmptyEntries);
            if (words.Length != MnemonicWords.RequiredWords || words.Any(w => MnemonicWords.Words.Contains(w) == false))
            {
                this.ModelState.AddModelError("Mnemonic", "Invalid mnemonic words");
                return this.View("Index", model);
            }
            
            var address = this.Service.CreateAddress(model.Mnemonic);
            model = Mapper.Map<AddressDto, AddressViewModel>(address);
            return this.View("Index", model);
        }

        [HttpPost]
        [Route("Create")]
        public IActionResult Create()
        {
            var service = this.ServiceProvider.GetService<AddressService>();
            var address = service.CreateAddress();
            var model = Mapper.Map<AddressDto, AddressViewModel>(address);
            return this.View("Index", model);
        }
    }
}