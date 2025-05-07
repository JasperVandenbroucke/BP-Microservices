using Microsoft.EntityFrameworkCore;
using ProductService.Dtos;
using ProductService.Models;

namespace ProductService.Data.Repository
{
    public class ProductRepo : IProductRepo
    {
        private readonly AppDbContext _context;

        public ProductRepo(AppDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Product>> GetAllProducts()
        {
            return await _context.Products.ToListAsync();
        }

        public async Task<Product> GetProductById(int id)
        {
            return await _context.Products.FirstOrDefaultAsync(p => p.Id == id);
        }

        public async Task<IEnumerable<Product>> GetProductsByIds(List<int> ids)
        {
            return await _context.Products.Where(p => ids.Contains(p.Id)).ToListAsync();
        }

        public async Task CreateProduct(ProductCreateDto productCreateDto)
        {
            _context.Products.Add(new Product() { Name = productCreateDto.Name, Price = productCreateDto.Price });
            await _context.SaveChangesAsync();
        }

        public bool SaveChanges()
        {
            // If something was saved to the InMem db (>= 0), return truev
            return (_context.SaveChanges() >= 0);
        }
    }
}