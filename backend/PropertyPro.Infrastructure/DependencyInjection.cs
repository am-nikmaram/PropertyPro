using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using PropertyPro.Application.Common.Interfaces;
using PropertyPro.Infrastructure.Identity;
using PropertyPro.Infrastructure.Persistence;
using PropertyPro.Infrastructure.Persistence.Interceptors;
using System.Text;

namespace PropertyPro.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(
        this IServiceCollection services,
        IConfiguration configuration)
    {
        //services.AddDbContext<ApplicationDbContext>(options =>
        //{
        //    options.UseSqlServer(
        //        configuration.GetConnectionString("DefaultConnection"));

        //});


        services.Configure<JwtOptions>(configuration.GetSection(JwtOptions.SectionName));

        services.AddIdentityCore<ApplicationUser>(options => 
        { 
            options.User.RequireUniqueEmail = true;
            options.Password.RequiredLength = 8;
            options.Password.RequireDigit = true;
            options.Password.RequireUppercase = true;
            options.Password.RequireLowercase = true;
            options.Password.RequireNonAlphanumeric = true;
            options.Lockout.MaxFailedAccessAttempts = 5;
            options.Lockout.DefaultLockoutTimeSpan = TimeSpan.FromMinutes(15); 
        })

            .AddRoles<IdentityRole>()
            .AddEntityFrameworkStores<ApplicationDbContext>()
            .AddSignInManager().
            AddDefaultTokenProviders();
        services.AddScoped<IIdentityService, IdentityService>();
        

        var jwtOptions = configuration
            .GetSection(JwtOptions.SectionName)
            .Get<JwtOptions>() 
            ?? throw new InvalidOperationException(
                "JWT configuration is missing.");

        var signingKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(jwtOptions.SecretKey));

        services.AddAuthentication(options =>
           {
               options.DefaultAuthenticateScheme =
               JwtBearerDefaults.AuthenticationScheme;
               options.DefaultChallengeScheme =
               JwtBearerDefaults.AuthenticationScheme;
           })
            .AddJwtBearer(options =>
            {
                options.TokenValidationParameters = new TokenValidationParameters
                {
                    ValidateIssuer = true,
                    ValidateAudience = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidIssuer = jwtOptions.Issuer,
                    ValidAudience = jwtOptions.Audience,
                    IssuerSigningKey = signingKey,
                    ClockSkew = TimeSpan.Zero
                };

                options.Events = new JwtBearerEvents
                {
                    OnChallenge = async context =>
                    {
                        context.HandleResponse();

                        context.Response.StatusCode =
                            StatusCodes.Status401Unauthorized;

                        context.Response.ContentType =
                            "application/problem+json";

                        var problemDetails = new ProblemDetails
                        {
                            Status = StatusCodes.Status401Unauthorized,
                            Title = "Unauthorized",
                            Detail = "Authentication is required to access this resource.",
                            Instance = context.HttpContext.Request.Path
                        };

                        problemDetails.Extensions["traceId"] =
                            context.HttpContext.TraceIdentifier;

                        await context.Response.WriteAsJsonAsync(
                            problemDetails);
                    },

                    OnForbidden = async context =>
                    {
                        context.Response.StatusCode =
                            StatusCodes.Status403Forbidden;

                        context.Response.ContentType =
                            "application/problem+json";

                        var problemDetails = new ProblemDetails
                        {
                            Status = StatusCodes.Status403Forbidden,
                            Title = "Forbidden",
                            Detail = "You do not have permission to access this resource.",
                            Instance = context.HttpContext.Request.Path
                        };

                        problemDetails.Extensions["traceId"] =
                            context.HttpContext.TraceIdentifier;

                        await context.Response.WriteAsJsonAsync(
                            problemDetails);
                    }


                };
            });
        

        services.AddDbContext<ApplicationDbContext>(
    (serviceProvider, options) =>
    {
        //options.UseSqlServer("DefaultConnection");
        options.UseSqlServer(configuration.GetConnectionString("DefaultConnection"));

        options.AddInterceptors(
            serviceProvider.GetRequiredService<
                AuditableEntityInterceptor>());
    });



        services.AddScoped<IApplicationDbContext>(
         provider => provider.GetRequiredService<ApplicationDbContext>());

        services.AddScoped<DatabaseInitializer>();

        
        services.AddScoped<ITokenService, TokenService>();


        services.Configure<RefreshTokenOptions>(
            configuration.GetSection(
        RefreshTokenOptions.SectionName));

        services.AddScoped<
            IRefreshTokenService,
            RefreshTokenService>();

        services.AddScoped<
            IUserManagementService,
            UserManagementService>();



        return services;
    }
}