﻿using Domain.Logic;
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
    [TestFixture]
    public class TestClass
    {
        [Test]
        public void TestMethod()
        {
            var passport = new Passport()
            {
                Address = "Звездная",
                Birthday = new DateTime(1980, 4, 3, new GregorianCalendar()),
                Firstname = "Дарт",
                IssuedBy = "Космос",
                IssuedDepartment = "Космический Альянс",
                IssuedOn = new DateTime(1980, 4, 3, new GregorianCalendar()),
                Lastname = "Вейдер",
                Number = "01",
                Secondname = "",
                Series = "01",
                Sex = SexType.Male
            };

            var repository = new PassportRepository(ConfigurationManager.AppSettings["DBConnection"]);

            repository.CreateAsync(passport);
            repository.SaveAsync();
        }
    }
}
