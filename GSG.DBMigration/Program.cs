// See https://aka.ms/new-console-template for more information

using System.Data;
using System.Diagnostics;
using FluentMigrator.Builders.Execute;
using FluentMigrator.Runner;
using GSG.Repository;
using Microsoft.Extensions.DependencyInjection;


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

var serviceProvider = new ServiceCollection()
    .AddMigrationServices()
    .AddScoped(services =>
    {
        return new DbConfiguration
        {
             ConnectionString = Environment.GetEnvironmentVariable("ConnectionString")
        };
    })
    .BuildServiceProvider(false);;


using (var scope = serviceProvider.CreateScope())
{
    //var logger = scope.ServiceProvider.GetService<ILogger>();
    try
    {
        //logger.LogInformation(company.CompanyName);
        // Instantiate the runner
        var runner = scope.ServiceProvider.GetRequiredService<IMigrationRunner>();
        // Execute the migrations
        runner.MigrateUp();
    }
    catch (Exception ex)
    {
        var x = ex;
    }
}

Console.WriteLine("Hello, World!");