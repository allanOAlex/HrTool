using Autofac;

namespace GSG.Repository.WebContext
{
    public static class CommonClassExtMethods
    {
        public static ContainerBuilder AddIdentityContext(this ContainerBuilder builder)
        {
            builder.RegisterType<IdentityContext>().As<IIdentityContext>();
            return builder;
        }
    }
}
