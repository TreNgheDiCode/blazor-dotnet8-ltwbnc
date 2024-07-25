using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/discounts")]
    [ApiController]
    [AllowAnonymous]
    public class DiscountController(IDiscountRepo repo) : ControllerBase
    {
        [HttpGet]
        public async Task<IActionResult> GetDiscounts(int? page, int? pageSize)
        {
            var result = await repo.GetDiscounts(page, pageSize);

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
        public async Task<IActionResult> GetDiscount(int id)
        {
            var result = await repo.GetDiscount(id);

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
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> AddDiscount(CreateDiscountDTO discount)
        {
            var result = await repo.AddDiscount(discount);

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
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> UpdateDiscount(int id, UpdateDiscountDTO discount)
        {
            var result = await repo.UpdateDiscount(id, discount);

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
        [Authorize(Roles="Admin")]
        public async Task<IActionResult> DeleteDiscount(int id)
        {
            var result = await repo.DeleteDiscount(id);

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
