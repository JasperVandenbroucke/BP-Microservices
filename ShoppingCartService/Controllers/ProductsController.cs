using Microsoft.AspNetCore.Mvc;

namespace ShoppingCartService.Controllers
{
    [Route("api/shoppingcart/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        public ProductsController()
        {

        }

        [HttpPost]
        public ActionResult TestInboundConnection()
        {
            Console.WriteLine("--> Inbound POST # ShoppingCartService");
            return Ok("--> Inbound test from ShoppingCart Products Controller");
        }
    }
}