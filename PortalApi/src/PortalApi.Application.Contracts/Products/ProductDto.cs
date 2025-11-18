using System;
using Volo.Abp.Application.Dtos;

namespace PortalApi.Products;

public class ProductDto : EntityDto<Guid>
{
    public string Name { get; set; } = string.Empty;
    
    public string? Description { get; set; }
    
    public decimal Price { get; set; }
    
    public int Stock { get; set; }
    
    public DateTime CreatedDate { get; set; }
}
