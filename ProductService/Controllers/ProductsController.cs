using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using ProductService.Data.Repository;
using ProductService.Dtos;
using ProductService.SyncDataServices.Http;

namespace ProductService.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductRepo _repository;
        private readonly IMapper _mapper;
        private readonly IShoppingCartDataClient _shoppingCartDataClient;

        public ProductsController(
            IProductRepo repository,
            IMapper mapper,
            IShoppingCartDataClient shoppingCartDataClient)
        {
            _repository = repository;
            _mapper = mapper;
            _shoppingCartDataClient = shoppingCartDataClient;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProducts()
        {
            Console.WriteLine("--> Getting all products...");

            var productItems = await _repository.GetAllProducts();

            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(productItems));
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<ProductReadDto>> GetProductById(int id)
        {
            Console.WriteLine("--> Getting a single product...");

            var productItem = await _repository.GetProductById(id);
            if (productItem != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(productItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> CreateProduct(ProductCreateDto productCreateDto)
        {
            try
            {
                await _repository.CreateProduct(productCreateDto);
                return Ok("Successfully created a product");
            }
            catch (Exception)
            {
                return BadRequest("Failed to create product");
            }
        }

        [HttpPost("bulk")]
        public async Task<ActionResult<IEnumerable<ProductReadDto>>> GetProductsByIds([FromBody] List<int> productIds)
        {
            Console.WriteLine("--> Received call from ShoppingService...");

            var products = await _repository.GetProductsByIds(productIds);

            if (products == null)
                return NotFound("--> No products found");

            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(products));
        }
    }
}