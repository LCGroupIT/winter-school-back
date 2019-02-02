using System.Threading.Tasks;
using Domain.Model;
using Microsoft.ServiceFabric.Services.Remoting;

namespace OcrService.Interfaces
{
    public interface IOcrService : IService
    {
        Task<Passport> ParsePassport(byte[] passportImage);
    }
}