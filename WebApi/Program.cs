using Serilog;
using System.Reflection;
using CurrencyUpdaterService.WebApi.Extensions;
using CurrencyUpdaterService.Database;

namespace CurrencyUpdaterService.WebApi;

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
            .AddHttpClients(builder.Configuration)
            .AddCustomLogging(builder.Configuration);

        builder.Services.AddControllers();
        builder.Services.AddRouting(options =>
        {
            options.LowercaseUrls = true;
        });

        var app = builder.Build();

        if (!app.Environment.IsProduction())
        {
            app.MigrateDb<CurrencyUpdaterContext>();
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseSerilogRequestLogging();

        app.UseHttpsRedirection();

        app.MapControllers();

        app.Run();
    }
}