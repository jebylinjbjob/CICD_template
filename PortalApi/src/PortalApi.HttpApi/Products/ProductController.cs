using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Volo.Abp;
using Volo.Abp.Application.Dtos;
using Volo.Abp.AspNetCore.Mvc;

namespace PortalApi.Products;

[RemoteService]
[Area("app")]
[Route("api/app/products")]
public class ProductController : AbpController, IProductAppService
{
    private readonly IProductAppService _productAppService;

    public ProductController(IProductAppService productAppService)
    {
        _productAppService = productAppService;
    }

    [HttpGet]
    public virtual Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        return _productAppService.GetListAsync(input);
    }

    [HttpGet]
    [Route("{id}")]
    public virtual Task<ProductDto> GetAsync(Guid id)
    {
        return _productAppService.GetAsync(id);
    }

    [HttpPost]
    public virtual Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        return _productAppService.CreateAsync(input);
    }

    [HttpPut]
    [Route("{id}")]
    public virtual Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        return _productAppService.UpdateAsync(id, input);
    }

    [HttpDelete]
    [Route("{id}")]
    public virtual Task DeleteAsync(Guid id)
    {
        return _productAppService.DeleteAsync(id);
    }
}
