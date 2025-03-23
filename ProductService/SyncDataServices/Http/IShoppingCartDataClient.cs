using ProductService.Dtos;

namespace ProductService.SyncDataServices.Http
{
    public interface IShoppingCartDataClient
    {
        Task SendProductToShoppingCart(ProductReadDto product);
    }
}