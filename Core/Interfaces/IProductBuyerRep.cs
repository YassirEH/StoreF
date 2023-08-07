using Infrastructure;
using Infrastructure.Models;

namespace Core.Interfaces
{
    public interface IProductBuyerRep
    {
        ICollection<Product> GetProductBuyer(int buyerId);
        ICollection<Buyer> GetBuyerOfProduct(int productId);
    }
}
