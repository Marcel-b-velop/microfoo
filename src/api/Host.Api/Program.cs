using Host.Api;
using Host.Register.Adapter;

WebApplicationOptions optionos = new()
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args
};


var builder = WebApplication.CreateBuilder(optionos);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddRegisterAdapter();
builder.Services.AddHostedService<MqttWorker>();

var app = builder.Build();


app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseCors();
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
return 0;