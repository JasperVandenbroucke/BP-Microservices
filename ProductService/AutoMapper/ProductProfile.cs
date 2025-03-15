using AutoMapper;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.AutoMapper
{
    public class ProductProfile : Profile
    {
        public ProductProfile()
        {
            // Source -> Target
            CreateMap<Product, ProductReadDto>();
        }
    }
}