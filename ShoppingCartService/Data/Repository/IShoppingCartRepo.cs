using ShoppingCartService.Models;

namespace ShoppingCartService.Data.Repository
{
    public interface IShoppingCartRepo
    {
        bool SaveChanges();

        Task<bool> AddProductToCart(int userId, int productId);
        Task<ShoppingCart> GetCart(int userId);
        void DeleteCart(ShoppingCart cart);
        bool ShoppingCartExists(int shoppingCartId, int userId);
    }
}