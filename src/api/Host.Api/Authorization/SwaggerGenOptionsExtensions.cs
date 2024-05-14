using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Host.Api.Authorization;

internal static class SwaggerGenOptionsExtensions
{
    public static SwaggerGenOptions AddSecurity(this SwaggerGenOptions swaggerGenOptions,
        string sectionName,
        IConfiguration configuration)
    {
        var authorization = configuration.GetSection(sectionName).Get<AuthorizationSettings>();

        swaggerGenOptions.AddSecurityDefinition(
            JwtBearerDefaults.AuthenticationScheme,
            new OpenApiSecurityScheme
            {
                Description = "Open ID Connect",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.OAuth2,
                Flows = new OpenApiOAuthFlows
                {
                    AuthorizationCode = new OpenApiOAuthFlow
                    {
                        AuthorizationUrl = new Uri($"{authorization!.Authority}/connect/authorize"),
                        TokenUrl = new Uri($"{authorization.Authority}/connect/token"),
                        Scopes = new Dictionary<string, string>
                        {
                                {"openid", "Open ID"},
                                {Scopes.HostApp, "Host Application"}
                        }
                    }
                },
            });
        swaggerGenOptions.AddSecurityRequirement(
            new OpenApiSecurityRequirement
            {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Id = JwtBearerDefaults.AuthenticationScheme,
                                Type = ReferenceType.SecurityScheme
                            }
                        },
                        new string[] { }
                    }
            });

        return swaggerGenOptions;
    }
}
