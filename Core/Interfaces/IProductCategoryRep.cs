using Infrastructure.Models;

namespace Core.Interfaces
{
    public interface IProductCategoryRep
    {
        ICollection<Product> GetProductByCategory(int categoryId);
    }
}
