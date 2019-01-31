using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting;
using Microsoft.ServiceFabric.Services;
using Domain.Model;

namespace OcrService.Interfaces
{
    public interface IOcrService : IService
    {
        Task<Passport>  ParsePassport(byte[] passportImage);
    }
}
