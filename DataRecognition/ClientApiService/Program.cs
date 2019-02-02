using Microsoft.ServiceFabric.Services.Runtime;
using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace ClientApiService
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

                ServiceRuntime.RegisterServiceAsync("ClientApiServiceType",
                    context => new ClientApiService(context)).GetAwaiter().GetResult();

                ServiceEventSource.Current.ServiceTypeRegistered(Process.GetCurrentProcess().Id, typeof(ClientApiService).Name);

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
