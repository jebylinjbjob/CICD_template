using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using PortalApi.EntityFrameworkCore;
using Serilog;
using Serilog.Events;
using Volo.Abp.Data;

namespace PortalApi.DbMigrator;

public class Program
{
    public static async Task<int> Main(string[] args)
    {
        Log.Logger = new LoggerConfiguration()
            .MinimumLevel.Information()
            .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
            .MinimumLevel.Override("Volo.Abp", LogEventLevel.Warning)
            .Enrich.FromLogContext()
            .WriteTo.Console()
            .CreateLogger();

        try
        {
            Log.Information("Starting PortalApi.DbMigrator");

            var builder = Host.CreateApplicationBuilder(args);
            
            builder.Services.AddLogging(c => c.AddSerilog());
            
            // Add ABP modules
            await builder.Services.AddApplicationAsync<PortalApiDbMigratorModule>();
            
            var app = builder.Build();
            
            await app.Services.GetRequiredService<IDataSeeder>().SeedAsync();
            
            Log.Information("PortalApi.DbMigrator completed successfully");
            
            return 0;
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "PortalApi.DbMigrator terminated unexpectedly!");
            return 1;
        }
        finally
        {
            Log.CloseAndFlush();
        }
    }
}
