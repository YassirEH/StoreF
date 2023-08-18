using Core.Models;

namespace Core.Interfaces
{
    public interface IBuyerRep
    {
        public bool BuyerExists(int id);
        public Buyer GetBuyer(int id);
        public ICollection<Buyer> GetBuyers();
        public bool CreateBuyer(Buyer buyer);
        public bool UpdateBuyer(Buyer buyer);
        public bool DeleteBuyer(Buyer buyer);

    }
}
