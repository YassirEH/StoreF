using AutoMapper;
using Core.Application.Dto;
using Core.Application.Interfaces;
using Core.Domain.Models;
using Infrastructure.Data;
using Microsoft.AspNetCore.Mvc;

namespace webApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRep _categoryRep;
        private readonly IMapper _mapper;
        private readonly IGenericRep _genericRep;

        public CategoryController(ICategoryRep categoryRep, IMapper mapper, IGenericRep genericRep)
        {
            _categoryRep = categoryRep;
            _mapper = mapper;
            _genericRep = genericRep;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _genericRep.GetCategories();
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories);

            return !ModelState.IsValid ? BadRequest(ModelState) : Ok(categoryDto);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_genericRep.CategoryExists(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_genericRep.GetCategory(categoryId));

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return Ok(category);
        }

        [HttpPost]
        [ProducesResponseType(204, Type = typeof(CategoryDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            var category = _mapper.Map<Category>(categoryDto);
            _genericRep.CreateCategory(category);
            var createdCategoryDto = _mapper.Map<CategoryDto>(category);
            return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, createdCategoryDto);
        }

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
        {
            if (updatedCategory == null)
                return BadRequest(ModelState);

            var existingCategory = _genericRep.GetCategory(categoryId);
            if (existingCategory == null)
                return NotFound();

            existingCategory.Name = updatedCategory.Name;


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            return NoContent();
        }


        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_genericRep.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _genericRep.GetCategory(categoryId);

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!_genericRep.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
                return BadRequest(ModelState); // Return BadRequestObjectResult here
            }

            return NoContent();
        }
    }
}
