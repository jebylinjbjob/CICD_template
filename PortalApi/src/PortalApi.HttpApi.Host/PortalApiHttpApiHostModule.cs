using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;
using PortalApi.EntityFrameworkCore;
using Volo.Abp;
using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.AspNetCore.Serilog;
using Volo.Abp.Autofac;
using Volo.Abp.EntityFrameworkCore;
using Volo.Abp.Modularity;

namespace PortalApi;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(AbpAspNetCoreMvcModule),
    typeof(AbpAspNetCoreSerilogModule),
    typeof(PortalApiApplicationModule),
    typeof(PortalApiHttpApiModule),
    typeof(PortalApiEntityFrameworkCoreModule)
)]
public class PortalApiHttpApiHostModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        var configuration = context.Services.GetConfiguration();
        
        // Configure Controllers
        context.Services.AddControllers();
        
        // Configure Swagger/OpenAPI
        context.Services.AddSwaggerGen(options =>
        {
            options.SwaggerDoc("v1", new OpenApiInfo
            {
                Title = "PortalApi API",
                Version = "v1",
                Description = "Portal API - ABP Framework",
                Contact = new OpenApiContact
                {
                    Name = "API Support",
                    Email = "support@example.com"
                }
            });
            options.DocInclusionPredicate((docName, description) => true);
            options.CustomSchemaIds(type => type.FullName);
        });
        
        // Configure ABP DB Context Options
        Configure<AbpDbContextOptions>(options =>
        {
            options.Configure<PortalApiDbContext>(opts =>
            {
                opts.DbContextOptions.UseNpgsql(configuration.GetConnectionString("Default"));
            });
        });
    }

    public override void OnApplicationInitialization(ApplicationInitializationContext context)
    {
        var app = context.GetApplicationBuilder();
        var env = context.GetEnvironment();

        if (env.IsDevelopment())
        {
            app.UseDeveloperExceptionPage();
        }

        // Enable Swagger middleware
        app.UseSwagger();
        app.UseSwaggerUI(options =>
        {
            options.SwaggerEndpoint("/swagger/v1/swagger.json", "PortalApi API v1");
            options.RoutePrefix = "swagger";
        });

        app.UseAbpRequestLocalization();
        app.UseRouting();
        app.UseConfiguredEndpoints();
    }
}
