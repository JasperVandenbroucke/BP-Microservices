using System.Text;
using System.Text.Json;
using ProductService.Dtos;

namespace ProductService.SyncDataServices.Http
{
    public class ShoppingCartDataClient : IShoppingCartDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _config;

        public ShoppingCartDataClient(HttpClient httpClient, IConfiguration config)
        {
            _httpClient = httpClient;
            _config = config;
        }

        public async Task SendProductToShoppingCart(ProductReadDto product)
        {
            var httpContent = new StringContent(
                JsonSerializer.Serialize(product),
                Encoding.UTF8,
                "application/json"
            );

            var response = await _httpClient.PostAsync(
                $"{_config["ShoppingCartService"]}",
                httpContent
            );

            if (response.IsSuccessStatusCode)
            {
                Console.WriteLine("--> Sync POST to ShoppingCartService was OK!");
            }
            else
            {
                Console.WriteLine("--> Sync POST to ShoppingCartService was NOT OK!");
            }
        }
    }
}