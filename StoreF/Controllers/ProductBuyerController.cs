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
        private readonly IProductBuyerRepository _productBuyerRep;
        private readonly IMapper _mapper;
        private readonly IBuyerRepository _buyerRep;

        public ProductBuyerController(IProductBuyerRepository productBuyerRep, IMapper mapper, IBuyerRepository buyerRep)
        {
            _productBuyerRep = productBuyerRep;
            _mapper = mapper;
            _buyerRep = buyerRep;
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

        [HttpPost("{buyerId}/products")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AssignProductToBuyer(int buyerId, [FromBody] int productId)
        {
            if (!_buyerRep.BuyerExists(buyerId))
                return NotFound();

            if (productId <= 0)
                return BadRequest("Invalid product ID provided.");

            _productBuyerRep.AssignProductToBuyer(buyerId, productId);

            return Ok();
        }
    }
}
