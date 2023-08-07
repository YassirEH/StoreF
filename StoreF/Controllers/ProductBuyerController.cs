using AutoMapper;
using Core.Dto;
using Core.Interfaces;
using Infrastructure.Models;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductBuyerController : ControllerBase
    {
        private readonly IProductBuyerRep _productBuyerRep;
        private readonly IMapper _mapper;

        public ProductBuyerController(IProductBuyerRep productBuyerRep, IMapper mapper)
        {
            _productBuyerRep = productBuyerRep;
            _mapper = mapper;
        }

        [HttpGet("Product/{buyerId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetproductByBuyer(int buyerId)
        {
            var product = _mapper.Map<List<ProductDto>>(_productBuyerRep.GetProductBuyer(buyerId));

            return !ModelState.IsValid ? BadRequest(ModelState) : Ok(product);
        }

        [HttpGet("Buyer/{productId}")]
        [ProducesResponseType(200, Type = typeof(Buyer))]
        [ProducesResponseType(400)]
        public IActionResult GetBuyerOfProduct(int productId)
        {
            var buyer = _mapper.Map<List<BuyerDto>>(_productBuyerRep.GetBuyerOfProduct(productId));

            return !ModelState.IsValid ? BadRequest(ModelState) : Ok(buyer);
        }
    }
}
