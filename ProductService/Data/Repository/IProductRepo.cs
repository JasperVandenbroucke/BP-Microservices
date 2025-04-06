using ProductService.Models;

namespace ProductService.Data.Repository
{
    public interface IProductRepo
    {
        bool SaveChanges();

        Task<IEnumerable<Product>> GetAllProducts();
        Task<Product> GetProductById(int id);
        Task<IEnumerable<Product>> GetProductsByIds(List<int> ids);
    }
}