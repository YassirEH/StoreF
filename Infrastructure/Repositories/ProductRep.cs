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

        public bool AddCategoriesToProduct(int productId, List<int> categoryIds)
        {
            var product = _context.Products
                .Include(p => p.ProductCategories)
                .FirstOrDefault(p => p.Id == productId);

            if (product != null)
            {
                foreach (int categoryId in categoryIds)
                {
                    var existingCategory = product.ProductCategories.FirstOrDefault(pc => pc.CategoryId == categoryId);
                    if (existingCategory == null)
                    {
                        var productCategory = new ProductCategory
                        {
                            ProductId = productId,
                            CategoryId = categoryId
                        };
                        _context.ProductCategories.Add(productCategory);
                    }
                }
                return Save();
            }
            return Save();
        }

        public bool SetPrice(int productId, double price)
        {
            var product = _context.Products.Find(productId);

            if (product != null)
            {
                product.Price = price;
                return Save();
            }
            else
            {
                throw new ArgumentException("Product not found with the given productId.");
            }
        }

        public bool CreateProduct(Product product, List<int> categoryIds)
        {
            _context.Products.Add(product);

            if (categoryIds != null && categoryIds.Any())
            {
                foreach (int categoryId in categoryIds)
                {
                    var productCategory = new ProductCategory
                    {
                        Product = product,
                        CategoryId = categoryId
                    };
                    _context.ProductCategories.Add(productCategory);
                }
            }

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

                existingProduct.Name = updatedProduct.Name;
                existingProduct.Description = updatedProduct.Description;
                existingProduct.Price = updatedProduct.Price;
                return Save();
        }

        public bool DeleteProduct(Product product)
        {
            _context.Remove(product);
            return Save();
        }
    }
}
