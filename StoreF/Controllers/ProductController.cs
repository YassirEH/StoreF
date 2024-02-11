using AutoMapper;
using Core.Dto;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;
using webApi.Application.Services;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : APIController
    {
        private readonly IMapper _mapper;
        private readonly IProductRep _productRep;
        private readonly ILogger _logger;

        public ProductController(IProductRep productRep, IMapper mapper, INotificationService notificationService, ILogger logger)
            : base(notificationService)
        {
            _mapper = mapper;
            _productRep = productRep;
            _logger = logger;
        }

        //------All Get Methods-------------------------------------------------------------------------------------------------------------------------------

        [HttpGet("Get All")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProducts()
        {
            try
            {
                var products = _mapper.Map<List<ProductDto>>(_productRep.GetProducts());

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

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

        [HttpGet("{productId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(int productId)
        {
            try
            {
                if (!_productRep.ProductExists(productId))
                {
                    _notificationService.Notify("Product not found", "Error", ErrorType.NotFound);
                    return NotFound();
                }

                var product = _mapper.Map<ProductDto>(_productRep.GetProduct(productId));

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                return Response(product);
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

        [HttpGet("Filter By Name")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult FilterByName()
        {
            try
            {
                var products = _mapper.Map<List<ProductDto>>(_productRep.FilterByName());

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

        [HttpGet("Filter By Price")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult FilterByPrice()
        {
            try
            {
                var products = _mapper.Map<List<ProductDto>>(_productRep.FilterByPrice());

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

        [HttpGet("Filter By Quantity")]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult FilterByQuantity()
        {
            try
            {
                var products = _mapper.Map<List<ProductDto>>(_productRep.FilterByQuantity());

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


        //------Post----Put----Delete---------------------------------------------------------------------------------------------------------------------------

        [HttpPost("Create Product/{categoryId}")]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductDto productDto, int categoryId)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    _notificationService.Notify("Invalid product data", "Error", ErrorType.Error);
                    return BadRequest(ModelState);
                }

                var product = _mapper.Map<Product>(productDto);

                _productRep.CreateProduct(product, categoryId);

                var createdProductDto = _mapper.Map<ProductDto>(product);

                _notificationService.Notify("A new product has been created", "Success", ErrorType.Success);

                return CreatedAtAction(nameof(GetProduct), new { productId = product.Id }, createdProductDto);
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


        [HttpPut("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductDto productDto)
        {
            try
            {
                if (!_productRep.ProductExists(productId))
                {
                    _notificationService.Notify("Product not found", "Error", ErrorType.NotFound);
                    return NotFound(ModelState);
                }

                if (!ModelState.IsValid)
                {
                    _notificationService.Notify("Invalid product data", "Error", ErrorType.Error);
                    return BadRequest(ModelState);
                }

                var existingProduct = _mapper.Map<Product>(productDto);
                _productRep.UpdateProduct(productId, existingProduct);

                _notificationService.Notify("Product updated successfully", "Success", ErrorType.Success);

                return NoContent();
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

        [HttpDelete("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int productId)
        {
            try
            {
                if (!_productRep.ProductExists(productId))
                {
                    return NotFound();
                }

                var productToDelete = _productRep.GetProduct(productId);

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                bool deleteResult = _productRep.DeleteProduct(productToDelete);

                if (deleteResult)
                {
                    _notificationService.Notify("Product deleted successfully", "Success", ErrorType.Success);
                    return NoContent();
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong deleting this product");
                    _notificationService.Notify("Error deleting the product", "Error", ErrorType.Error);
                    return BadRequest(ModelState);
                }
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