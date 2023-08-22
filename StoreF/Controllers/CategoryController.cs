﻿using AutoMapper;
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
        private readonly ICategoryRep _categoryRep;

        public CategoryController(ICategoryRep categoryRep, IMapper mapper, INotificationService notificationService)
            : base(mapper, notificationService)
        {
            _categoryRep = categoryRep;
        }

        [HttpGet]
        [ProducesResponseType(200, Type = typeof(IEnumerable<Category>))]
        public IActionResult GetCategories()
        {
            var categories = _categoryRep.GetCategories();
            var categoryDto = _mapper.Map<List<CategoryDto>>(categories);

            return !ModelState.IsValid ? BadRequest(ModelState) : Response(categoryDto);
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

            return Response(category);
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

            _notificationService.Notify("A new category has been created", "Success", ErrorType.Success);

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
    }
}
