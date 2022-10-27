using Autofac;
using Autofac.Extensions.DependencyInjection;
using GSG.Repository;
using GSG.Repository.EF;
using GSG.Repository.Validation;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using GSG.Service;
using Microsoft.Extensions.Configuration;


namespace GSG.Tests
{
    public abstract class BaseServiceTest
    {
        IContainer _container;

        [SetUp]
        public void Setup()
        {
            var configuration = new ConfigurationBuilder().Build();

            var builder = new ContainerBuilder();

            var serviceCollection = new ServiceCollection().AddSingleton(configuration);

            builder.Register<TestIdentityContext>(context => GetTestIdentityContext()).As<IIdentityContext>();

            builder.RegisterModule<DBEFModule>();
            builder.RegisterModule<InMemoryModule>();
            builder.RegisterModule<ModelValidationModule>();
            builder.AddServices();
            builder.Populate(serviceCollection);
            _container = builder.Build();
            PostSetup(_container);
        }


        protected TestIdentityContext GetTestIdentityContext()
        {
            return new TestIdentityContext(
                userName : "ObandoBoyZ",
                name: "Lastname",
                lastName : "Firstname",
                email : "test2@hotmail.com"
            );
        }

        protected abstract void PostSetup(IContainer container);

        [TearDown]
        public void Cleanup()
        {
            _container.Dispose();
        }
    }
}