using AutoMapper;
using Core.Dto;
using Core.Interfaces;
using Infrastructure.Models;
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

        [HttpPost("{productId}/categories")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult AddCategoriesToProduct(int productId, [FromBody] List<int> categoryIds)
        {
            if (!_productRep.ProductExists(productId))
                return NotFound();

            if (categoryIds == null || categoryIds.Count == 0)
                return BadRequest("Category IDs must be provided.");

            _productRep.AddCategoriesToProduct(productId, categoryIds);

            return Ok();
        }


        [HttpPost("{productId}/setprice")]
        [ProducesResponseType(200)]
        [ProducesResponseType(400)]
        public IActionResult SetPrice(int productId, [FromBody] double price)
        {
            if (!_productRep.ProductExists(productId))
                return NotFound();

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _productRep.SetPrice(productId, price);

            return Ok();
        }

        [HttpPost]
        [ProducesResponseType(201, Type = typeof(ProductDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateProduct([FromBody] ProductDto productDto, [FromQuery] List<int> categoryIds)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var product = _mapper.Map<Product>(productDto);

            _productRep.CreateProduct(product, categoryIds);

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
        public IActionResult DeleteCategory(int productId)
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
