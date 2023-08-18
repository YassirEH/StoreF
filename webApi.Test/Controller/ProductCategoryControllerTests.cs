using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using webApi.Controllers;

namespace webApi.Test.Controller
{
    public class ProductCategoryControllerTests
    {
        private readonly ProductCategoryController _controller;
        private readonly Mock<IMapper> _mapper;
        private readonly Mock<IProductCategoryRep> _productCategoryRep;

        public ProductCategoryControllerTests()
        {
            _mapper = new Mock<IMapper>();
            _productCategoryRep = new Mock<IProductCategoryRep>();
            _controller = new ProductCategoryController(_productCategoryRep.Object, _mapper.Object);
        }

        [Fact]
        public void ProductCategoryController_GetProductByCategory_ReturnOk()
        {
            int categoryId = 1;
            var products = new List<Product>();
            _productCategoryRep.Setup(rep => rep.GetProductByCategory(categoryId)).Returns(products);

            var result = _controller.GetProductByCategory(categoryId);

            Assert.NotNull(result);
            Assert.IsType<OkObjectResult>(result);
        }


        [Fact]
        public void ProductCategoryController_GetProductByCategory_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var categoryId = 4;
            var productDto = new List<Product>();
            _productCategoryRep.Setup(repo => repo.GetProductByCategory(categoryId)).Returns(productDto);
            _controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = _controller.GetProductByCategory(categoryId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void GetCategoryByProduct_ValidId_ReturnsOkWithCategories()
        {
            // Arrange
            var productId = 1;
            var categories = new List<Category>(); // Assuming Category is the correct type
            _productCategoryRep.Setup(repo => repo.GetCategoryByProduct(productId)).Returns(categories);

            // Act
            var result = _controller.GetCategoryByProduct(productId);

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }



        [Fact]
        public void GetCategoryByProduct_InvalidId_ReturnsBadRequest()
        {
            // Arrange
            var productId = -1;
            _controller.ModelState.AddModelError("productId", "Invalid product ID");

            // Act
            var result = _controller.GetCategoryByProduct(productId);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
            var badRequestResult = result as BadRequestObjectResult;
            Assert.NotNull(badRequestResult);
            Assert.Equal(400, badRequestResult.StatusCode);
        }
    }
}
