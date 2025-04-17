using AutoMapper;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.AutoMapper
{
    public class OrdersProfile : Profile
    {
        public OrdersProfile()
        {
            CreateMap<Order, OrderReadDto>();
            CreateMap<OrderItem, OrderItemReadDto>();
            CreateMap<ShoppingCartItemReadDto, OrderItem>();
            CreateMap<ShoppingCartReadDto, Order>()
                .ForMember(dest => dest.Items, opt => opt.MapFrom(src => src.Items))
                .ForMember(dest => dest.ShoppingCartId, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.TotalPrice, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.Id, opt => opt.Ignore());
        }
    }
}