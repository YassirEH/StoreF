using Core.Application.Dto;
using Core.Application.Interfaces;
using Core.Domain.Models;
using FluentAssertions;
using Microsoft.AspNetCore.Mvc;
using webApi.Controllers;

namespace webApi.Test.Controller
{
    public class BuyerControllerTests
    {
        private readonly IMapper _mapper;
        private readonly IBuyerRep _buyerRep;
        public BuyerControllerTests()
        {
            _buyerRep = A.Fake<IBuyerRep>();
            _mapper = A.Fake<IMapper>();
        }

        [Fact]
        public void BuyerController_GetBuyer_ReturnOk()
        {
            //Assemble
            var buyers = A.Fake<BuyerDto>();
            var buyerList = A.Fake<List<BuyerDto>>();
            A.CallTo(() => _mapper.Map<List<BuyerDto>>(buyers)).Returns(buyerList);
            var controller = new BuyerController(_buyerRep, _mapper);

            //Act
            var result = controller.GetBuyer();
            //Assert
            result.Should().NotBeNull();
            result.Should().BeOfType(typeof(OkObjectResult));
        }

        [Theory]
        [InlineData(1)]
        public void BuyerController_GetBuyerById(int buyerId)
        {
            //Assemble
            var buyer = A.Fake<Buyer>();
            var buyerDto = A.Fake<BuyerDto>();

            A.CallTo(() => _mapper.Map<BuyerDto>(buyer)).Returns(buyerDto);
            A.CallTo(() => _buyerRep.BuyerExists(buyerId)).Returns(true);
            A.CallTo(() => _buyerRep.GetBuyer(buyerId)).Returns(buyer);
            var controller = new BuyerController(_buyerRep, _mapper);

            //Act
            var result = controller.GetBuyerById(buyerId);

            //Assert
            result.Should().NotBeNull();
        }

        [Fact]
        public void BuyerController_CreateBuyer_ReturnOk()
        {
            //Assemble
            var buyer = A.Fake<Buyer>();
            var buyerDto = A.Fake<BuyerDto>();
            var buyerCreate = A.Fake<BuyerDto>();
            var buyers = A.Fake<ICollection<BuyerDto>>();
            var buyerList = A.Fake<IList<BuyerDto>>();
            A.CallTo(() => _mapper.Map<Buyer>(buyerDto)).Returns(buyer);
            A.CallTo(() => _buyerRep.CreateBuyer(buyer)).Returns(true);
            var controller = new BuyerController(_buyerRep,_mapper);

            var result = controller.CreateBuyer(buyerCreate);

            result.Should().NotBeNull();


        }
    }
}
