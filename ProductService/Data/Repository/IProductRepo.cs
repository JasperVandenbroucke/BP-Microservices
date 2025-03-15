using ProductService.Models;

namespace ProductService.Data.Repository
{
    public interface IProductRepo
    {
        bool SaveChanges();

        IEnumerable<Product> GetAllProducts();
        Product GetProductById(int id);
    }
}