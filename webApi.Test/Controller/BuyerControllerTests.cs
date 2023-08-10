using Core.Application.Dto;
using Core.Application.Interfaces;
using Core.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using webApi.Controllers;
using FakeItEasy;

namespace webApi.Test
{
    public class BuyerControllerTests
    {
        private readonly Mock<IBuyerRep> _buyerRepMock;
        private readonly Mock<IMapper> _mapperMock;
        private readonly BuyerController _controller;
        private readonly IMapper _mapper;

        public BuyerControllerTests()
        {
            _buyerRepMock = new Mock<IBuyerRep>();
            _mapperMock = new Mock<IMapper>();
            _controller = new BuyerController(_buyerRepMock.Object, _mapperMock.Object);
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void GetBuyer_ReturnsOkResult()
        {
            // Arrange
            var buyers = new List<Buyer>();
            var buyerDtos = new List<BuyerDto>();
            _buyerRepMock.Setup(repo => repo.GetBuyers()).Returns(buyers);
            _mapperMock.Setup(mapper => mapper.Map<List<BuyerDto>>(buyers)).Returns(buyerDtos);

            // Act
            var result = _controller.GetBuyer();

            // Assert
            Assert.IsType<OkObjectResult>(result);
        }

        [Fact]
        public void GetBuyerById_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var buyerId = 1;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.GetBuyerById(buyerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void CreateBuyer_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var buyerDto = new BuyerDto();
            _controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = _controller.CreateBuyer(buyerDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void UpdateBuyer_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var buyerId = 1;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.UpdateBuyer(buyerId, new BuyerDto());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateBuyer_InvalidModel_ReturnsBadRequest()
        {
            // Arrange
            var buyerId = 1;
            var buyerDto = new BuyerDto();
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(true);
            _controller.ModelState.AddModelError("key", "error message");

            // Act
            var result = _controller.UpdateBuyer(buyerId, buyerDto);

            // Assert
            Assert.IsType<BadRequestObjectResult>(result);
        }

        [Fact]
        public void DeleteBuyer_InvalidId_ReturnsNotFound()
        {
            // Arrange
            var buyerId = 1;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.DeleteBuyer(buyerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void GetBuyer_ValidData_ReturnsOkWithListOfBuyerDto()
        {
            // Arrange
            var buyers = new List<Buyer>();
            _buyerRepMock.Setup(repo => repo.GetBuyers()).Returns(buyers);
            var buyerDtos = new List<BuyerDto>();
            _mapperMock.Setup(mapper => mapper.Map<List<BuyerDto>>(buyers)).Returns(buyerDtos);

            // Act
            var result = _controller.GetBuyer();

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBuyerDtos = Assert.IsAssignableFrom<IEnumerable<BuyerDto>>(okResult.Value);
            Assert.Equal(buyerDtos, returnedBuyerDtos);
        }

        [Fact]
        public void GetBuyerById_ExistingId_ReturnsOkWithBuyerDto()
        {
            // Arrange
            var buyerId = 1;
            var buyer = new Buyer();
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(true);
            _buyerRepMock.Setup(repo => repo.GetBuyer(buyerId)).Returns(buyer);
            var buyerDto = new BuyerDto();
            _mapperMock.Setup(mapper => mapper.Map<BuyerDto>(buyer)).Returns(buyerDto);

            // Act
            var result = _controller.GetBuyerById(buyerId);

            // Assert
            var okResult = Assert.IsType<OkObjectResult>(result);
            var returnedBuyerDto = Assert.IsType<BuyerDto>(okResult.Value);
            Assert.Equal(buyerDto, returnedBuyerDto);
        }

        [Fact]
        public void GetBuyerById_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var buyerId = 1;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.GetBuyerById(buyerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void UpdateBuyer_ExistingIdAndValidData_ReturnsNoContent()
        {
            // Arrange
            var buyerId = 1;
            var buyerDto = new BuyerDto();
            var existingBuyer = new Buyer();
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(true);
            _buyerRepMock.Setup(repo => repo.GetBuyer(buyerId)).Returns(existingBuyer);

            // Act
            var result = _controller.UpdateBuyer(buyerId, buyerDto);

            // Assert
            Assert.IsType<NoContentResult>(result);
            _buyerRepMock.Verify(repo => repo.UpdateBuyer(existingBuyer), Moq.Times.Once);
        }

        [Fact]
        public void UpdateBuyer_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var buyerId = 1;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.UpdateBuyer(buyerId, new BuyerDto());

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

        [Fact]
        public void DeleteBuyer_ExistingId_ReturnsNoContent()
        {
            // Arrange
            var buyerId = 1;
            var existingBuyer = new Buyer();
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(true);
            _buyerRepMock.Setup(repo => repo.GetBuyer(buyerId)).Returns(existingBuyer);
            _buyerRepMock.Setup(repo => repo.DeleteBuyer(existingBuyer)).Returns(true);

            // Act
            var result = _controller.DeleteBuyer(buyerId);

            // Assert
            Assert.IsType<NoContentResult>(result);
        }

        [Fact]
        public void DeleteBuyer_NonExistingId_ReturnsNotFound()
        {
            // Arrange
            var buyerId = 1;
            _buyerRepMock.Setup(repo => repo.BuyerExists(buyerId)).Returns(false);

            // Act
            var result = _controller.DeleteBuyer(buyerId);

            // Assert
            Assert.IsType<NotFoundResult>(result);
        }

    }
}
