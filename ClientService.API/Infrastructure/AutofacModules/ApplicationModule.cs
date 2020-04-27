using Autofac;
using System.Reflection;
using ClientService.Data.Queries;


namespace ClientService.API.Infrastructure.AutofacModules
{
    public class ApplicationModule : Autofac.Module
    {
        public string QueriesConnectionString { get; }
        public string ClientCache { get; }

        public ApplicationModule(string qconstr, string cacheConStr)
        {
            QueriesConnectionString = qconstr;
            ClientCache = cacheConStr;

        }

        protected override void Load(ContainerBuilder builder)
        {

            builder.Register(c => new ClientQueries(QueriesConnectionString, ClientCache))
                .As<IClientQueries>()
                .InstancePerLifetimeScope();



        }
    }
}
