using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/discounts")]
    [ApiController]
    [Authorize(Roles = "Admin")]
    public class DiscountController(IDiscountRepo repo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetDiscounts(int? page, int? pageSize)
        {
            var result = await repo.GetDiscounts(page, pageSize);
            return Ok(result);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetDiscount(int id)
        {
            var result = await repo.GetDiscount(id);
            return Ok(result);
        }

        [HttpPost]
        public async Task<IActionResult> AddDiscount(CreateDiscountDTO discount)
        {
            var result = await repo.AddDiscount(discount);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateDiscount(int id, UpdateDiscountDTO discount)
        {
            var result = await repo.UpdateDiscount(id, discount);
            return Ok(result);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var result = await repo.DeleteDiscount(id);
            return Ok(result);
        }
    }
}
