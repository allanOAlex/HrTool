using Autofac;
using Microsoft.Extensions.Configuration;

namespace GSG.Repository.EF;

public class PgModule:Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register<ApplicationContext>(context =>
        {
            var dbConfiguration = context.Resolve<DbConfiguration>();
            var dbContext = new PostgreSqlContext(dbConfiguration.ConnectionString);
            return dbContext;
        }).InstancePerLifetimeScope();
    }
}