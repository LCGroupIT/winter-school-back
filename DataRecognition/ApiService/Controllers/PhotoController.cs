using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using OcrService.Interfaces;

namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PhotoController : ControllerBase
    {
        [HttpPost]
        public async Task<UIPassportData> Post([FromBody] IFormFile passportPhoto)
        {
            byte[] outputArray;

            if (passportPhoto == null || passportPhoto.Length == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            using (var stream = new MemoryStream())
            {
                await passportPhoto.CopyToAsync(stream);
                outputArray = stream.ToArray();
            }

            try
            {
                var calculatorClient = ServiceProxy.Create<IOcrService>(new Uri("fabric:/DataServiceApplication/OcrService"));
                var passport = await calculatorClient.ParsePassport(outputArray);
                return new UIPassportData(passport);
            }
            catch (Exception ocrException)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }
    }
}
