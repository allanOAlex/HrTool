using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using GSG.WebApp.Client;
using BlazorStrap;
using Radzen;
using Blazored.Modal;
using GSG.WebApp.Client.Services.EmployeeService;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("GSG.WebApp.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("GSG.WebApp.ServerAPI"));

builder.Services.AddScoped<IEmployeeService, EmployeeService>();

builder.Services.AddMsalAuthentication(options =>
{
    builder.Configuration.Bind("AzureAd", options.ProviderOptions.Authentication);
    options.ProviderOptions.DefaultAccessTokenScopes.Add("api://5944e24c-9976-4746-ab74-53bf2c530ebd/Read.Report");
    options.ProviderOptions.LoginMode = "Redirect";
});

builder.Services.AddBlazorStrap();
builder.Services.AddScoped<DialogService>();
builder.Services.AddBlazoredModal();
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();


await builder.Build().RunAsync();
