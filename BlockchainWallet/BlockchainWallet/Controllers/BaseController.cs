using Newtonsoft.Json;
using Org.BouncyCastle.Asn1.X509.Qualified;

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

        protected T GetDtoFromTempData<T>(string key)
        {
            T dto = default(T);

            if (this.TempData.ContainsKey(key) &&
                this.TempData[key] != null)
            {
                dto = JsonConvert.DeserializeObject<T>(
                    this.TempData[key] as string);

                this.TempData[key] = null;
            }

            return dto;
        }

        protected void AddDtoToTempData<T>(string key, T dto)
        {
            this.TempData[key] = JsonConvert.SerializeObject(dto);
        }
    }
}
