using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using Domain.Model;
using IronOcrWrapper;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;
using OcrService.Interfaces;

namespace OcrService
{
    public class OcrService : StatelessService, IOcrService
    {
        public OcrService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(this.CreateServiceRemotingListener)
            };
        }

        public Task<Passport> ParsePassport(byte[] passportImage)
        {
            var p = OcrHelper.Recognise(passportImage);

            return Task.FromResult(p);
        }
    }
}
