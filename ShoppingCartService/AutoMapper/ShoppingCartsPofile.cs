using AutoMapper;
using ShoppingCartService.Dtos;
using ShoppingCartService.Models;

namespace ShoppingCartService.AutoMapper
{
    public class ShoppingCartsProfile : Profile
    {
        public ShoppingCartsProfile()
        {
            // Source -> Target
            CreateMap<ShoppingCart, ShoppingCartReadDto>();
            CreateMap<ShoppingCartItem, ShoppingCartItemReadDto>();
            CreateMap<ShoppingCartItemCreateDto, ShoppingCartItem>()
                .ForMember(dest => dest.ShoppingCartId, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}