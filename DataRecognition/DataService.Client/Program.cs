using Domain.Model;
using System;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using DataService.Interfaces;

namespace DataService.Client
{
    public class Program
    {
        //Тест. Nunit вылетает, возможно плохо взаимодействует с fabric service
        private static void Main(string[] args)
        {
            var passport = new Passport()
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

            var calculatorClient = ServiceProxy.Create<IDataService>(new Uri("fabric:/DataServiceApplication/DataService"));

            calculatorClient.SavePassportAsync(passport).Wait();

            Console.WriteLine("No exceptions");
        }
    }
}
