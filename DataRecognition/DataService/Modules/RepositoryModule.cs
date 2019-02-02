using Autofac;
using Domain.Interfaces;
using Domain.Logic;
using Domain.Model;

namespace DataService.Modules
{
    public class RepositoryModule : Module
    {
        protected override void Load(ContainerBuilder builder)
        {
            builder.RegisterType<PassportRepository>()
                .WithParameter("connectionString", "DBConnection")
                .As<IRepository<Passport>>()
                .InstancePerDependency();
        }
    }
}