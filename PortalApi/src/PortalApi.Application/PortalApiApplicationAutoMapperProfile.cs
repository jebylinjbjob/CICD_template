using AutoMapper;
using PortalApi.Products;

namespace PortalApi;

public class PortalApiApplicationAutoMapperProfile : Profile
{
    public PortalApiApplicationAutoMapperProfile()
    {
        CreateMap<Product, ProductDto>();
        CreateMap<CreateUpdateProductDto, Product>();
    }
}
