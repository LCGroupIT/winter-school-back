using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Client;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using DataService.Interfaces;
using ApiService.Model;
using Domain.Model;
using Newtonsoft.Json;

namespace ApiService.Controllers
{
    class PassportController
    {
        [Route("api/[controller]")]
        public class ProductsController : Controller
        {
            private readonly IDataService _passportService;

            public ProductsController()
            {
                _passportService = ServiceProxy.Create<IDataService>(
                    new Uri("fabric:/DataServiceApplication/DataService"),
                    new ServicePartitionKey(0));
            }

            //[HttpPost]
            //public async Task Post([FromBody] ApiPassport passport)
            //{
            //    var newPassport = JsonConvert.DeserializeObject<Passport>(passport);

            //    await _passportService.SavePassportAsync(newPassport);
            //}
        }
    }
}
