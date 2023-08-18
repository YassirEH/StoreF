using AutoMapper;
using Core.Dto;
using Core.Interfaces;
using Core.Models;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductCategoryController : ControllerBase
    {
        private readonly IProductCategoryRep _productCategoryRep;
        private readonly IMapper _mapper;

        public ProductCategoryController(IProductCategoryRep productCategoryRep, IMapper mapper)
        {
            _productCategoryRep = productCategoryRep;
            _mapper = mapper;
        }

        [HttpGet("Product/{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetProductByCategory(int categoryId)
        {
            var product = _mapper.Map<List<ProductDto>>(_productCategoryRep.GetProductByCategory(categoryId));
            return !ModelState.IsValid ? BadRequest(ModelState) : Ok(product);
        }

        [HttpGet("Category/{productId}")]
        [ProducesResponseType(200, Type = typeof(Product))]
        [ProducesResponseType(400)]
        public IActionResult GetCategoryByProduct(int productId)
        {
            var category = _mapper.Map<List<CategoryDto>>(_productCategoryRep.GetCategoryByProduct(productId));
            return !ModelState.IsValid ? BadRequest(ModelState) : Ok(category);
        }
    }
}
