using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DataService.Interfaces;
using Domain.Model;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using OcrService.Interfaces;

namespace ClientApiService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        // GET api/values
        [HttpGet]
        public ActionResult<IEnumerable<string>> Get()
        {
            return new[] { "value1", "value2" };
        }

        // GET api/values/5
        [HttpGet("{id}")]
        public ActionResult<string> Get(int id)
        {
            return "value";
        }

        // POST api/values
        [HttpPost]
        public async Task Post([FromBody] PassportModel value)
        {
            var p = new Passport
            {
                Address = value.BirthPlace,
                Birthday = DateTime.ParseExact(value.BirthDate, "ddMMyyyy", CultureInfo.InvariantCulture),
                Firstname = value.FirstName,
                IssuedBy = value.PlaceIssue,
                IssuedDepartment = value.DepartmentCode,
                IssuedOn = DateTime.ParseExact(value.DateIssue, "ddMMyyyy", CultureInfo.InvariantCulture),
                Lastname = value.LastName,
                Number = value.Number,
                Secondname = value.SecondName,
                Series = value.Series,
                Sex = value.Sex.Contains("муж") ? SexType.Male : SexType.Female
            };

            var client = ServiceProxy.Create<IDataService>(new Uri("fabric:/DataServiceApplication/DataService"));
            await client.SavePassportAsync(p);
        }

        [HttpPost("UploadFiles")]
        public async Task<PassportModel> Post(List<IFormFile> files)
        {
            var result = new PassportModel
            {
                BirthPlace = "error"
            };

            var file = files?.FirstOrDefault();
            if (file == null)
                return result;

            if (file.Length <= 0)
            {
                result.BirthPlace = "error2";
                return result;
            }

            byte[] array;
            using (var stream = new MemoryStream())
            {
                await file.CopyToAsync(stream);
                array = stream.ToArray();
            }

            if (array.Length <= 0)
            {
                result.BirthPlace = "error3";
                return result;
            }

            var client = ServiceProxy.Create<IOcrService>(new Uri("fabric:/DataServiceApplication/OcrService"));
            var p = await client.ParsePassport(array);

            result = new PassportModel
            {
                BirthDate = p.Birthday == DateTime.MinValue ? null : p.Birthday.ToString("ddMMyyyy"),
                BirthPlace = p.Address,
                DateIssue = p.IssuedOn == DateTime.MinValue ? null : p.IssuedOn.ToString("ddMMyyyy"),
                DepartmentCode = p.IssuedDepartment,
                PlaceIssue = p.IssuedBy,
                Series = p.Series,
                Number = p.Number,
                FirstName = p.Firstname,
                LastName = p.Lastname,
                SecondName = p.Secondname,
                Sex = p.Sex == SexType.Male ? "муж." : "жен."
            };

            return result;
        }

        // PUT api/values/5
        [HttpPut("{id}")]
        public void Put(int id, [FromBody] string value)
        {
        }

        // DELETE api/values/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
