using Volo.Abp.AspNetCore.Mvc;
using Volo.Abp.Modularity;

namespace PortalApi;

[DependsOn(
    typeof(PortalApiApplicationContractsModule),
    typeof(AbpAspNetCoreMvcModule)
)]
public class PortalApiHttpApiModule : AbpModule
{
}
