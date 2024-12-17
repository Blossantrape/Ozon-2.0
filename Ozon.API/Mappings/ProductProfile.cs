using AutoMapper;
using Ozon.Application.DTOs;
using Ozon.Core.Models;

namespace Ozon.API.Mappings;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        CreateMap<ProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<UpdateProductDto, Product>()
            .ForMember(dest => dest.Id, opt => opt.Ignore());
        CreateMap<Product, ProductDto>();
    }
}