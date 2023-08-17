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
        private readonly IGenericRep _genericRep;

        public ProductController(IProductRep productRep, IMapper mapper, IGenericRep genericRep)
        {
            _productRep = productRep;
            _mapper = mapper;
            _genericRep = genericRep;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Product>))]
        public IActionResult GetProducts()
        {
            var product = _mapper.Map<List<ProductDto>>(_genericRep.GetProducts());

            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            return Ok(product);
        }

        [HttpGet("{productId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProduct(int productId)
        {
            if (!_genericRep.ProductExists(productId))
                return NotFound();

            var product = _mapper.Map<ProductDto>(_genericRep.GetProduct(productId));

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

            _genericRep.CreateProduct(product, categoryId);

            var createdProductDto = _mapper.Map<ProductDto>(product);

            return CreatedAtAction(nameof(GetProduct), new { productId = product.Id }, createdProductDto);
        }

        [HttpPut("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(200)]
        [ProducesResponseType(404)]
        public IActionResult UpdateProduct(int productId, [FromBody] ProductDto productDto)
        {
            if (!_genericRep.ProductExists(productId))
                return NotFound(ModelState);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var existingProduct = _mapper.Map<Product>(productDto);
            _genericRep.UpdateProduct(productId, existingProduct);
            return NoContent();
        }
        [HttpDelete("{productId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteProduct(int productId)
        {
            if (!_genericRep.ProductExists(productId))
            {
                return NotFound();
            }

            var productToDelete = _genericRep.GetProduct(productId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_genericRep.DeleteProduct(productToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting this product");
            }

            return NoContent();
        }
    }
}
