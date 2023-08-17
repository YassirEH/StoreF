using Core.Domain.Models;

namespace Core.Application.Interfaces
{
    public interface IProductRep    
    {
        public ICollection<Product> FilterByPrice();
        public ICollection<Product> FilterByName();
        public ICollection<Product> FilterByQuantity();
    }
}
