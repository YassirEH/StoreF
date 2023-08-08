using Infrastructure.Models;

namespace Core.Interfaces
{
    public interface IProductBuyerRepository
    {
        ICollection<Product> GetProductBuyer(int buyerId);
        ICollection<Buyer> GetBuyerOfProduct(int productId);
        bool AssignProductToBuyer(int buyerId, int productId);
    }
}
