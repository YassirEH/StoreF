using Core.Interfaces;
using Infrastructure.Data;
using Infrastructure.Models;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Repositories
{
    public class BuyerRep : IBuyerRep
    {
        private readonly DataContext _context;

        public BuyerRep(DataContext context)
        {
            _context = context;
        }

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

        public void AssignBuyerToProduct(int buyerId, List<int> ProductIds)
        {
            var buyer = _context.Buyers
                .Include(b => b.ProductBuyers)
                .FirstOrDefault(b => b.Id == buyerId);

            if (buyer != null)
            {
                foreach (int productId in ProductIds)
                {
                    var existingProduct = buyer.ProductBuyers.FirstOrDefault(pb => pb.ProductId == productId);
                    if (existingProduct == null)
                    {
                        var productBuyer = new ProductBuyer
                        {
                            BuyerId = buyerId,
                            ProductId = productId,
                        };
                        _context.ProductBuyers.Add(productBuyer);
                    }
                }
                Save();
            }
        }

        public bool Save()
        {
            var saved = _context.SaveChanges();
            return saved > 0 ? true : false;
        }

        public bool UpdateBuyer(Buyer buyer)
        {
            var existingBuyer = _context.Buyers.FirstOrDefault(b => b.Id == buyer.Id);
            existingBuyer.FName = buyer.FName;
            existingBuyer.LName = buyer.LName;
            existingBuyer.Email = buyer.Email;

            return Save();
        }

        public bool DeleteBuyer(Buyer buyer)
        {
            _context.Remove(buyer);
            return Save();
        }
    }
}