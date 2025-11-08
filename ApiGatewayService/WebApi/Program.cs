using Serilog;
using System.Reflection;
using ApiGatewayService.WebApi.Extensions;
using ApiGatewayService.WebApi.Middleware;
using ApiGatewayService.WebApi.Services;

namespace ApiGatewayService.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(new WebApplicationOptions
        {
            ContentRootPath = Directory.GetCurrentDirectory()
        });

        builder.Configuration.AddJsonFile("yarpsettings.json", optional: false);

        builder.Services
            .AddSwaggerGen(opts =>
            {
                var xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath, true);
            })
            .AddHttpContextAccessor()
            .AddEndpointsApiExplorer()
            .AddCustomLogging(builder.Configuration)
            .AddCorsFrontEnd(builder.Configuration)
            .AddJwtValidation(builder.Configuration)
            .AddAuthorizationPolicy()
            .AddYarp(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        builder.Services.AddSingleton<IWhitelistService, WhitelistService>();

        var app = builder.Build();

        app.UseCors("AllowFrontend");

        if (!app.Environment.IsProduction())
        {
            app.UseSwagger();
            app.UseSwaggerUI(c =>
            {
                c.SwaggerEndpoint("/user-service/swagger.json", "User Service");
                c.SwaggerEndpoint("/currency-service/swagger.json", "Currency Service");
                c.SwaggerEndpoint("/favorites-service/swagger.json", "Favorites Service");
            });
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.UseAuthentication();
        app.UseAuthorization();
        app.UseMiddleware<AuthMiddleware>();

        app.MapReverseProxy();

        app.Run();
    }
}