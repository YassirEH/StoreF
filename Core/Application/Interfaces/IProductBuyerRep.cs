using Core.Domain.Models;

namespace Core.Application.Interfaces
{
    public interface IProductBuyerRep
    {
        ICollection<Product> GetProductBuyer(int buyerId);
        ICollection<Buyer> GetBuyerOfProduct(int productId);
        bool AssignProductToBuyer(int buyerId, int productId);
    }
}
