using Core.Domain.Models;

namespace Core.Application.Interfaces
{
    public interface IBuyerRep
    {
        Buyer GetBuyer(int id);
        bool BuyerExists(int id);
        ICollection<Buyer> GetBuyers();
        bool CreateBuyer(Buyer buyer);
        bool UpdateBuyer(Buyer buyer);
        bool DeleteBuyer(Buyer buyer);
    }
}
