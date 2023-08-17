using Core.Application.Interfaces;
using Core.Domain.Models;
using Infrastructure.Data;

namespace Infrastructure.Repositories
{
    public class ProductRepository : IProductRep
    {
        private readonly DataContext _context;

        public ProductRepository(DataContext context)
        {
            _context = context;
        }

        public ICollection<Product> FilterByName()
        {
            return _context.Products.OrderBy(p => p.Name).ToList();
        }

        public ICollection<Product> FilterByPrice()
        {
            return _context.Products.OrderBy(p => p.Price).ToList();
        }

        public ICollection<Product> FilterByQuantity()
        {
            return _context.Products.OrderBy(p => p.Stock).ToList();
        }


        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0;
        }
    }
}
