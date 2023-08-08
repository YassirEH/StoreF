using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class ProductRep : IProductRep
    {
        private readonly DataContext _context;

        public ProductRep(DataContext context)
        {
            _context = context;
        }

        public Product GetProduct(int id)
        {
            return _context.Products.FirstOrDefault(p => p.Id == id);
        }

        public Product GetProduct(string name)
        {
            return _context.Products.Include(p => p.ProductBuyers).FirstOrDefault(p => p.Name == name);
        }

        public ICollection<Product> GetProducts()
        {
            return _context.Products.OrderBy(p => p.Id).ToList();
        }

        public bool ProductExists(int productId)
        {
            return _context.Products.Any(p => p.Id == productId);
        }

        public bool CreateProduct(Product product, int categoryId)
        {
            var category = _context.Categories.FirstOrDefault(a => a.Id == categoryId);

            var productCategory = new ProductCategory()
            {
                Category = category,
                Product = product
            };

            _context.ProductCategories.Add(productCategory);
            _context.Add(product);

            return Save();
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        public bool UpdateProduct(int productId, Product updatedProduct)
        {
            var existingProduct = _context.Products.Find(productId);

            existingProduct!.Name = updatedProduct.Name;
            existingProduct.Description = updatedProduct.Description;
            existingProduct.Price = updatedProduct.Price;
            return Save();

        }

        public bool DeleteProduct(Product product)
        {
            _context.Products.Remove(product);
            return Save();
        }
    }
}
