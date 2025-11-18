using PortalApi;
using Serilog;
using Serilog.Events;

Log.Logger = new LoggerConfiguration()
#if DEBUG
    .MinimumLevel.Debug()
#else
    .MinimumLevel.Information()
#endif
    .MinimumLevel.Override("Microsoft", LogEventLevel.Information)
    .Enrich.FromLogContext()
    .WriteTo.Async(c => c.Console())
    .CreateLogger();

try
{
    Log.Information("Starting PortalApi.HttpApi.Host");
    
    var builder = WebApplication.CreateBuilder(args);
    builder.Host.AddAppSettingsSecretsJson()
        .UseAutofac()
        .UseSerilog();
    
    await builder.AddApplicationAsync<PortalApiHttpApiHostModule>();
    
    var app = builder.Build();
    
    await app.InitializeApplicationAsync();
    
    await app.RunAsync();
    
    return 0;
}
catch (Exception ex)
{
    Log.Fatal(ex, "Host terminated unexpectedly!");
    return 1;
}
finally
{
    Log.CloseAndFlush();
}
