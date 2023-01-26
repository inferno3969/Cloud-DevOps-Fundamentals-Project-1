using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using CloudDevOpsProject1.Client;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.Services.AddScoped<DialogService>();
builder.Services.AddScoped<NotificationService>();
builder.Services.AddScoped<TooltipService>();
builder.Services.AddScoped<ContextMenuService>();
builder.Services.AddTransient(sp => new HttpClient{BaseAddress = new Uri(builder.HostEnvironment.BaseAddress)});
builder.Services.AddScoped<CloudDevOpsProject1.Client.DevOps_Proj_DatabaseService>();
var host = builder.Build();
await host.RunAsync();