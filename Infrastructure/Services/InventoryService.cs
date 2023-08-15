using Core.Application.Interfaces;
using Infrastructure.Data;

namespace Infrastructure.Services
{
    public class InventoryService : IInventoryService
    {
        private readonly DataContext _dbContext;

        public InventoryService(DataContext dbContext)
        {
            _dbContext = dbContext;
        }

        public int GetCurrentStock(int productId)
        {
            var product = _dbContext.Products.Find(productId);
            return product != null ? product.Stock : 0;
        }

        public bool IsProductInStock(int productId, int requiredQuantity)
        {
            int currentStock = GetCurrentStock(productId);
            return currentStock >= requiredQuantity;
        }

        public void UpdateStock(int productId, int quantity)
        {
            var product = _dbContext.Products.Find(productId);
            if (product != null)
            {
                product.Stock += quantity;
                _dbContext.SaveChanges();
            }
        }

        public void AdjustStock(int productId, int adjustmentQuantity)
        {
            var product = _dbContext.Products.Find(productId);
            if (product != null)
            {
                product.Stock += adjustmentQuantity;
                _dbContext.SaveChanges();
            }
        }

    }
}
