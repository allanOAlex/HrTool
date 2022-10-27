using Autofac;
using Autofac.Extensions.DependencyInjection;
using GSG.Service;
using AutoMapper.Contrib.Autofac.DependencyInjection;
using GSG.API.Middleware;
using GSG.Logging;
using GSG.Repository;
using GSG.Repository.EF;
using GSG.Repository.Profiles;
using GSG.Repository.Validation;
using GSG.Repository.WebContext;
using Microsoft.Identity.Web;
using Microsoft.Identity.Web.Resource;
using Microsoft.OpenApi.Models;

var configuration = new ConfigurationBuilder()
    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
    .Build();

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddHttpContextAccessor(); //Adds http context
builder.Services.RegisterLogger(null);
// Add services to the container.
//
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());
// Call ConfigureContainer on the Host sub property 
builder.Host.ConfigureContainer<ContainerBuilder>(builder =>
{
    builder.Register(context =>
    {
        IConfiguration configuration = context.Resolve<IConfiguration>();
        var connectionString = configuration.GetConnectionString("default");
        return new DbConfiguration
        {
            ConnectionString = connectionString
        };
    }).SingleInstance();
    builder.AddIdentityContext();
    builder.RegisterModule<DBEFModule>();
    builder.RegisterModule<PgModule>();
    builder.RegisterModule<ModelValidationModule>();
    builder.AddServices();
    builder.RegisterAutoMapper(context => { context.AddProfile<BOToDTOProfile>(); });
});

//
builder.Services.AddControllers().ConfigureApiBehaviorOptions(options =>
{
    options.SuppressModelStateInvalidFilter = true;
});
;
// Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddMicrosoftIdentityWebApiAuthentication(builder.Configuration, "AzureAd");
builder.Services.AddAuthorization();
builder.Services.AddSwaggerGen(c =>
{
    // Enabled OAuth security in Swagger
    //var scopes = JosephGuadagno.Broadcasting.Domain.Scopes.ToDictionary(settings.ApiScopeUrl);
    //scopes.Add($"{settings.ApiScopeUrl}user_impersonation", "Access application on user behalf");
    c.AddSecurityRequirement(new OpenApiSecurityRequirement()
    {
        {
            new OpenApiSecurityScheme
            {
                Reference = new OpenApiReference
                {
                    Type = ReferenceType.SecurityScheme,
                    Id = "oauth2"
                },
                Scheme = "oauth2",
                Name = "oauth2",
                In = ParameterLocation.Header
            },
            new List<string>()
        }
    });
    c.AddSecurityDefinition("oauth2", new OpenApiSecurityScheme
    {
        Type = SecuritySchemeType.OAuth2,
        Flows = new OpenApiOAuthFlows
        {
            Implicit = new OpenApiOAuthFlow()
            {
                AuthorizationUrl =
                    new Uri(
                        "https://login.microsoftonline.com/1c1b6373-b1bc-4db7-90e4-1ef2b82273e0/oauth2/v2.0/authorize"), //Directory (tenant) ID
                TokenUrl = new Uri(
                    "https://login.microsoftonline.com/1c1b6373-b1bc-4db7-90e4-1ef2b82273e0/oauth2/v2.0/token"), //Directory (tenant) ID
                Scopes = new Dictionary<string, string>
                {
                    {
                        "api://5944e24c-9976-4746-ab74-53bf2c530ebd/Read.Report",
                        "Reads the Weather forecast"
                    }
                }
            }
        }
    });
});

var app = builder.Build();
app.UseMiddleware<LoggingMiddleware>();
app.UseMiddleware<ErrorHandlerMiddleware>();
// Configure the HTTP request pipeline.

app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "AzureAD_OAuth_API v1");
    //c.RoutePrefix = string.Empty;    
    c.OAuthClientId("Client Id");
    c.OAuthClientSecret("Client Secret Key");
    c.OAuthUseBasicAuthenticationWithAccessCodeGrant();
});


app.UseAuthentication();

app.UseHttpsRedirection();

app.UseAuthorization();

// global error handler
app.UseMiddleware<LoggingUserMiddleware>();

app.MapControllers();

app.Run();