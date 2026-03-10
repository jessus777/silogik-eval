using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using SilogikEval.Web.Client.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);

var apiBaseUrl = builder.Configuration["ApiSettings:BaseUrl"] ?? "http://localhost:5079";
builder.Services.AddClientServices(new Uri(apiBaseUrl));

await builder.Build().RunAsync();
