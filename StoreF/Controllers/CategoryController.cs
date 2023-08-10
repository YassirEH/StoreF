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
        private readonly DataContext _context; // Add this line to define the DataContext variable

        public CategoryController(ICategoryRep categoryRep, IMapper mapper, DataContext context)
        {
            _categoryRep = categoryRep;
            _mapper = mapper;
            _context = context; // Initialize the DataContext variable through dependency injection
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryRep.GetCategories();
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories);

            return !ModelState.IsValid ? BadRequest(ModelState) : Ok(categoryDto);
        }

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            if (!_categoryRep.CategoryExists(categoryId))
                return NotFound();

            var category = _mapper.Map<CategoryDto>(_categoryRep.GetCategory(categoryId));

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
            _categoryRep.CreateCategory(category);
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

            var existingCategory = _categoryRep.GetCategory(categoryId);
            if (existingCategory == null)
                return NotFound();

            existingCategory.Name = updatedCategory.Name;


            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            _context.SaveChanges();

            return NoContent();
        }


        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            if (!_categoryRep.CategoryExists(categoryId))
            {
                return NotFound();
            }

            var categoryToDelete = _categoryRep.GetCategory(categoryId);

            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            if (!_categoryRep.DeleteCategory(categoryToDelete))
            {
                ModelState.AddModelError("", "Something went wrong deleting category");
            }

            return NoContent();
        }
    }
}
