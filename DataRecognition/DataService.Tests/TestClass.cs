using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.ServiceFabric.Services.Remoting.Client;
using System.Threading;
using DataService.Interfaces;
using Domain.Model;
using System.Globalization;

namespace DataService.Tests
{
    //По каким то причинам тест вылетает -- возможно дело в unit, а может в vs. Советуют поставить обновления на vs, хотя они уже есть. 
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            System.Diagnostics.Debugger.Launch();
            System.Diagnostics.Debugger.Break();

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

            Assert.Pass();

            
        }
    }
}
