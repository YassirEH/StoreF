using Core.Interfaces;
using Core.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class CategoryRepository : ICategoryRep
    {
        private DataContext _context;

        public CategoryRepository(DataContext context)
        {
            _context = context;
        }

        public bool Save()
        {
            try
            {
                var saved = _context.SaveChanges();
                return saved > 0 ? true : false;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error saving changes: {ex.Message}");
                return false;
            }
        }

        public bool CategoryExists(int id)
        {
            try
            {
                return _context.Categories.Any(c => c.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error checking if category exists: {ex.Message}");
                return false;
            }
        }

        public Category GetCategory(int id)
        {
            try
            {
                return _context.Categories.FirstOrDefault(e => e.Id == id);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting category: {ex.Message}");
                return null;
            }
        }

        public ICollection<Category> FilterByName()
        {
            try
            {
                return _context.Categories.OrderBy(c => c.Name).ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error filtering categories by name: {ex.Message}");
                return null;
            }
        }

        public ICollection<Category> GetCategories()
        {
            try
            {
                return _context.Categories.ToList();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error getting categories: {ex.Message}");
                return null;
            }
        }

        public bool CreateCategory(Category category)
        {
            try
            {
                _context.Categories.Add(category);
                return Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error creating category: {ex.Message}");
                return false;
            }
        }

        public bool UpdateCategory(Category category)
        {
            try
            {
                _context.Update(category);
                return Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error updating category: {ex.Message}");
                return false;
            }
        }

        public bool DeleteCategory(Category category)
        {
            try
            {
                _context.Remove(category);
                return Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error deleting category: {ex.Message}");
                return false;
            }
        }
    }
}
