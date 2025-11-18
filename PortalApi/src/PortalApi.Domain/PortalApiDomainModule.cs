using Volo.Abp.Domain;
using Volo.Abp.Modularity;

namespace PortalApi;

[DependsOn(
    typeof(AbpDddDomainModule),
    typeof(PortalApiDomainSharedModule)
)]
public class PortalApiDomainModule : AbpModule
{
}
