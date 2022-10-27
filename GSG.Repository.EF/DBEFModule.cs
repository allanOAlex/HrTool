using Autofac;
using GSG.Repository.Capability;
using Microsoft.Extensions.Configuration;

namespace GSG.Repository.EF;

public class DBEFModule : Module
{
    protected override void Load(ContainerBuilder builder)
    {
        builder.RegisterGeneric(typeof(EFRepository<>)).As(typeof(IRepository<>));
    }
}