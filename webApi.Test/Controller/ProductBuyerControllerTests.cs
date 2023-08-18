using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using webApi.Controllers;

namespace webApi.Test.Controller
{
    public class ProductBuyerControllerTests
    {
        private readonly Mock<IProductBuyerRep> _productBuyerRepMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly Mock<IBuyerRep> _buyerRepMock;
        private readonly ProductBuyerController _controller;

        public ProductBuyerControllerTests()
        {
            _productBuyerRepMock = new Mock<IProductBuyerRep>();
            _mapperMock = new Mock<IMapper>();
            _buyerRepMock = new Mock<IBuyerRep>();
            _controller = new ProductBuyerController(_productBuyerRepMock.Object, _mapperMock.Object, _buyerRepMock.Object);
        }

        [Fact]
        public void GetProductByBuyer_ValidId_ReturnsOkWithProducts()
        {
            // Arrange
            var buyerId = 1;
            var products = new List<Product>();
            _productBuyerRepMock.Setup(repo => repo.GetProductBuyer(buyerId)).Returns(products);

            // Act
            var result = _controller.GetproductByBuyer(buyerId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetBuyerOfProduct_ValidId_ReturnsOkWithBuyers()
        {
            // Arrange
            var productId = 1;
            var buyers = new List<Buyer>();
            _productBuyerRepMock.Setup(repo => repo.GetBuyerOfProduct(productId)).Returns(buyers);

            // Act
            var result = _controller.GetBuyerOfProduct(productId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void BuyerController_AssignProductToBuyer_BuyerNotFound_ReturnsNotFound()
        {
            // Arrange
            int buyerId = 1;
            int productId = 123;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.AssignProductToBuyer(buyerId, productId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void BuyerController_AssignProductToBuyer_InvalidProductId_ReturnsBadRequest()
        {
            // Arrange
            int buyerId = 1;
            int productId = 0;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(true);

            // Act
            var result = _controller.AssignProductToBuyer(buyerId, productId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void BuyerController_AssignProductToBuyer_ValidData_ReturnsOk()
        {
            // Arrange
            int buyerId = 1;
            int productId = 123;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(true);

            // Act
            var result = _controller.AssignProductToBuyer(buyerId, productId);

            // Assert
            Assert.IsType<OkResult>(result);
        }
    }
}
