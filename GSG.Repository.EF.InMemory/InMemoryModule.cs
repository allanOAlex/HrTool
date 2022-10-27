using Autofac;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace GSG.Repository.EF;

public class InMemoryModule:Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.Register<ApplicationContext>(context =>
        {
            var configurationRoot = context.Resolve<IConfigurationRoot>();
            var dbContext = new InMemoryContext(
                $"Test-{Guid.NewGuid()}"
                );
            return dbContext;
        }).SingleInstance();
    }
}