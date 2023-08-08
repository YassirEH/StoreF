using Infrastructure.Models;

namespace Core.Interfaces
{
    public interface IProductCategoryRepository
    {
        ICollection<Product> GetProductByCategory(int categoryId);
        ICollection<Category> GetCategoryByProduct(int  productId);
    }
}
