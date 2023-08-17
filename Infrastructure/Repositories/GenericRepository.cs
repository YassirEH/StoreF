using Core.Application.Interfaces;
using Core.Domain.Models;
using Infrastructure.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class GenericRepository : IGenericRep
    {
        private readonly DataContext _context;

        public GenericRepository(DataContext context)
        {
            _context = context;
        }

        //Save Method
        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }

        //BUYER
        public bool BuyerExists(int id)
        {
            return _context.Buyers.Any(b => b.Id == id);
        }

        public Buyer GetBuyer(int id)
        {
            return _context.Buyers.FirstOrDefault(x => x.Id == id);
        }

        public ICollection<Buyer> GetBuyers()
        {
            return _context.Buyers.ToList();
        }

        public bool CreateBuyer(Buyer buyer)
        {
            _context.Buyers.Add(buyer);
            return Save();
        }

        public bool UpdateBuyer(Buyer buyer)
        {
            var existingBuyer = _context.Buyers.FirstOrDefault(b => b.Id == buyer.Id);
            existingBuyer!.FName = buyer.FName;
            existingBuyer.LName = buyer.LName;
            existingBuyer.Email = buyer.Email;

            return Save();
        }

        public bool DeleteBuyer(Buyer buyer)
        {
            _context.Remove(buyer);
            return Save();
        }

        //Category

        public bool CategoryExists(int id)
        {
            return _context.Categories.Any(c => c.Id == id);
        }

        public Category GetCategory(int id)
        {
            return _context.Categories.FirstOrDefault(e => e.Id == id);
        }

        public ICollection<Category> GetCategories()
        {
            return _context.Categories.ToList();
        }

        public bool CreateCategory(Category category)
        {
            _context.Categories.Add(category);
            return Save();
        }

        public bool UpdateCategory(Category category)
        {
            _context.Update(category);
            return Save();
        }

        public bool DeleteCategory(Category category)
        {
            _context.Remove(category);
            return Save();
        }

        //Product

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
