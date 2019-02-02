using System.Collections.Generic;
using System.Fabric;
using System.IO;
using System.Net;
using System.Net.Http;
using System.Security.Cryptography.X509Certificates;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.ServiceFabric.Services.Communication.AspNetCore;
using Microsoft.ServiceFabric.Services.Communication.Runtime;
using Microsoft.ServiceFabric.Services.Runtime;

namespace ClientApiService
{
    internal sealed class ClientApiService : StatelessService
    {
        public ClientApiService(StatelessServiceContext context)
            : base(context)
        { }

        protected override IEnumerable<ServiceInstanceListener> CreateServiceInstanceListeners()
        {
            return new[]
            {
                new ServiceInstanceListener(serviceContext =>
                    new KestrelCommunicationListener(serviceContext, "ServiceEndpoint", (url, listener) =>
                    {
                        ServiceEventSource.Current.ServiceMessage(serviceContext, $"Starting Kestrel on {url}");

                        return new WebHostBuilder()
                            //.UseKestrel()
                            //.ConfigureServices(
                            //    services => services
                            //        .AddSingleton(serviceContext))
                            .UseKestrel(opt =>
                            {
                                var port = serviceContext.CodePackageActivationContext.GetEndpoint("ServiceEndpoint")
                                    .Port;
                                opt.Listen(IPAddress.IPv6Any, port, listenOptions =>
                                {
                                    listenOptions.UseHttps(GetCertificateFromStore());
                                    listenOptions.NoDelay = true;
                                });
                            })
                            .ConfigureAppConfiguration((builderContext, config) =>
                            {
                                config.AddJsonFile("appsettings.json", false, true);
                            })
                            .ConfigureServices(
                                services => services
                                    .AddSingleton(new HttpClient())
                                    .AddSingleton(new FabricClient())
                                    .AddSingleton(serviceContext))
                            .UseContentRoot(Directory.GetCurrentDirectory())
                            .UseStartup<Startup>()
                            .UseServiceFabricIntegration(listener, ServiceFabricIntegrationOptions.None)
                            .UseUrls(url)
                            .Build();
                    }))
            };
        }

        private static X509Certificate2 GetCertificateFromStore()
        {
            var store = new X509Store(StoreName.My, StoreLocation.LocalMachine);
            try
            {
                store.Open(OpenFlags.ReadOnly);
                var certCollection = store.Certificates;
                var currentCerts = certCollection.Find(X509FindType.FindBySubjectDistinguishedName, "CN=schooltestcert", false);
                return currentCerts.Count == 0 ? null : currentCerts[0];
            }
            finally
            {
                store.Close();
            }
        }}
}
