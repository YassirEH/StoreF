using Core.Domain.Models;

namespace Core.Application.Interfaces
{
    public interface IProductRep
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
