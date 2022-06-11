using ExampleBlazorApp.Client;
using ExampleBlazorApp.Client.HttpClients;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient(Options.DefaultName, httpClient => httpClient.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddTypedClient<WeatherForecastClient>()
    .AddTypedClient<WeatherProfileClient>();

await builder.Build().RunAsync();
