using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;


namespace ApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PassportController : ControllerBase
    {
        // POST api/values
        [HttpPost]
        public async  Post([FromBody] IFormFile passportPhoto)
        {
            if (passportPhoto == null || passportPhoto.Length == 0)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }

            byte[] outputArray;
            using (var stream = new MemoryStream())
            {
                await passportPhoto.CopyToAsync(stream);
                outputArray = stream.ToArray();
            }

            try
            {
                var ocrService = ServiceProxy.Create<IOcrService>(new Uri("fabric:/DataServiceApplication/OcrService"));
                var passport = await ocrService.ParsePassport(outputArray);
                Response.StatusCode = (int)HttpStatusCode.OK;
            }
            catch (Exception exception)
            {
                Response.StatusCode = (int)HttpStatusCode.BadRequest;
            }
        }
    }
}
