using Infrastructure.Models;

namespace Core.Interfaces
{
    public interface IProductRep
    {
        Product GetProduct(int id);
        Product GetProduct(string name);
        ICollection<Product> GetProducts();
        bool ProductExists(int productId);
        bool AddCategoriesToProduct(int productId, List<int> categoryIds);
        bool SetPrice(int productId, double price);
        bool CreateProduct(Product product, List<int> categoryIds);
        bool UpdateProduct(int productId, Product updatedProduct);
        bool DeleteProduct(Product product);
    }
}
