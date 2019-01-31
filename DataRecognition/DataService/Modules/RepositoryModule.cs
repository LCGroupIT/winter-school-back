using Autofac;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataService
{
    public class RepositoryModule : Module
    {
        //Модуль используется для внедрения зависимостей
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType(typeof(PassportRepository))
                .As(typeof(IRepository<Passport>))
                .WithParameter("connectionString", ConfigurationManager.AppSettings["DBConnection"])
                .InstancePerLifetimeScope();
        }
    }
}
