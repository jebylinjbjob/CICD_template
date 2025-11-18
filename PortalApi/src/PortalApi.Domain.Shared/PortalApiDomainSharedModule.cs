using Volo.Abp.Modularity;
using Volo.Abp.Validation;

namespace PortalApi;

[DependsOn(
    typeof(AbpValidationModule)
)]
public class PortalApiDomainSharedModule : AbpModule
{
}
