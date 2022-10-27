// See https://aka.ms/new-console-template for more information

using Autofac;
using Autofac.Extensions.DependencyInjection;
using GSG.Logging;
using GSG.Model;
using GSG.Repository.Capability;
using GSG.Repository.EF;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var builder = new ContainerBuilder();

var serviceCollection = new ServiceCollection()
    .RegisterLogger(configuration)
    .AddSingleton(configuration);

builder.Populate(serviceCollection);
builder.RegisterModule<DBEFModule>();
builder.RegisterModule<PgModule>();
var container = builder.Build();

ILogger logger = container.Resolve<ILogger<Program>>();
logger.LogError("this is an error");

var repository = container.Resolve<IRepository<Skill>>();
repository.Insert(new Skill() { SkillName = "test", CreatedBy = "me"});

var x = repository.GetAll().ToList();

Console.WriteLine("Hello, World!");