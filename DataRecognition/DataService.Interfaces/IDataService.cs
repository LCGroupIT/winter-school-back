using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain.Model;
using Microsoft.ServiceFabric.Services.Remoting;

namespace DataService.Interfaces
{
    public interface IDataService : IService
    {
        Task SavePassportAsync(Passport passport);
    }
}
