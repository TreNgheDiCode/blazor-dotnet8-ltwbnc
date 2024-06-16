using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class CategoryController(ICategoryRepo categoryRepo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCategories(int? page, int? pageSize)
        {
            var result = await categoryRepo.GetCategories(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await categoryRepo.GetCategory(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddCategory(CreateCategoryDTO category)
        {
            var result = await categoryRepo.AddCategory(category);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDTO category)
        {
            var result = await categoryRepo.UpdateCategory(id, category);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await categoryRepo.DeleteCategory(id);
            return Ok(result);
        }
    }
}
