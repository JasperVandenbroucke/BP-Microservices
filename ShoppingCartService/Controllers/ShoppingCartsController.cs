using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ShoppingCartService.Data.Repository;
using ShoppingCartService.Dtos;
using ShoppingCartService.SyncDataServices.Http;

namespace ShoppingCartService.Controllers
{
    [Route("api/shoppingcart")]
    [ApiController]
    public class ShoppingCartsController : ControllerBase
    {
        private readonly IShoppingCartRepo _repository;
        private readonly IMapper _mapper;
        private readonly IProductDataClient _productClient;

        public ShoppingCartsController(IShoppingCartRepo repository, IMapper mapper, IProductDataClient productClient)
        {
            _repository = repository;
            _mapper = mapper;
            _productClient = productClient;
        }

        [HttpGet]
        public async Task<ActionResult<ShoppingCartReadDto>> GetCart()
        {
            Console.WriteLine("--> Getting a shopping cart...");

            // Get the user ID from the logged-in user
            var userIdHeader = Request.Headers["UserId"].FirstOrDefault();
            if (string.IsNullOrEmpty(userIdHeader))
                return BadRequest("UserId header is required");

            if (!int.TryParse(userIdHeader, out int userId))
                return BadRequest("UserId header must be a valid integer");

            var cart = await _repository.GetCart(userId);

            if (cart == null)
                return NotFound($"Shopping cart not found for user {userId}");

            var cartDto = _mapper.Map<ShoppingCartReadDto>(cart);

            // Get the ids of the products in the cart
            var productIds = cartDto.Items.Select(i => i.ProductId).Distinct().ToList();
            if (productIds.Count > 0)
            {
                var products = await _productClient.GetProductsById(productIds);

                foreach (var item in cartDto.Items)
                {
                    var product = products?.FirstOrDefault(product => product.Id == item.ProductId);
                    item.Price = product.Price;
                    item.Name = product.Name;
                }
            }

            return Ok(cartDto);
        }

        [HttpPut("{productId}")]
        public async Task<ActionResult> AddProductToCart(int productId)
        {
            Console.WriteLine("--> Adding a product...");

            // Get the user ID from the logged-in user
            var userIdHeader = Request.Headers["UserId"].FirstOrDefault();
            if (string.IsNullOrEmpty(userIdHeader))
                return BadRequest("UserId header is required");

            if (!int.TryParse(userIdHeader, out int userId))
                return BadRequest("UserId header must be a valid integer");

            var productExists = await _productClient.DoesProductExists(productId);
            if (!productExists)
                return BadRequest($"Product with id {productId} does not exist");

            var response = await _repository.AddProductToCart(userId, productId);

            if (response)
                return Ok($"Product {productId} added to cart for user {userId}");
            else
                return BadRequest($"Failed to add product {productId} to cart for user {userId}");
        }
    }
}