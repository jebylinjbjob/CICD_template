using System;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;

namespace PortalApi.Products;

public interface IProductAppService : IApplicationService
{
    Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input);
    
    Task<ProductDto> GetAsync(Guid id);
    
    Task<ProductDto> CreateAsync(CreateUpdateProductDto input);
    
    Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input);
    
    Task DeleteAsync(Guid id);
}
