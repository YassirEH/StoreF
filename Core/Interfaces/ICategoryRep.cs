using Infrastructure.Models;

namespace Core.Interfaces
{
    public interface ICategoryRep
    {
        ICollection<Category> GetCategories();
        Category GetCategory(int id);
        bool CategoryExists(int id);
        bool CreateCategory(Category category);
        bool UpdateCategory(Category category);
        bool DeleteCategory(Category category);
    }
}
