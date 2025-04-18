using AutoMapper;
using Microsoft.EntityFrameworkCore;
using OrderService.Models;

namespace OrderService.Data.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly AppDbContext _context;

        public OrderRepository(AppDbContext context)
        {
            _context = context;
        }

        public async Task<Order> GetOrderById(int orderId, int userId)
        {
            return await _context.Orders
                .Include(o => o.Items)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);
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