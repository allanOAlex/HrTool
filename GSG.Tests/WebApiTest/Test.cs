using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Encodings.Web;
using System.Threading.Tasks;
using Autofac;
using Autofac.Extensions.DependencyInjection;
using GSG.Model;
using GSG.Model.DTO.Responses;
using GSG.Repository.Capability;
using GSG.Repository.EF;
using GSG.Repository.Validation;
using GSG.Service;
using GSG.Shared;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using NUnit.Framework;

namespace GSG.Tests.WebApiTest;

internal class IntegrationTestAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
{
    public IntegrationTestAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
        ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock)
        : base(options, logger, encoder, clock)
    {
    }

    protected override Task<AuthenticateResult> HandleAuthenticateAsync()
    {
        var claims = new[]
        {
            new Claim(ClaimTypes.Surname, "TestSurname"),
            new Claim(ClaimTypes.GivenName, "TestName"),
            new Claim(ClaimTypes.Name, "TestFullName"),
            new Claim(ClaimTypes.Upn, "test@test.com"),
            new Claim(ClaimTypes.Role, "Manager")
        };

        var identity = new ClaimsIdentity(claims, "IntegrationTest");
        var principal = new ClaimsPrincipal(identity);
        var ticket = new AuthenticationTicket(principal, "IntegrationTest");
        var result = AuthenticateResult.Success(ticket);
        return Task.FromResult(result);
    }
}

public class CustomWebApplicationFactory<T, I> : WebApplicationFactory<T> where T : class where I : AuthenticationHandler<AuthenticationSchemeOptions>
{
    protected override IHost CreateHost(IHostBuilder builder)
    {
        builder.ConfigureServices(services =>
        {
            services
                .AddAuthentication("IntegrationTest")
                .AddScheme<AuthenticationSchemeOptions, I>(
                    "IntegrationTest",
                    options => { }
                );
        });

        builder.ConfigureContainer<ContainerBuilder>(
            (_, builder) =>
            {
                var configuration = new ConfigurationBuilder().Build();
                var serviceCollection = new ServiceCollection().AddSingleton(configuration);

                builder.RegisterModule<DBEFModule>();
                builder.RegisterModule<InMemoryModule>();
                builder.RegisterModule<ModelValidationModule>();
                builder.AddServices();
                builder.Populate(serviceCollection);
            });
        return base.CreateHost(builder);
    }
}

public class Test
{
    [SetUp]
    public void Setup()
    {
    }

    [TearDown]
    public void Cleanup()
    {
    }


    [Test]
    public async Task Testing([Values(
            "api/certificate?CertificateId=1",
            "api/certificate?CertificateName=test",
            "api/certificate")]
        string url)
    {
        var application = new CustomWebApplicationFactory<Program, IntegrationTestAuthenticationHandler>();
        IRepository<Certificate> certificateRepository = application.Services.GetService<IRepository<Certificate>>();
        certificateRepository.Insert(new Certificate { CertificateName = "test", CreatedBy = "test6" });

        ResponseBody<IEnumerable<CertificateResponse>> responseBody = await NewMethod<IEnumerable<CertificateResponse>>(url, application);
        Assert.AreEqual(1, responseBody.Body.Count());
    }
    
    [Test]
    public async Task Testing2([Values(
            "api/certificate?CertificateId=2",
            "api/certificate?CertificateName=test2"
           )]
        string url)
    {
        var application = new CustomWebApplicationFactory<Program, IntegrationTestAuthenticationHandler>();
        IRepository<Certificate> certificateRepository = application.Services.GetService<IRepository<Certificate>>();
        certificateRepository.Insert(new Certificate { CertificateName = "test", CreatedBy = "test6" });

        ResponseBody<IEnumerable<CertificateResponse>> responseBody = await NewMethod<IEnumerable<CertificateResponse>>(url, application);
        Assert.AreEqual(0, responseBody.Body.Count());
    }

    private static async Task<ResponseBody<T>> NewMethod<T>(string url, CustomWebApplicationFactory<Program, IntegrationTestAuthenticationHandler> application)
    {
        var client = application.CreateClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        ResponseBody<T> responseBody = JsonConvert.DeserializeObject<ResponseBody<T>>(result);
        return responseBody;
    }

    [Test]
    public async Task TestingId([Values("api/certificate/1")] string url, [Values(1)] int id)
    {
        DateOnly testDateOnly = DateOnly.FromDateTime(DateTime.Now);

        var application = new CustomWebApplicationFactory<Program, IntegrationTestAuthenticationHandler>();

        IRepository<Certificate> certificateRepository = application.Services.GetService<IRepository<Certificate>>();

        certificateRepository.Insert(new Certificate { CertificateName = "test", CreatedBy = "test6" });

        var client = application.CreateClient();
        var response = await client.GetAsync(url);
        response.EnsureSuccessStatusCode();
        var result = await response.Content.ReadAsStringAsync();
        var responseBody = JsonConvert.DeserializeObject<ResponseBody<CertificateResponse>>(result);
        Assert.AreEqual(id, responseBody.Body.CertificateId);
    }
}