using System;
using DataService.Interfaces;
using Domain.Model;
using Microsoft.ServiceFabric.Services.Remoting.Client;

namespace DataService.Client
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            var passport = new Passport
            {
                Address = "Звездная",
                Birthday = new DateTime(1980, 4, 3),
                Firstname = "Дарт",
                IssuedBy = "Космос",
                IssuedDepartment = "Космический Альянс",
                IssuedOn = new DateTime(1980, 4, 3),
                Lastname = "Вейдер",
                Number = "01",
                Secondname = "",
                Series = "01",
                Sex = SexType.Male
            };

            var client = ServiceProxy.Create<IDataService>(new Uri("fabric:/DataServiceApplication/DataService"));
            client.SavePassportAsync(passport).Wait();
        }
    }
}
