using Domain.Logic;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.Client
{
    class Program
    {
        static void Main(string[] args)
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

            var repository = new PassportRepository("data source = (localdb)\\MSSQLLocalDB; Initial Catalog = PassportStore; Integrated Security = True;");

            repository.CreateAsync(passport).Wait();
        }
    }
}
