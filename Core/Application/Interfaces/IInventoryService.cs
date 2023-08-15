namespace Core.Application.Interfaces
{
    public interface IInventoryService
    {
        void UpdateStock(int productId, int quantity);
        void AdjustStock(int productId, int adjustmentQuantity);
        int GetCurrentStock(int productId);
        bool IsProductInStock(int productId, int requiredQuantity);
    }
}
