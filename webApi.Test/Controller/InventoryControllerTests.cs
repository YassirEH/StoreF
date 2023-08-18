using Core.Interfaces;
using Microsoft.AspNetCore.Mvc;
using webApi.Controllers;

namespace Tests
{
    public class InventoryControllerTests
    {
        private readonly Mock<IInventoryService> _mockInventoryService;
        private readonly InventoryController _inventoryController;

        public InventoryControllerTests()
        {
            _mockInventoryService = new Mock<IInventoryService>();
            _inventoryController = new InventoryController(_mockInventoryService.Object);
        }

        // Existing tests for PurchaseProduct, AddStock, and DeductStock...

        [Fact]
        public void UpdateStock_ValidInput_ReturnsOk()
        {
            // Arrange
            int productId = 1;
            int newStock = 20;
            int currentStock = 10;
            _mockInventoryService.Setup(service => service.GetCurrentStock(productId)).Returns(currentStock);

            // Act
            IActionResult result = _inventoryController.UpdateStock(productId, newStock);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Updated stock for Product ID: {productId} to {newStock}", (result as OkObjectResult)?.Value);
            _mockInventoryService.Verify(service => service.AddStock(productId, newStock - currentStock), Times.Once);
        }

        [Fact]
        public void PurchaseProduct_ProductInStock_ReturnsOk()
        {
            // Arrange
            int productId = 1;
            int quantity = 5;
            _mockInventoryService.Setup(service => service.IsProductInStock(productId, quantity)).Returns(true);

            // Act
            IActionResult result = _inventoryController.PurchaseProduct(productId, quantity);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal("Purchase successful!", (result as OkObjectResult)?.Value);
        }

        [Fact]
        public void PurchaseProduct_ProductOutOfStock_ReturnsBadRequest()
        {
            // Arrange
            int productId = 1;
            int quantity = 5;
            _mockInventoryService.Setup(service => service.IsProductInStock(productId, quantity)).Returns(false);

            // Act
            IActionResult result = _inventoryController.PurchaseProduct(productId, quantity);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            Assert.Equal("Product is out of stock.", (result as BadRequestObjectResult)?.Value);
        }

        [Fact]
        public void DeductStock_ValidInput_ReturnsOk()
        {
            // Arrange
            int productId = 56456;
            int quantity = 5345345;

            // Act
            IActionResult result = _inventoryController.DeductStock(productId, quantity);

            // Assert
            Assert.IsType<OkObjectResult>(result);
            Assert.Equal($"Deducted {quantity} units of stock for Product ID: {productId}", (result as OkObjectResult)?.Value);
        }

    }
}
