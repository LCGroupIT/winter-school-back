using Domain.Model;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using DataService.Interfaces;

namespace DataService.Client
{
    public class Program
    {
        //Тест. Nunit вылетает, возможно плохо взаимодействует с fabric service
        static void Main(string[] args)
        {
            var passport = new Passport()
            {
                Address = "Звездная",
                Birthday = new DateTime(1264, 4, 3, new GregorianCalendar()),
                Firstname = "Дарт",
                IssuedBy = "Космос",
                IssuedDepartment = "Космический Альянс",
                IssuedOn = new DateTime(1264, 4, 3, new GregorianCalendar()),
                Lastname = "Вейдер",
                Number = "01",
                Secondname = "",
                Series = "01",
                Sex = SexType.Male
            };

            var calculatorClient = ServiceProxy.Create<IDataService>(new Uri("fabric:/DataServiceApplication/DataService"));

            calculatorClient.SavePassportAsync(passport);

            Console.WriteLine("No exceptions");
        }
    }
}
