using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public class ProductCategoryRep : IProductCategoryRep
    {
        private readonly DataContext _context;

        public ProductCategoryRep(DataContext context)
        {
            _context = context;
        }

        public int ProductId { get; internal set; }

        public ICollection<Category> GetCategoryByProduct(int productId)
        {
            return _context.ProductCategories.Where(p=>p.ProductId == productId).Select(w=>w.Category).ToList();
        }

        public ICollection<Product> GetProductByCategory(int categoryId)
        {
            return _context.ProductCategories.Where(e => e.CategoryId == categoryId).Select(c => c.Product).ToList();
        }
    }
}
