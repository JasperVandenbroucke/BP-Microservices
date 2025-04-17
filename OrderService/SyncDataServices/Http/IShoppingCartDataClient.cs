using OrderService.Dtos;

namespace OrderService.SyncDataServices.Http
{
    public interface IShoppingCartDataClient
    {
        Task<ShoppingCartReadDto> GetShoppingCartContent(int userId);
    }
}