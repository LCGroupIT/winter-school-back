using Autofac;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Model;

namespace DataService
{
    public class RepositoryModule : Module
    {
        //Модуль используется для внедрения зависимостей
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PassportRepository>()
                .WithParameter("connectionString", "DBConnection")
                .As<IRepository<Passport>>()
                .InstancePerDependency();
        }
    }
}
