using Volo.Abp.Application;
using Volo.Abp.Modularity;

namespace PortalApi;

[DependsOn(
    typeof(PortalApiDomainModule),
    typeof(AbpDddApplicationContractsModule)
)]
public class PortalApiApplicationContractsModule : AbpModule
{
}
