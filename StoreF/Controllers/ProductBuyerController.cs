using AutoMapper;
using Core.Dto;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductBuyerController : APIController
    {
        private readonly IMapper _mapper;
        private readonly IProductBuyerRep _productBuyerRep;
        private readonly IBuyerRep _buyerRep;
        private readonly ILogger _logger;

        public ProductBuyerController(IProductBuyerRep productBuyerRep, IMapper mapper, IBuyerRep buyerRep, INotificationService notificationService, ILogger logger)
            : base(notificationService)
        {
            _mapper = mapper;
            _productBuyerRep = productBuyerRep;
            _buyerRep = buyerRep;
            _logger = logger;
        }

        [HttpGet("Product/{buyerId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetproductByBuyer(int buyerId)
        {
            try
            {
                var product = _mapper.Map<List<ProductDto>>(_productBuyerRep.GetProductBuyer(buyerId));
                return !ModelState.IsValid ? BadRequest(ModelState) : Response(product);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "A null argument was passed.");
                return BadRequest("A null argument was passed. Please check your request and try again.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation was attempted.");
                return BadRequest("An invalid operation was attempted. Please check your request and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpGet("Buyer/{productId}")]
        [ProducesResponseType(200, Type = typeof(Buyer))]
        [ProducesResponseType(400)]
        public IActionResult GetBuyerOfProduct(int productId)
        {
            try
            {
                var buyer = _mapper.Map<List<BuyerDto>>(_productBuyerRep.GetBuyerOfProduct(productId));
                return !ModelState.IsValid ? BadRequest(ModelState) : Response(buyer);
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "A null argument was passed.");
                return BadRequest("A null argument was passed. Please check your request and try again.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation was attempted.");
                return BadRequest("An invalid operation was attempted. Please check your request and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }

        [HttpPost("{buyerId}/products")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AssignProductToBuyer(int buyerId, [FromBody] int productId)
        {
            try
            {
                if (!_buyerRep.BuyerExists(buyerId))
                {
                    _notificationService.Notify("Buyer not found", "Error", ErrorType.NotFound);
                    return NotFound();
                }

                if (productId <= 0)
                {
                    _notificationService.Notify("Invalid product ID provided", "Error", ErrorType.Error);
                    return BadRequest("Invalid product ID provided.");
                }

                _productBuyerRep.AssignProductToBuyer(buyerId, productId);

                _notificationService.Notify("Product assigned to buyer", "Success", ErrorType.Success);
                return Ok();
            }
            catch (ArgumentNullException ex)
            {
                _logger.LogError(ex, "A null argument was passed.");
                return BadRequest("A null argument was passed. Please check your request and try again.");
            }
            catch (InvalidOperationException ex)
            {
                _logger.LogError(ex, "An invalid operation was attempted.");
                return BadRequest("An invalid operation was attempted. Please check your request and try again.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while processing the request.");
                return BadRequest("An error occurred while processing your request. Please try again later.");
            }
        }
    }
}
