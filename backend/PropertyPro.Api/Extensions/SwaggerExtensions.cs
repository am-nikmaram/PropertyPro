using Microsoft.OpenApi;
using Swashbuckle.AspNetCore.SwaggerGen;

namespace PropertyPro.Api.Extensions;

public static class SwaggerExtensions
{
    public static IServiceCollection AddPropertyProSwagger(
        this IServiceCollection services)
    {
        services.AddEndpointsApiExplorer();

        services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc(
                "v1",
                new OpenApiInfo
                {
                    Title = "PropertyPro API",
                    Version = "v1",
                    Description =
                        "Property and facility management REST API."
                });

            options.AddSecurityDefinition(
                "Bearer",
                new OpenApiSecurityScheme
                {
                    Name = "Authorization",
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    In = ParameterLocation.Header,
                    Description =
                        "Enter your JWT token. Example: Bearer {token}"
                });

            options.AddSecurityRequirement(
                document =>
                    new OpenApiSecurityRequirement
                    {
                        [new OpenApiSecuritySchemeReference("Bearer", document)] =
                            []
                    });

            var xmlFile =
                $"{System.Reflection.Assembly.GetExecutingAssembly().GetName().Name}.xml";

            var xmlPath = Path.Combine(
                AppContext.BaseDirectory,
                xmlFile);

            if (File.Exists(xmlPath))
            {
                options.IncludeXmlComments(xmlPath);
            }
        });

        return services;
    }

    public static WebApplication UsePropertyProSwagger(
        this WebApplication app)
    {
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();

            app.UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint(
                    "/swagger/v1/swagger.json",
                    "PropertyPro API v1");

                options.DocumentTitle = "PropertyPro API";
            });
        }

        return app;
    }
}