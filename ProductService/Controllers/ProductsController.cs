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
        public ActionResult<IEnumerable<ProductReadDto>> GetProducts()
        {
            Console.WriteLine("--> Getting all products...");

            var productItems = _repository.GetAllProducts();

            return Ok(_mapper.Map<IEnumerable<ProductReadDto>>(productItems));
        }

        [HttpGet("{id}")]
        public ActionResult<ProductReadDto> GetProductById(int id)
        {
            Console.WriteLine("--> Getting a single product...");

            var productItem = _repository.GetProductById(id);
            if (productItem != null)
            {
                return Ok(_mapper.Map<ProductReadDto>(productItem));
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<ActionResult> TestInboundConnection()
        {
            var productDto = new ProductReadDto() { Id = 9, Name = "test", Price = 12.5 };

            try
            {
                await _shoppingCartDataClient.SendProductToShoppingCart(productDto);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"--> Could not send synchronously: {ex.Message}");
            }

            return Ok("--> Succesfuly send POST from ProductServer");
        }
    }
}