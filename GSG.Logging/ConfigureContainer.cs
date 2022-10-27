using Autofac;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using Serilog;
using Microsoft.Extensions.DependencyInjection;
using Serilog.Configuration;
using Serilog.Core;
using Serilog.Events;

namespace GSG.Logging;

public static class DelimitedEnrichersExtensions
{
    public const string Delimiter = " | ";


    public static LoggerConfiguration WithPropertyDelimited(this LoggerEnrichmentConfiguration enrichmentConfiguration,
        string propertyName)
    {
        return enrichmentConfiguration.With(new DelimitedEnricher(propertyName, Delimiter));
    }
}

public class DelimitedEnricher : ILogEventEnricher
{
    private readonly ILogEventEnricher innerEnricher;
    private readonly string innerPropertyName;
    private readonly string delimiter;

    public DelimitedEnricher(string innerPropertyName, string delimiter)
    {
        this.innerPropertyName = innerPropertyName;
        this.delimiter = delimiter;
    }

    public DelimitedEnricher(ILogEventEnricher innerEnricher, string innerPropertyName, string delimiter) : this(
        innerPropertyName, delimiter)
    {
        this.innerEnricher = innerEnricher;
    }

    public void Enrich(LogEvent logEvent, ILogEventPropertyFactory propertyFactory)
    {
        innerEnricher?.Enrich(logEvent, propertyFactory);

        LogEventPropertyValue eventPropertyValue;
        if (logEvent.Properties.TryGetValue(innerPropertyName, out eventPropertyValue))
        {
            var value = (eventPropertyValue as ScalarValue)?.Value.ToString();
            if (!String.IsNullOrEmpty(value))
            {
                logEvent.AddPropertyIfAbsent(new LogEventProperty(innerPropertyName + "Delimited",
                    new ScalarValue(value + delimiter)));
            }
        }
    }
}

public static class ConfigureContainer
{
    public static IServiceCollection RegisterLogger(this IServiceCollection container, IConfiguration configuration)
    {
        string outputTemplate =
            "[{Timestamp:HH:mm:ss} {Level:u3}]  {GuidDelimited}{UserIdDelimited}{Message:lj}{NewLine}{Exception}";

        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
            .Enrich.WithPropertyDelimited("Guid")
            .Enrich.WithPropertyDelimited("UserId")
            .Enrich.WithPropertyDelimited("Url")
            //.Enrich.WithPropertyDelimited("PathUrl")
            //   .ReadFrom.Configuration(configuration)
            .WriteTo.Console(outputTemplate: outputTemplate)
            .CreateLogger();

        return container.AddLogging(opt =>
        {
            opt.ClearProviders();
            opt.AddSerilog();
        });
    }
}

//The log needs to be able to track a correlation id 
//it also needs to track who is making the request 
//as well as how log it took to complete the request 