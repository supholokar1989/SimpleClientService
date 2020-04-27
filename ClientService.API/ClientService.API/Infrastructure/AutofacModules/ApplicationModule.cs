using Autofac;
using System.Reflection;
using ClientService.Data.Queries;


namespace ClientService.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }

        public ApplicationModule(string qconstr)
        {
            QueriesConnectionString = qconstr;

        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(c => new ClientQueries(QueriesConnectionString))
                .As<IClientQueries>()
                .InstancePerLifetimeScope();



        }
    }
}
