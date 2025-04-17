using OrderService.Dtos;

namespace OrderService.SyncDataServices.Http
{
    public class ShoppingCartDataClient : IShoppingCartDataClient
    {
        private readonly HttpClient _httpClient;
        private readonly IConfiguration _configuration;

        public ShoppingCartDataClient(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _configuration = configuration;
        }

        public async Task<ShoppingCartReadDto> GetShoppingCartContent(int userId)
        {
            try
            {
                var request = new HttpRequestMessage(HttpMethod.Get, _configuration["ShoppingCartService"]);
                request.Headers.Add("UserId", userId.ToString());

                var response = await _httpClient.SendAsync(request);
                if (!response.IsSuccessStatusCode)
                {
                    var errorMessage = await response.Content.ReadAsStringAsync();
                    throw new Exception($"ShoppingCartService returned {response.StatusCode}: {errorMessage}");
                }

                var cartDto = await response.Content.ReadFromJsonAsync<ShoppingCartReadDto>()
                    ?? throw new Exception("Could not deserialize cart");
                return cartDto;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}