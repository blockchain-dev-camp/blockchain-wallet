using Microsoft.AspNetCore.Mvc;

namespace BlockchainWallet.Controllers
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using global::AutoMapper;
    using Invoicer.Filters;
    using Microsoft.Extensions.DependencyInjection;
    using Models.Domain;
    using Models.Dto;
    using Models.ViewModels;
    using Newtonsoft.Json;
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
            if (words.Length != MnemonicWords.RequiredWords || MnemonicWords.Words.Intersect(words).Count() != MnemonicWords.RequiredWords)
            {
                this.ModelState.AddModelError("Mnemonic", "Invalid mnemonic words");
                return this.View("Index", model);
            }
            
            var address = this.Service.CreateAddress(model.Mnemonic);
            model = Mapper.Map<AddressDto, AddressViewModel>(address);
            return this.View("Index", model);
        }

        [HttpGet]
        [Route("Create")]
        public IActionResult Create()
        {
            return this.View();
        }

        [HttpPost]
        [Route("Create")]
        [ValidateModel]
        public IActionResult Create(string mnemonicData)
        {
            var data = JsonConvert.DeserializeObject<List<Entropy>>(mnemonicData);
            var mnemonicService = this.ServiceProvider.GetService<MnemonicService>();
            string mnemonics = mnemonicService.GetMnemonics(data);
            var addressService = this.ServiceProvider.GetService<AddressService>();
            var address = addressService.CreateAddress(mnemonics);
            var model = Mapper.Map<AddressDto, AddressViewModel>(address);
            return this.View("Index", model);
        }
    }
}