using FavoritesService.Database;
using FavoritesService.WebApi.Extensions;
using FavoritesService.WebApi.Middleware;
using Serilog;
using System.Reflection;

namespace FavoritesService.WebApi;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services
            .AddSwaggerGen(opts =>
            {
                var xmlFile = Assembly.GetExecutingAssembly().GetName().Name + ".xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                opts.IncludeXmlComments(xmlPath, true);
            })
            .AddHttpContextAccessor()
            .AddEndpointsApiExplorer()
            .AddCustomDbContext(builder.Configuration)
            .AddInternalModules(builder.Configuration)
            .AddCustomLogging(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        var app = builder.Build();

        if (!app.Environment.IsProduction())
        {
            app.MigrateDb<CurrencyShowcaseContext>();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.UseMiddleware<UserIdentityMiddleware>();

        app.MapControllers();

        app.Run();
    }
}