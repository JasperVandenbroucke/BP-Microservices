using OrderService.Dtos;

namespace OrderService.AsyncDataServices
{
    public interface IMessageBusClient
    {
        Task InitializeAsync();
        Task PublishNewOrder(OrderPublishedDto orderPublishedDto);
    }
}