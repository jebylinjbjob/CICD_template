using Microsoft.Extensions.DependencyInjection;
using Volo.Abp.Http.Client;
using Volo.Abp.Modularity;

namespace PortalApi;

[DependsOn(
    typeof(PortalApiApplicationContractsModule),
    typeof(AbpHttpClientModule)
)]
public class PortalApiHttpApiClientModule : AbpModule
{
    public override void ConfigureServices(ServiceConfigurationContext context)
    {
        // Create dynamic client proxies for application services
        context.Services.AddHttpClientProxies(
            typeof(PortalApiApplicationContractsModule).Assembly,
            remoteServiceConfigurationName: "Default"
        );
    }
}
