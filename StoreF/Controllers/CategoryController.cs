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
    public class CategoryController : APIController
    {
        private readonly IMapper _mapper;
        private readonly ICategoryRep _categoryRep;
        private readonly ILogger _logger;

        public CategoryController(ICategoryRep categoryRep, IMapper mapper, INotificationService notificationService, ILogger logger)
            : base(notificationService)
        {
            _mapper = mapper;
            _categoryRep = categoryRep;
            _logger = logger;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            try
            {
                var categories = _categoryRep.GetCategories();
                var categoryDto = _mapper.Map<List<CategoryDto>>(categories);

                return !ModelState.IsValid ? BadRequest(ModelState) : Response(categoryDto);
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

        [HttpGet("{categoryId}")]
        [ProducesResponseType(200, Type = typeof(Category))]
        [ProducesResponseType(400)]
        public IActionResult GetCategory(int categoryId)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                //if (!_categoryRep.CategoryExists(categoryId))  return NotFound();

                var category = _mapper.Map<CategoryDto>(_categoryRep.GetCategory(categoryId));

                return Response(category);
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
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult FilterByName()
        {
            try
            {
                var categories = _categoryRep.FilterByName();
                var categoryDto = _mapper.Map<List<CategoryDto>>(categories);

                return !ModelState.IsValid ? BadRequest(ModelState) : Response(categoryDto);
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

        [HttpPost]
        [ProducesResponseType(204, Type = typeof(CategoryDto))]
        [ProducesResponseType(400)]
        public IActionResult CreateCategory([FromBody] CategoryDto categoryDto)
        {
            try
            {
                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

                var category = _mapper.Map<Category>(categoryDto);
                _categoryRep.CreateCategory(category);
                var createdCategoryDto = _mapper.Map<CategoryDto>(category);

                _notificationService.Notify("A new category has been created", "Success", ErrorType.Success);

                return CreatedAtAction(nameof(GetCategory), new { categoryId = category.Id }, createdCategoryDto);
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

        [HttpPut("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult UpdateCategory(int categoryId, [FromBody] CategoryDto updatedCategory)
        {
            try
            {
                if (updatedCategory == null)
                    return BadRequest(ModelState);

                var existingCategory = _categoryRep.GetCategory(categoryId);
                if (existingCategory == null)
                    return NotFound();

                existingCategory.Name = updatedCategory.Name;

                if (!ModelState.IsValid)
                    return BadRequest(ModelState);

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

        [HttpDelete("{categoryId}")]
        [ProducesResponseType(400)]
        [ProducesResponseType(204)]
        [ProducesResponseType(404)]
        public IActionResult DeleteCategory(int categoryId)
        {
            try
            {
                if (!_categoryRep.CategoryExists(categoryId))
                {
                    return NotFound();
                }

                var categoryToDelete = _categoryRep.GetCategory(categoryId);

                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                bool deleteResult = _categoryRep.DeleteCategory(categoryToDelete);

                if (deleteResult)
                {
                    _notificationService.Notify("The category has been deleted", "Success", ErrorType.Success);
                    return NoContent();
                }
                else
                {
                    ModelState.AddModelError("", "Something went wrong deleting this category");
                    _notificationService.Notify("Error deleting the category", "Error", ErrorType.Error);
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
