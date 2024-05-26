using com.b_velop.microfe.connect;
WebApplicationOptions optionos = new()
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args
};
var builder = WebApplication.CreateBuilder(optionos);
builder.Services.AddHostApplication();

var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();