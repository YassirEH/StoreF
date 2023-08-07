using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Models;

namespace Infrastructure.Repositories
{
    public class ProductBuyerRep : IProductBuyerRep
    {
        private readonly DataContext _context;

        public ProductBuyerRep(DataContext context)
        {
            _context = context;
        }

        public ICollection<Buyer> GetBuyerOfProduct(int productId)
        {
            return _context.ProductBuyers.Where(pb => pb.ProductId == productId).Select(pb => pb.Buyer).ToList();
        }

        public ICollection<Product> GetProductBuyer(int buyerId)
        {
            return _context.ProductBuyers.Where(pb => pb.BuyerId == buyerId).Select(pb => pb.Product).ToList();
        }
    }
}
