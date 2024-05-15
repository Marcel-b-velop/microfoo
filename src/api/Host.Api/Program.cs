using Host.Api;
using Host.Api.Authorization;
using Host.Application;
using Host.Register.Adapter;
using Microsoft.OpenApi.Models;

const string serviceName = "Host";
const string authorizationSectionName = "Authorization";

WebApplicationOptions optionos = new()
{
    ContentRootPath = AppContext.BaseDirectory,
    Args = args
};


var builder = WebApplication.CreateBuilder(optionos);

var authorizationSettings = AuthorizationSettings.Read(builder.Configuration);

builder.Services.AddControllers();
builder.Services.AddHostApplication();
builder.Services.AddEndpointsApiExplorer()
    .AddSwaggerGen(option => option
        .AddSecurity(authorizationSectionName, builder.Configuration)
        .SwaggerDoc("v1", new OpenApiInfo { Title = "Host", Version = "v1" }));
builder.Services.AddMemoryCache();

builder.Services
    .AddAuthorization(opts =>
        {
            opts.AddPolicy(AccessPolicy.User, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(
                    authorizationSettings.User!,
                    authorizationSettings.Admin!);
                policy.RequireClaim("scope", Scopes.HostApp);
            });
            opts.AddPolicy(AccessPolicy.Admin, policy =>
            {
                policy.RequireAuthenticatedUser();
                policy.RequireRole(authorizationSettings.Admin!);
                policy.RequireClaim("scope", Scopes.HostApp);
            });
        }
    );

builder.Services.AddRegisterAdapter();
builder.Services.AddHostedService<MqttWorker>();

var app = builder.Build();


app.UseDeveloperExceptionPage();
app.UseRouting();
app.UseCors();
app.UseSwagger();
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", $"{serviceName} v1");
    c.OAuthUsePkce();
    c.OAuthClientId("Swagger");
    c.OAuthClientSecret("nichtNotwendig");
});
app.UseStaticFiles();
app.MapControllerRoute(
    name: "default",
    pattern: "{controller}/{action=Index}/{id?}");

app.Run();
return 0;