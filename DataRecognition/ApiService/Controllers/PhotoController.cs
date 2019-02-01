using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading.Tasks;
using DataService.Interfaces;
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
        [HttpPost("image")]
        public async Task<UIPassportData> Post(IFormFile passportPhoto)
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
                var ocrClient = ServiceProxy.Create<IOcrService>(new Uri("fabric:/DataServiceApplication/OcrService"));
                var passport = await ocrClient.ParsePassport(outputArray);
                return new UIPassportData(passport);
            }
            catch (Exception ocrException)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
                return null;
            }
        }

        [HttpPost("passport")]
        public async void Post(UIPassportData passport)
        {
            try
            {
                var dataBaseClient = ServiceProxy.Create<IDataService>(new Uri("fabric:/DataServiceApplication/DataService"));
                await dataBaseClient.SavePassportAsync(passport.ParseToDatabasePassport());
            }
            catch (Exception dataBaseException)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
