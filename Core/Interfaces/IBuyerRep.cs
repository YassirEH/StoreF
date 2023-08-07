using Infrastructure.Models;

namespace Core.Interfaces
{
    public interface IBuyerRep
    {
        Buyer GetBuyer(int id);
        bool BuyerExists(int id);
        ICollection<Buyer> GetBuyers();
        bool CreateBuyer(Buyer buyer);
        void AssignBuyerToProduct(int buyerId, List<int> ProductIds);
        bool UpdateBuyer(Buyer buyer);
        bool DeleteBuyer(Buyer buyer);
    }
}
