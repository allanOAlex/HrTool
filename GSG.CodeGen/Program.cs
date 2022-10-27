// See https://aka.ms/new-console-template for more information

using GSG.CodeGen;
using GSG.CodeGen.Gen;
using MediatR;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Scaffolding;
using Microsoft.EntityFrameworkCore.Scaffolding.Internal;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Npgsql.EntityFrameworkCore.PostgreSQL.Design.Internal;
using CSharpEntityTypeGenerator = GSG.CodeGen.Gen.CSharpEntityTypeGenerator;
using CSharpModelGenerator = GSG.CodeGen.Gen.CSharpModelGenerator;
using ReverseEngineerScaffolder = GSG.CodeGen.Gen.ReverseEngineerScaffolder;


var builder = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

var configuration = builder.Build();
ApplicationSettings applicationSettings = configuration.GetSection("ApplicationSettings").Get<ApplicationSettings>();

var services = new ServiceCollection()
    .AddEntityFrameworkDesignTimeServices();
new NpgsqlDesignTimeServices().ConfigureDesignTimeServices(services);

services.AddSingleton<IModelCodeGenerator, CSharpModelGenerator>()   
    .AddSingleton(applicationSettings)
    
    .AddSingleton<ICSharpDbContextGenerator, GSG.CodeGen.Gen.CSharpDbContextGenerator>()
    .AddSingleton<ICSharpEntityTypeGenerator, CSharpEntityTypeGenerator>()
    
    .AddSingleton<IValidationEnityTypeCollectionCodeGenerator, EnityTypeValidationRegistrationCodeGenerator>()
    .AddSingleton<IValidationEnityTypeCodeGenerator, EnityTypeValidationCodeGenerator>()
    .AddSingleton<IReverseEngineerScaffolder,ReverseEngineerScaffolder>()
    .AddSingleton<IComplexReverseEngineerScaffolder, ReverseEngineerScaffolder>()
    .AddMediatR(typeof(Program))
    .AddSingleton<EntityTypeDataStore>();

var serviceProvider = services.BuildServiceProvider();
IReverseEngineerScaffolder createPostgreSqlScaffolder = serviceProvider.GetRequiredService<IReverseEngineerScaffolder>();

IEnumerable<string> tables = new List<string>();
IEnumerable<string> schemas = new List<string>();

var scaffoldedModel = createPostgreSqlScaffolder.ScaffoldModel(
    connectionString: applicationSettings.ConnectionString,
    new DatabaseModelFactoryOptions(tables, schemas),
    new ModelReverseEngineerOptions { UseDatabaseNames = false, NoPluralize = true },
    new ModelCodeGenerationOptions
    {
        UseDataAnnotations = false,
        RootNamespace = applicationSettings.RootNamespace,
        ModelNamespace = applicationSettings.ModelNamespace,
        ContextNamespace = applicationSettings.ContextNamespace,
        Language = "C#", 
        ContextDir = applicationSettings.ContextDir, //,
        ContextName = applicationSettings.ContextName,
        SuppressOnConfiguring = true,
    });

//
createPostgreSqlScaffolder.Save(
    scaffoldedModel,
    outputDir: applicationSettings.OutputDir,
    overwriteFiles: true);