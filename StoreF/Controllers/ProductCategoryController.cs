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
    public class ProductCategoryController : APIController
    {
        private readonly IMapper _mapper;
        private readonly ILogger _logger;
        private readonly IProductCategoryRep _productCategoryRep;


        public ProductCategoryController(IProductCategoryRep productCategoryRep, IMapper mapper, INotificationService notificationService, ILogger logger)
            : base(notificationService)
        {
            _mapper = mapper;
            _logger = logger;
            _productCategoryRep = productCategoryRep;
        }

        [HttpGet("Product/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProductByCategory(int categoryId)
        {
            try
            {
                var products = _mapper.Map<List<ProductDto>>(_productCategoryRep.GetProductByCategory(categoryId));

                if (!ModelState.IsValid)
                {
                    _notificationService.Notify("Invalid input", "Error", ErrorType.Error);
                    return BadRequest(ModelState);
                }

                return Response(products);
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

        [HttpGet("Category/{productId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetCategoryByProduct(int productId)
        {
            try
            {
                var categories = _mapper.Map<List<CategoryDto>>(_productCategoryRep.GetCategoryByProduct(productId));

                if (!ModelState.IsValid)
                {
                    _notificationService.Notify("Invalid input", "Error", ErrorType.Error);
                    return BadRequest(ModelState);
                }

                return Response(categories);
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
