using Core.Application.Interfaces;
using Infrastructure.Services;
using Moq;
using Xunit;

namespace UnitTests.Infrastructure.Services
{
    public class InventoryServiceTests
    {
        [Fact]
        public void GetCurrentStock_ProductExists_ReturnsStock()
        {
            // Arrange
            var inventoryService = new InventoryService();
            int productId = 1;
            int expectedStock = 10;
            inventoryService.UpdateStock(productId, expectedStock);

            // Act
            int currentStock = inventoryService.GetCurrentStock(productId);

            // Assert
            Assert.Equal(expectedStock, currentStock);
        }

        [Fact]
        public void GetCurrentStock_ProductDoesNotExist_ReturnsZero()
        {
            // Arrange
            var inventoryService = new InventoryService();
            int productId = 1;

            // Act
            int currentStock = inventoryService.GetCurrentStock(productId);

            // Assert
            Assert.Equal(0, currentStock);
        }

        [Theory]
        [InlineData(10, 5, true)]   // Stock is greater than required quantity
        [InlineData(5, 10, false)]  // Stock is less than required quantity
        [InlineData(5, 5, true)]    // Stock is equal to required quantity
        public void IsProductInStock_ProductStockCheck(int currentStock, int requiredQuantity, bool expectedResult)
        {
            // Arrange
            var inventoryService = new InventoryService();
            int productId = 1;
            inventoryService.UpdateStock(productId, currentStock);

            // Act
            bool isProductInStock = inventoryService.IsProductInStock(productId, requiredQuantity);

            // Assert
            Assert.Equal(expectedResult, isProductInStock);
        }

        [Fact]
        public void UpdateStock_AddsQuantityToStock()
        {
            // Arrange
            var inventoryService = new InventoryService();
            int productId = 1;
            int initialStock = 5;
            int addedStock = 10;
            inventoryService.UpdateStock(productId, initialStock);

            // Act
            inventoryService.UpdateStock(productId, addedStock);

            // Assert
            int currentStock = inventoryService.GetCurrentStock(productId);
            Assert.Equal(initialStock + addedStock, currentStock);
        }

        [Fact]
        public void AdjustStock_AdjustsStockByQuantity()
        {
            // Arrange
            var inventoryService = new InventoryService();
            int productId = 1;
            int initialStock = 10;
            int adjustmentQuantity = -5;
            inventoryService.UpdateStock(productId, initialStock);

            // Act
            inventoryService.AdjustStock(productId, adjustmentQuantity);

            // Assert
            int currentStock = inventoryService.GetCurrentStock(productId);
            Assert.Equal(initialStock + adjustmentQuantity, currentStock);
        }
    }
}
