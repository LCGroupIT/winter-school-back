using System;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using OcrService.Interfaces;

namespace OcrService.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var client = ServiceProxy.Create<IOcrService>(new Uri("fabric:/DataServiceApplication/OcrService"));
            var p = client.ParsePassport(null).Result;

            Console.WriteLine($"Address: {p.Address}");
        }
    }
}
