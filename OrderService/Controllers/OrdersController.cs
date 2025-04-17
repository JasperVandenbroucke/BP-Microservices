using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using OrderService.Data.Repository;
using OrderService.Dtos;
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

        public OrdersController(IOrderRepository orderRepository, IMapper mapper, IShoppingCartDataClient shoppingCartDataClient)
        {
            _repository = orderRepository;
            _mapper = mapper;
            _dataClient = shoppingCartDataClient;
        }

        [HttpGet("{orderId}", Name = "GetOrderById")]
        public async Task<ActionResult<OrderReadDto>> GetOrderById(int orderId)
        {
            var order = await _repository.GetOrderById(orderId);
            if (order == null)
                return NotFound($"No order found with id {orderId}");

            return Ok(_mapper.Map<OrderReadDto>(order));
        }

        [HttpPost]
        public async Task<ActionResult<OrderReadDto>> CreateOrder()
        {
            try
            {
                // Get the user ID from the logged-in user
                var userIdHeader = Request.Headers["UserId"].FirstOrDefault();
                if (string.IsNullOrEmpty(userIdHeader))
                    return BadRequest("UserId header is required");
                if (!int.TryParse(userIdHeader, out int userId))
                    return BadRequest("UserId header must be a valid integer");

                var shoppingCartDto = await _dataClient.GetShoppingCartContent(userId);

                var newOrder = await _repository.PlaceOrder(shoppingCartDto);
                if (newOrder == null)
                    return BadRequest("Could not place the order...");

                return Ok(_mapper.Map<OrderReadDto>(newOrder));
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}