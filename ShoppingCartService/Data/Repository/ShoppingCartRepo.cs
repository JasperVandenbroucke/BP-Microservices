using Microsoft.EntityFrameworkCore;
using ShoppingCartService.Models;

namespace ShoppingCartService.Data.Repository
{
    public class ShoppingCartRepo : IShoppingCartRepo
    {
        private readonly AppDbContext _context;

        public ShoppingCartRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<bool> AddProductToCart(int userId, int productId)
        {
            var cart = await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            // No cart found, create a new one
            if (cart == null)
            {
                cart = new ShoppingCart { UserId = userId };
                _context.ShoppingCarts.Add(cart);
                SaveChanges();
            }

            var cartItem = new ShoppingCartItem
            {
                ShoppingCartId = cart.Id,
                ProductId = productId
            };

            _context.ShoppingCartItems.Add(cartItem);
            return SaveChanges();
        }

        public async Task<ShoppingCart> GetCart(int userId)
        {
            return await _context.ShoppingCarts
                .Include(c => c.Items)
                .FirstOrDefaultAsync(c => c.UserId == userId);
        }

        public bool SaveChanges()
        {
            return (_context.SaveChanges() >= 0);
        }
    }
}