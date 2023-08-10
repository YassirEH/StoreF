using Core.Application.Dto;
using Core.Application.Interfaces;
using Core.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using webApi.Controllers;

namespace webApi.Test.Controller
{
    public class ProductControllerTests
    {
        private readonly Mock<IProductRep> _productRepMock;
        private readonly Mock<IMapper> _mapperMock;

        public ProductControllerTests()
        {
            _productRepMock = new Mock<IProductRep>();
            _mapperMock = new Mock<IMapper>();
        }

        [Fact]
        public void ProductController_GetProduct_ReturnOk()
        {
            // Arrange
            var products = new Mock<ICollection<ProductDto>>();
            var productList = new Mock<List<ProductDto>>();
            _mapperMock.Setup(mapper => mapper.Map<List<ProductDto>>(products.Object)).Returns(productList.Object);
            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object);

            // Act
            var result = controller.GetProducts();

            // Assert
            result.Should().NotBeNull().And.BeOfType<OkObjectResult>();
        }

        [Fact]
        public void ProductController_CreateProduct_ReturnOk()
        {
            // Arrange
            int categoryId = 2;
            var productMap = new Mock<Product>();
            var product = new Mock<Product>();
            var productCreate = new Mock<ProductDto>();
            var products = new Mock<ICollection<ProductDto>>();
            var productList = new Mock<IList<ProductDto>>();
            _mapperMock.Setup(mapper => mapper.Map<Product>(productCreate.Object)).Returns(product.Object);
            _productRepMock.Setup(rep => rep.CreateProduct(productMap.Object, categoryId)).Returns(true);
            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object);

            // Act
            var result = controller.CreateProduct(productCreate.Object, categoryId);

            // Assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1)]
        public void ProductController_UpdateProduct_ReturnOk(int productId)
        {
            // Arrange
            var product = new Mock<Product>();
            var existingProduct = new Mock<ProductDto>();
            _mapperMock.Setup(mapper => mapper.Map<Product>(existingProduct.Object)).Returns(product.Object);
            _productRepMock.Setup(rep => rep.UpdateProduct(productId, product.Object)).Returns(true);
            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object);

            // Act
            var result = controller.UpdateProduct(productId, existingProduct.Object);

            // Assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1)]
        public void ProductController_DeleteProduct_ReturnOk(int productId)
        {
            // Arrange
            var productToDelete = new Mock<Product>();
            _productRepMock.Setup(rep => rep.GetProduct(productId)).Returns(productToDelete.Object);
            _productRepMock.Setup(rep => rep.DeleteProduct(productToDelete.Object)).Returns(true);
            var controller = new ProductController(_productRepMock.Object, _mapperMock.Object);

            // Act
            var result = controller.DeleteProduct(productId);

            // Assert
            result.Should().NotBeNull();
        }
    }
}
