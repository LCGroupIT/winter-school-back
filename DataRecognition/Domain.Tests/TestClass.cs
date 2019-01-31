using Domain.Interfaces;
using Domain.Logic;
using Domain.Model;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Tests
{
    public class PassportMock : IRepository<Passport>
    {
        public List<Passport> _passports;

        public Task CreateAsync(Passport passport)
        {
            return Task.Run(() => _passports.Add(passport));
        }
    }

    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
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

            var mock = new PassportMock();
            mock.CreateAsync(passport).Wait();

            Assert.AreEqual(mock._passports.FirstOrDefault(), passport);
        }
    }
}
