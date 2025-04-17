using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.AsyncDataServices;
using OrderService.Data.Repository;
using OrderService.Dtos;
using OrderService.Models;
using OrderService.SyncDataServices.Http;

namespace OrderService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly IOrderRepository _repository;
        private readonly IMapper _mapper;
        private readonly IShoppingCartDataClient _dataClient;
        private readonly IMessageBusClient _messageBusClient;

        public OrdersController(IOrderRepository orderRepository, IMapper mapper, IShoppingCartDataClient shoppingCartDataClient, IMessageBusClient messageBusClient)
        {
            _repository = orderRepository;
            _mapper = mapper;
            _dataClient = shoppingCartDataClient;
            _messageBusClient = messageBusClient;
        }

        [HttpGet("{orderId}", Name = "GetOrderById")]
        public async Task<ActionResult<OrderReadDto>> GetOrderById(int orderId)
        {
            Console.WriteLine("--> Getting an order...");

            var order = await _repository.GetOrderById(orderId);
            if (order == null)
                return NotFound($"No order found with id {orderId}");

            return Ok(_mapper.Map<OrderReadDto>(order));
        }

        [HttpPost]
        public async Task<ActionResult<OrderReadDto>> CreateOrder()
        {
            Console.WriteLine("--> Placing an order...");
            try
            {
                // Get the user ID from the logged-in user
                var userIdHeader = Request.Headers["UserId"].FirstOrDefault();
                if (string.IsNullOrEmpty(userIdHeader))
                    return BadRequest("UserId header is required");
                if (!int.TryParse(userIdHeader, out int userId))
                    return BadRequest("UserId header must be a valid integer");

                var shoppingCartDto = await _dataClient.GetShoppingCartContent(userId);
                var orderToPlace = _mapper.Map<Order>(shoppingCartDto);
                orderToPlace.Status = "Verwerking";
                orderToPlace.TotalPrice = orderToPlace.Items.Sum(i => i.Price);

                await _repository.PlaceOrder(orderToPlace);

                var orderReadDto = _mapper.Map<OrderReadDto>(orderToPlace);

                // Tell the ShoppingCartService about the order
                try
                {
                    var orderPublishedDto = _mapper.Map<OrderPublishedDto>(orderReadDto);
                    orderPublishedDto.Event = "Order_Published";
                    await _messageBusClient.PublishNewOrder(orderPublishedDto);
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"--> Could not send asynchronously: {ex.Message}");
                }

                return CreatedAtRoute(nameof(GetOrderById), new { OrderId = orderReadDto.Id }, orderReadDto);
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}