using BaseLibrary.DTOs;
using BaseLibrary.Models.Products;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Emplementations;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepo categoryRepo;
        public CategoryController(ICategoryRepo categoryRepo)
        {
            this.categoryRepo = categoryRepo;
        }

        [HttpGet("Get-Category/{IdCategory:int}")]
        public async Task<ActionResult<ServiceCategory>> GetCategory(int IdCategory)
        {
            return Ok(await categoryRepo.GetCategory(IdCategory));
        }
        [HttpPost("Add-Category")]
        public async Task<ActionResult<ServiceCategory>> AddCategory(Category category)
        {
            return Ok(await categoryRepo.AddCategory(category));
        }
        [HttpGet("Get-Categories")]
        public async Task<ActionResult<ServiceCategory>>GetCategories()
        {
            return Ok(await categoryRepo.GetCategories());
        }

        [HttpDelete("Delete-Category/{IdCategory:int}")]
        public async Task<ActionResult<ServiceCategory>> DeteleCategory(int IdCategory)
        {
            return Ok(await categoryRepo.DeleteCategory(IdCategory));
        }
        [HttpPost("Update-Category/{IdCategory:int}")]
        public async Task<ActionResult<ServiceCategory>> UpdateCategory(int IdCategory,string? name)
        {
            return Ok(await categoryRepo.UpdateCategory(IdCategory,name));
        }
    }
}
