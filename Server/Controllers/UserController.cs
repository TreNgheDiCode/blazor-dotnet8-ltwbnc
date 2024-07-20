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
            // Check if the user has the "Admin" role
            if (!User.IsInRole("Admin"))
            {
                // Return a custom error response
                return Unauthorized(new { Error = "Quyền hạn không hợp lệ." });
            }

            var result = await repo.GetUsers(page, pageSize);

            return Ok(result);
        }

        [HttpGet("users/{id}")]
        public async Task<IActionResult> GetUser(int id)
        {
            var result = await repo.GetUser(id);
            return Ok(result);
        }

        [HttpPut("users/{id}")]
        public async Task<IActionResult> UpdateUser(int id, UpdateUserDTO user)
        {
            var result = await repo.UpdateUser(id, user);
            return Ok(result);
        }

        [HttpDelete("users/{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            // Check if the user has the "Admin" role
            if (!User.IsInRole("Admin"))
            {
                // Return a custom error response
                return Unauthorized(new { Error = "Quyền hạn không hợp lệ." });
            }

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
