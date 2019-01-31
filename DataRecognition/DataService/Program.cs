using System;
using System.Diagnostics;
using System.Fabric;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Runtime;
using Autofac;
using Autofac.Integration.ServiceFabric;
using Domain.Logic;

namespace DataService
{
    internal static class Program
    {
        /// <summary>
        /// Это точка входа для хост-процесса службы.
        /// </summary>
        private static void Main()
        {
            try
            {
                // В файле ServiceManifest.XML определяется одно имя типа служб или несколько.
                // При регистрации службы имя ее типа сопоставляется с типом .NET.
                // Когда Service Fabric создает экземпляр этого типа службы,
                // в этом хост-процессе создается экземпляр класса.

                //DI - магия
                var builder = new ContainerBuilder();
                builder.RegisterModule<RepositoryModule>();
                builder.RegisterServiceFabricSupport();
                builder.RegisterStatelessService<DataService>("DataServiceType");

                using (builder.Build())
                {
                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(DataService).Name);

                    Thread.Sleep(Timeout.Infinite);
                }
                // Предотвращает завершение работы этого хост-процесса, обеспечивая непрерывную работу служб.
                Thread.Sleep(Timeout.Infinite);
            }
            catch (Exception e)
            {
                ServiceEventSource.Current.ServiceHostInitializationFailed(e.ToString());
                throw;
            }
        }
    }
}
