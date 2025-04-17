using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;
        private readonly IMapper _mapper;

        public OrderRepository(AppDbContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        public async Task<Order> GetOrderById(int orderId)
        {
            return await _context.Orders.FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task<Order> PlaceOrder(ShoppingCartReadDto shoppingCartDto) // dto instead of int
        {
            var orderToPlace = _mapper.Map<Order>(shoppingCartDto);
            orderToPlace.Status = "Verwerking";
            orderToPlace.TotalPrice = orderToPlace.Items.Sum(i => i.Price);

            _context.Orders.Add(orderToPlace);
            await SaveChanges();

            return orderToPlace;
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}