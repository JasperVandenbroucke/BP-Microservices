using System.Text.Json;
using AutoMapper;
using ShoppingCartService.Data.Repository;
using ShoppingCartService.Dtos;

namespace ShoppingCartService.EventProcessing
{
    public class EventProcessor : IEventProcessor
    {
        private readonly IServiceScopeFactory _scopeFactory;
        private readonly IMapper _mapper;

        public EventProcessor(IServiceScopeFactory scopeFactory, IMapper mapper)
        {
            _scopeFactory = scopeFactory;
            _mapper = mapper;
        }

        public async Task ProcessEvent(string message)
        {
            var eventType = DetermineEvent(message);

            switch (eventType)
            {
                case EventType.OrderPublished:
                    await RemoveShoppingCart(message);
                    break;
                default:
                    break;
            }
        }

        private EventType DetermineEvent(string notificationMessage)
        {
            Console.WriteLine("--> Determining Event");

            var eventType = JsonSerializer.Deserialize<GenericEventDto>(notificationMessage);

            switch (eventType.Event)
            {
                case "Order_Published":
                    Console.WriteLine("--> Order Published Event Detected");
                    return EventType.OrderPublished;
                default:
                    Console.WriteLine("--> Could not determine the event type");
                    return EventType.Undetermined;
            }
        }

        private async Task RemoveShoppingCart(string orderPublishedMessage)
        {
            using var scope = _scopeFactory.CreateScope();
            var repo = scope.ServiceProvider.GetRequiredService<IShoppingCartRepo>();

            var orderPublishedDto = JsonSerializer.Deserialize<OrderPublishedDto>(orderPublishedMessage);
            var shoppingCartId = orderPublishedDto.ShoppingCartId;
            var userId = orderPublishedDto.UserId;

            try
            {
                if (repo.ShoppingCartExists(shoppingCartId, userId))
                {
                    var cart = await repo.GetCart(userId);
                    repo.DeleteCart(cart);
                    Console.WriteLine("--> ShoppingCart removed");
                }
                else
                {
                    Console.WriteLine("--> ShoppingCart does not exists...");
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not remove ShoppingCart: {ex.Message}");
            }
        }
    }

    enum EventType
    {
        OrderPublished,
        Undetermined
    }
}