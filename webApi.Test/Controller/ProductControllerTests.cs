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
        private readonly IProductRep _productRep;
        private readonly IMapper _mapper;
        public ProductControllerTests()
        {
            _productRep = A.Fake<IProductRep>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void ProductController_GetProduct_ReturnOk()
        {
            //arrange
            var products = A.Fake<ICollection<ProductDto>>();
            var productList = A.Fake<List<ProductDto>>();
            A.CallTo(() => _mapper.Map<List<ProductDto>>(products)).Returns(productList);
            var controller = new ProductController(_productRep, _mapper);

            //act
            var result = controller.GetProducts();
            //assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Fact]
        public void ProductController_CreateProduct_ReturnOk()
        {
            //Arrange
            int categoryId = 2;
            var productMap = A.Fake<Product>();
            var product = A.Fake<Product>();
            var productCreate = A.Fake<ProductDto>();
            var products = A.Fake<ICollection<ProductDto>>();
            var productList = A.Fake<IList<ProductDto>>();
            A.CallTo(() => _mapper.Map<Product>(productCreate)).Returns(product);
            A.CallTo(() => _productRep.CreateProduct(productMap, categoryId)).Returns(true);
            var controller = new ProductController(_productRep, _mapper);

            //Act
            var result = controller.CreateProduct(productCreate, categoryId);

            //Assert
            result.Should().NotBeNull();
        }

        [Theory]
        [InlineData(1)]
        public void ProductController_UpdateProduct_ReturnOk(int productId)
        {
            //Assemble
            var product = A.Fake<Product>();
            var existingProduct = A.Fake<ProductDto>();
            A.CallTo(() => _mapper.Map<Product>(existingProduct)).Returns(product);
            A.CallTo(() => _productRep.UpdateProduct(productId, product)).Returns(true);
            var controller = new ProductController(_productRep, _mapper);

            //Act
            var result = controller.UpdateProduct(productId, existingProduct);

            //Assert
            result.Should().NotBeNull();

        }

        [Theory]
        [InlineData(1)]
        public void ProductController_DeleteProduct_ReturnOk(int productId)
        {
            var productToDelete = A.Fake<Product>();
            A.CallTo(() => _productRep.GetProduct(productId)).Returns(productToDelete);
            A.CallTo(() => _productRep.DeleteProduct(productToDelete)).Returns(true);
            var controller = new ProductController(_productRep, _mapper);

            var result = controller.DeleteProduct(productId);

            result.Should().NotBeNull();
        }
    }
}
