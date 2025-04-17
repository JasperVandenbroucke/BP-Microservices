using OrderService.Dtos;
using OrderService.Models;

namespace OrderService.Data.Repository
{
    public interface IOrderRepository
    {
        Task<bool> SaveChanges();

        Task<Order> GetOrderById(int orderId);
        Task<Order> PlaceOrder(ShoppingCartReadDto shoppingCartDto);
    }
}