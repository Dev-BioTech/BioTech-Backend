using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace Shared.Infrastructure.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddMicroserviceSwagger(this IServiceCollection services, string title, string version = "v1")
    {
        services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc(version, new OpenApiInfo { Title = title, Version = version });

            // JWT Bearer Security Definition
            c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
            {
                Description = "JWT Authorization header using the Bearer scheme. Example: \"Authorization: Bearer {token}\"",
                Name = "Authorization",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Bearer"
            });

            // Gateway Secret Security Definition
            c.AddSecurityDefinition("Gateway", new OpenApiSecurityScheme
            {
                Description = "Gateway Secret for direct access (X-Gateway-Secret header)",
                Name = "X-Gateway-Secret",
                In = ParameterLocation.Header,
                Type = SecuritySchemeType.ApiKey,
                Scheme = "Gateway"
            });

            c.AddSecurityRequirement(new OpenApiSecurityRequirement
            {
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Bearer"
                        }
                    },
                    Array.Empty<string>()
                },
                {
                    new OpenApiSecurityScheme
                    {
                        Reference = new OpenApiReference
                        {
                            Type = ReferenceType.SecurityScheme,
                            Id = "Gateway"
                        }
                    },
                    Array.Empty<string>()
                }
            });
        });

        return services;
    }
}
