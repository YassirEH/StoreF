using Core.Application.Interfaces;

namespace Infrastructure.Services
{
    public class InventoryService : IInventoryService
    {
        private Dictionary<int, int> _productStocks;

        public InventoryService()
        {
            _productStocks = new Dictionary<int, int>();
        }

        public int GetCurrentStock(int productId)
        {
            // Check if the product exists in the dictionary
            if (_productStocks.ContainsKey(productId))
            {
                return _productStocks[productId];
            }

            return 0; // Product not found, return 0 stock
        }

        public bool IsProductInStock(int productId, int requiredQuantity)
        {
            int currentStock = GetCurrentStock(productId);
            return currentStock >= requiredQuantity;
        }

        public void UpdateStock(int productId, int quantity)
        {
            // Check if the product exists in the dictionary
            if (!_productStocks.ContainsKey(productId))
            {
                _productStocks[productId] = 0;
            }

            // Update the stock based on the quantity
            _productStocks[productId] += quantity;
        }

        public void AdjustStock(int productId, int adjustmentQuantity)
        {
            // Check if the product exists in the dictionary
            if (!_productStocks.ContainsKey(productId))
            {
                _productStocks[productId] = 0;
            }

            // Adjust the stock based on the adjustment quantity
            _productStocks[productId] += adjustmentQuantity;
        }
    }
}
