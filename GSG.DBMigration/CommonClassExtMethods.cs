using FluentMigrator.Infrastructure;
using FluentMigrator.Runner;
using FluentMigrator.Runner.Initialization;
using GSG.DBMigration.Migrations;
using GSG.Repository;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;

public static class CommonClassExtMethods
{
    public static IServiceCollection AddMigrationServices(this IServiceCollection builder)
    {
        return builder
            // Add common FluentMigrator services
            .AddFluentMigratorCore()
            .Configure<AssemblySourceOptions>(
                x =>
                    x.AssemblyNames = new[] { typeof(Init0000000000).Assembly.GetName().Name })
            .ConfigureRunner(
                rb => rb
                    // Add PostgresL Server    support to FluentMigrator
                    .AddPostgres()
                    // Set the connection string
                    .WithGlobalConnectionString(context =>
                    {
                        DbConfiguration config = context.GetService(typeof(DbConfiguration)) as DbConfiguration;
                        return config.ConnectionString;
                    })
                    // Define the assembly containing the migrations
                    .ScanIn(typeof(Init0000000000).Assembly).For.Migrations()
                    .For.EmbeddedResources())
            // Enable logging to console in the FluentMigrator way
            .AddLogging(lb => lb.AddFluentMigratorConsole())
            // Build the service provider
            // Add the default embedded resource provider
            .RemoveAll<IEmbeddedResourceProvider>()
            .AddSingleton<IEmbeddedResourceProvider>(
                sp => new ExtDefaultEmbeddedResourceProvider(sp.GetRequiredService<IAssemblySource>().Assemblies));
    }
}