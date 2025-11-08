using Serilog;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using ApiGatewayService.WebApi.Infrastructure;

namespace ApiGatewayService.WebApi.Extensions;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddCustomLogging(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddSerilog((serviceProvider, loggerConfiguration) =>
        {
            loggerConfiguration
                .ReadFrom.Configuration(configuration)
                .Enrich.FromLogContext()
                .WriteTo.Console();
        });

        return services;
    }

    public static IServiceCollection AddJwtValidation(this IServiceCollection services, IConfiguration configuration)
    {
        var jwtOptionsSection = configuration.GetRequiredSection(nameof(JwtOptions));
        services.Configure<JwtOptions>(jwtOptionsSection);

        var jwtOptions = jwtOptionsSection.Get<JwtOptions>();
        var secretKey = Encoding.ASCII.GetBytes(jwtOptions.Secret);

        services
            .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
            .AddJwtBearer(JwtBearerDefaults.AuthenticationScheme, o =>
            {
                o.TokenValidationParameters = new()
                {
                    ValidateAudience = true,
                    ValidateIssuer = true,
                    ValidateLifetime = true,
                    ValidateIssuerSigningKey = true,
                    ValidAudience = jwtOptions.Audience,
                    ValidIssuer = jwtOptions.Issuer,
                    IssuerSigningKey = new SymmetricSecurityKey(secretKey)
                };
            });

        return services;
    }

    public static IServiceCollection AddYarp(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddReverseProxy()
            .LoadFromConfig(configuration.GetSection("ReverseProxy"));

        return services;
    }

    public static IServiceCollection AddCorsFrontEnd(this IServiceCollection services, IConfiguration configuration)
    {
        var corsOptionsSection = configuration.GetRequiredSection(nameof(CorsSettings));
        var corsSettings = corsOptionsSection.Get<CorsSettings>();

        services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(corsSettings.AllowedOrigins)
                        .AllowAnyHeader()
                        .AllowAnyMethod()
                        .AllowCredentials()
                        .WithExposedHeaders("Authorization", "authorization"); ;
            });
        });

        return services;
    }

    public static IServiceCollection AddAuthorizationPolicy(this IServiceCollection services)
    {
        services.AddAuthorization(options =>
        {
            options.AddPolicy("require-auth", policy =>
            {
                policy.RequireAuthenticatedUser();
            });
        });

        return services;
    }
}