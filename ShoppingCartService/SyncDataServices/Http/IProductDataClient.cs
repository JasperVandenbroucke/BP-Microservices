using ShoppingCartService.Dtos;

namespace ShoppingCartService.SyncDataServices.Http
{
    public interface IProductDataClient
    {
        Task<List<ProductReadDto>?> GetProductsById(IEnumerable<int> productId);
        Task<bool> DoesProductExists(int productId);
    }
}