using AutoMapper;
using Core.Application.Dto;
using Core.Application.Interfaces;
using Core.Domain.Models;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductController : ControllerBase
    {
        private readonly IProductRep _productRep;
        private readonly IMapper _mapper;

        public ProductController(IProductRep productRep, IMapper mapper)
        {
            _productRep = productRep;
            _mapper = mapper;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProducts()
        {
            var product = _mapper.Map<List<ProductDto>>(_productRep.GetProducts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(product);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(int productId)
        {
            if (!_productRep.ProductExists(productId))
                return NotFound();

            var product = _mapper.Map<ProductDto>(_productRep.GetProduct(productId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(product);
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductDto productDto, [FromQuery] int categoryId)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(productDto);

            _productRep.CreateProduct(product, categoryId);

            var createdProductDto = _mapper.Map<ProductDto>(product);

            return CreatedAtAction(nameof(GetProduct), new { productId = product.Id }, createdProductDto);
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductDto productDto)
        {
            if (!_productRep.ProductExists(productId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = _mapper.Map<Product>(productDto);
            _productRep.UpdateProduct(productId, existingProduct);
            return NoContent();
        }
        [HttpDelete("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int productId)
        {
            if (!_productRep.ProductExists(productId))
            {
                return NotFound();
            }

            var productToDelete = _productRep.GetProduct(productId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_productRep.DeleteProduct(productToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting this product");
            }

            return NoContent();
        }
    }
}
