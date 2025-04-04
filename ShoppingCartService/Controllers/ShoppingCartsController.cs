using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.Data.Repository;
using ShoppingCartService.Dtos;

namespace ShoppingCartService.Controllers
{
    [Route("api/shoppingcart")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IShoppingCartRepo _repository;
        private readonly IMapper _mapper;

        public ShoppingCartsController(IShoppingCartRepo repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCartReadDto>> GetCart()
        {
            // Get the user ID from the logged-in user
            var userIdHeader = Request.Headers["UserId"].FirstOrDefault();
            if (string.IsNullOrEmpty(userIdHeader))
                return BadRequest("UserId header is required");

            if (!int.TryParse(userIdHeader, out int userId))
                return BadRequest("UserId header must be a valid integer");

            var cart = await _repository.GetCart(userId);

            if (cart == null)
                return NotFound($"Shopping cart not found for user {userId}");

            return Ok(_mapper.Map<ShoppingCartReadDto>(cart));
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult> AddProductToCart(int productId)
        {
            // Get the user ID from the logged-in user
            var userIdHeader = Request.Headers["UserId"].FirstOrDefault();
            if (string.IsNullOrEmpty(userIdHeader))
                return BadRequest("UserId header is required");

            if (!int.TryParse(userIdHeader, out int userId))
                return BadRequest("UserId header must be a valid integer");

            var response = await _repository.AddProductToCart(userId, productId);

            if (response)
                return Ok($"Product {productId} added to cart for user {userId}");
            else
                return BadRequest($"Failed to add product {productId} to cart for user {userId}");
        }
    }
}