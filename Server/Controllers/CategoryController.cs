using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/categories")]
    [ApiController]
    [AllowAnonymous]
    public class CategoryController(ICategoryRepo categoryRepo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetCategories(int? page, int? pageSize)
        {
            var result = await categoryRepo.GetCategories(page, pageSize);

            if (result.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetCategory(int id)
        {
            var result = await categoryRepo.GetCategory(id);

            if (result.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("name/{name}")]
        public async Task<IActionResult> GetCategoryByName(string name)
        {
            var result = await categoryRepo.GetCategoryByName(name);

            if (result.Success == true)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> AddCategory(CreateCategoryDTO category)
        {
            var result = await categoryRepo.AddCategory(category);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateCategory(int id, UpdateCategoryDTO category)
        {
            var result = await categoryRepo.UpdateCategory(id, category);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var result = await categoryRepo.DeleteCategory(id);

            if (result.Flag)
            {
                return Ok(result);
            }
            else
            {
                return BadRequest(result);
            }
        }
    }
}
