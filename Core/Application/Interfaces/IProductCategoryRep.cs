using Core.Domain.Models;

namespace Core.Application.Interfaces
{
    public interface IProductCategoryRep
    {
        ICollection<Product> GetProductByCategory(int categoryId);
        ICollection<Category> GetCategoryByProduct(int productId);
    }
}
