using System;
using System.Collections.Generic;
using System.Fabric;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using DataService.Interfaces;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Model;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace DataService
{
    /// <summary>
    /// Среда выполнения Service Fabric создает экземпляр этого класса для каждого экземпляра службы.
    /// </summary>
    public class DataService : StatelessService, IDataService
    {
        //Для DI подключен autofac
        private IRepository<Passport> _repository;

        public DataService(StatelessServiceContext context, IRepository<Passport> repository)
            : base(context)
        {
            _repository = repository;
        }

        public Task SavePassportAsync(Passport passport)
        {
            _repository.CreateAsync(passport);
            _repository.SaveAsync();
            return Task.FromResult(true);
        }

        /// <summary>
        /// Необязательное переопределение для создания прослушивателей (например, TCP, HTTP) для этой реплики службы, чтобы обрабатывать запросы клиентов или пользователей.
        /// </summary>
        /// <returns>Коллекция прослушивателей.</returns>
        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new ServiceInstanceListener[0];
        }
    }
}
