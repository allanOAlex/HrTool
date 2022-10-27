using System;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using DotNet.Testcontainers.Builders;
using DotNet.Testcontainers.Configurations;
using DotNet.Testcontainers.Containers;
using FluentMigrator.Runner;
using GSG.Repository;
using GSG.Repository.EF;
using GSG.Repository.Validation;
using Microsoft.Extensions.DependencyInjection;
using NUnit.Framework;
using GSG.Service;
using Microsoft.Extensions.Configuration;


namespace GSG.Tests
{
    public abstract class DBBaseServiceTest
    {
        private PostgreSqlTestcontainer _dbContainer;
        
        [SetUp]
        public async Task Setup()
        {
            _dbContainer =
            new TestcontainersBuilder<PostgreSqlTestcontainer>()
                .WithDatabase(new PostgreSqlTestcontainerConfiguration
                {
                    Database = "mydb",
                    Username = "test",
                    Password = "password"
                }).Build();
            await _dbContainer.StartAsync();
            var configuration = new ConfigurationBuilder().Build();

            var builder = new ContainerBuilder();

            var serviceCollection = new ServiceCollection().AddSingleton(configuration).AddMigrationServices();
            builder.Register(context =>
            {
                return new DbConfiguration
                {
                    ConnectionString = _dbContainer.ConnectionString
                };
            }).SingleInstance();
            builder.Register<TestIdentityContext>(context => GetTestIdentityContext()).As<IIdentityContext>();

            builder.RegisterModule<DBEFModule>();
            //builder.RegisterModule<InMemoryModule>();
            builder.RegisterModule<PgModule>();
            builder.RegisterModule<ModelValidationModule>();
            builder.AddServices();
            builder.Populate(serviceCollection);
            var container = builder.Build();


            string sourcePath = @"/Users/joelobando/GSGUS/internalhrtool/DBMigration";
            string targetPath = @"Migrations";

            System.IO.Directory.CreateDirectory(targetPath);
            if (System.IO.Directory.Exists(sourcePath))
            {
                string[] files = System.IO.Directory.GetFiles(sourcePath);

                // Copy the files and overwrite destination files if they already exist.
                foreach (string s in files)
                {
                    // Use static Path methods to extract only the file name from the path.
                    var fileName = System.IO.Path.GetFileName(s);
                    var destFile = System.IO.Path.Combine(targetPath, fileName);
                    System.IO.File.Copy(s, destFile, true);
                }
            }

            using (var scope = container.BeginLifetimeScope())
            {
                try
                {
                    var runner = scope.Resolve<IMigrationRunner>();
                    // Execute the migrations
                    runner.MigrateUp();
                }
                catch (Exception ex)
                {
                    var x = ex;
                }
            }


            PostSetup(container);
        }


        protected TestIdentityContext GetTestIdentityContext()
        {
            return new TestIdentityContext(
                userName: "ObandoBoyZ",
                name: "Lastname",
                lastName: "Firstname",
                email: "test2@hotmail.com"
            );
        }

        protected abstract void PostSetup(IContainer container);

        [TearDown]
        public async Task Cleanup()
        {
            await _dbContainer.StopAsync();
            //_container.Dispose();
        }
    }
}