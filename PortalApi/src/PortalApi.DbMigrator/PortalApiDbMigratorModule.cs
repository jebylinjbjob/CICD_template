using Microsoft.Extensions.DependencyInjection;
using PortalApi.EntityFrameworkCore;
using Volo.Abp.Autofac;
using Volo.Abp.Modularity;

namespace PortalApi.DbMigrator;

[DependsOn(
    typeof(AbpAutofacModule),
    typeof(PortalApiEntityFrameworkCoreModule)
)]
public class PortalApiDbMigratorModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Configure services
    }
}
