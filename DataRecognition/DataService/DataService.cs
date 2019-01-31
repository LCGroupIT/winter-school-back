using System.Collections.Generic;
using System.Fabric;
using System.Threading.Tasks;
using DataService.Interfaces;
using Domain.Interfaces;
using Domain.Model;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;
using Microsoft.ServiceFabric.Services.Remoting.Runtime;

namespace DataService
{
    public class DataService : StatelessService, IDataService
    {
        private readonly IRepository<Passport> _repository;

        public DataService(StatelessServiceContext context, IRepository<Passport> repository)
            : base(context)
        {
            _repository = repository;
        }

        public Task SavePassportAsync(Passport passport)
        {
            return _repository.CreateAsync(passport);
        }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(this.CreateServiceRemotingListener)
            };
        }
    }
}
