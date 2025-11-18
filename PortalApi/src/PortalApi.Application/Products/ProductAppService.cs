using System;
using System.Linq;
using System.Threading.Tasks;
using Volo.Abp.Application.Dtos;
using Volo.Abp.Application.Services;
using Volo.Abp.Domain.Repositories;

namespace PortalApi.Products;

public class ProductAppService : ApplicationService, IProductAppService
{
    private readonly IRepository<Product, Guid> _productRepository;

    public ProductAppService(IRepository<Product, Guid> productRepository)
    {
        _productRepository = productRepository;
    }

    public async Task<PagedResultDto<ProductDto>> GetListAsync(PagedAndSortedResultRequestDto input)
    {
        var queryable = await _productRepository.GetQueryableAsync();
        
        var products = queryable
            .OrderBy(p => p.Name)
            .Skip(input.SkipCount)
            .Take(input.MaxResultCount)
            .ToList();

        var totalCount = await _productRepository.GetCountAsync();

        return new PagedResultDto<ProductDto>(
            totalCount,
            ObjectMapper.Map<System.Collections.Generic.List<Product>, System.Collections.Generic.List<ProductDto>>(products)
        );
    }

    public async Task<ProductDto> GetAsync(Guid id)
    {
        var product = await _productRepository.GetAsync(id);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    public async Task<ProductDto> CreateAsync(CreateUpdateProductDto input)
    {
        var product = new Product(
            GuidGenerator.Create(),
            input.Name,
            input.Price,
            input.Stock
        )
        {
            Description = input.Description
        };

        await _productRepository.InsertAsync(product);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, CreateUpdateProductDto input)
    {
        var product = await _productRepository.GetAsync(id);
        
        product.Name = input.Name;
        product.Description = input.Description;
        product.Price = input.Price;
        product.Stock = input.Stock;

        await _productRepository.UpdateAsync(product);
        return ObjectMapper.Map<Product, ProductDto>(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        await _productRepository.DeleteAsync(id);
    }
}
