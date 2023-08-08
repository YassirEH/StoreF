using Infrastructure.Models;

namespace Core.Interfaces
{
    public interface IProductRepository
    {
        Product GetProduct(int id);
        Product GetProduct(string name);
        ICollection<Product> GetProducts();
        bool ProductExists(int productId);
        bool CreateProduct(Product product, int categoryId);
        bool UpdateProduct(int productId, Product updatedProduct);
        bool DeleteProduct(Product product);
    }
}
