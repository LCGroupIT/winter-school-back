using System;
using System.Diagnostics;
using System.Threading;
using Autofac;
using Autofac.Integration.ServiceFabric;
using DataService.Modules;

namespace DataService
{
    internal static class Program
    {
        private static void Main()
        {
            try
            {
                var builder = new ContainerBuilder();
                builder.RegisterModule<RepositoryModule>();
                builder.RegisterServiceFabricSupport();
                builder.RegisterStatelessService<DataService>("DataServiceType");

                using (builder.Build())
                {
                    ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(DataService).Name);

                    Thread.Sleep(Timeout.Infinite);
                }

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
