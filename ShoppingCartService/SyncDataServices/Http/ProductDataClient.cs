using ShoppingCartService.Dtos;

namespace ShoppingCartService.SyncDataServices.Http
{
    public class ProductDataClient : IProductDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ProductDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<List<ProductReadDto>?> GetProductsById(IEnumerable<int> productIds)
        {
            var response = await _httpClient.PostAsJsonAsync($"{_configuration["ProductService"]}/bulk", productIds);
            if (response.IsSuccessStatusCode)
            {
                return await response.Content.ReadFromJsonAsync<List<ProductReadDto>>();
            }
            return null;
        }

        public async Task<bool> DoesProductExists(int productId)
        {
            Console.WriteLine("--> Does product exists?");
            Console.WriteLine($"--> Product address: {_configuration["ProductService"]}");
            var response = await _httpClient.GetAsync($"{_configuration["ProductService"]}/{productId}");
            if (response.IsSuccessStatusCode)
                return true;
            return false;
        }
    }
}