using BaseLibrary.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/")]
    [ApiController]
    public class UserController(IUserRepo repo) : ControllerBase
    {
        [HttpGet("users")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetUsers(int? page, int? pageSize)
        {
            var result = await repo.GetUsers(page, pageSize);

            if (result.Success == true)
            {
                return Ok(result);
            } else
            {
                return BadRequest(result);
            }
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await repo.GetUser(id);

            if (result.Success == true)
            {
                return Ok(result);
            } else
            {
                return BadRequest(result);
            }
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO user)
        {
            var result = await repo.UpdateUser(id, user);

            if (result.Flag)
            {
                return Ok(result);
            } else
            {
                return BadRequest(result);
            }
        }

        [HttpPost("users/{id}/lock")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> LockUser(int id)
        {
            var result = await repo.LockUser(id);

            if (result.Flag)
            {
                return Ok(result);
            } else
            {
                return BadRequest(result);
            }
        }

        [HttpDelete("users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var result = await repo.DeleteUser(id);

            if (result.Flag)
            {
                return Ok(result);
            } else
            {
                return BadRequest(result);
            }
        }
    }
}
