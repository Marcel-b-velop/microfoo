using com.b_velop.microfe.connect;
using com.b_velop.microfe.shared.Client;
using com.b_velop.microfe.shared.Models;
using com.b_velop.microfe.shared.workers;

WebApplicationOptions optionos = new()
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args
};

var builder = WebApplication.CreateBuilder(optionos);

builder.Services.AddHttpClient<IHostClient, HostClient>(rg => rg.BaseAddress = new Uri("http://localhost:8000"));
builder.Services.AddHostedService<RegisterApplicationWorker>();
builder.Services.AddHostedService<ApplicationAliveWorker>();
builder.Services.Configure<ApplicationConfiguration>(builder.Configuration.GetSection("ApplicationConfiguration"));
builder.Services.AddHostApplication();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();