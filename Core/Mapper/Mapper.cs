using AutoMapper;
using Core.Dto;
using Infrastructure.Models;

namespace Core.Mapper
{
    public class MappingProfiles : Profile
    {
        public MappingProfiles()
        {
            CreateMap<Product, ProductDto>();
            CreateMap<Category, CategoryDto>();
            CreateMap<Buyer, BuyerDto>();
            CreateMap<ProductDto, Product>();
            CreateMap<CategoryDto, Category>();
            CreateMap<BuyerDto, Buyer>();
        }
    }
}
