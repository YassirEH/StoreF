using Infrastructure.Models;

namespace Core.Interfaces
{
    public interface IBuyerRepository
    {
        Buyer GetBuyer(int id);
        bool BuyerExists(int id);
        ICollection<Buyer> GetBuyers();
        bool CreateBuyer(Buyer buyer);
        bool UpdateBuyer(Buyer buyer);
        bool DeleteBuyer(Buyer buyer);
    }
}
