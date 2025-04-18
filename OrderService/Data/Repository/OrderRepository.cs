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
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId);
        }

        public async Task PlaceOrder(Order orderToPlace)
        {
            _context.Orders.Add(orderToPlace);
            await SaveChanges();
        }

        public async Task<bool> SaveChanges()
        {
            return (await _context.SaveChangesAsync() >= 0);
        }
    }
}