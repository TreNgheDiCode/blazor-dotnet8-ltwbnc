using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ServerLibrary.Repositories.Interfaces;

namespace Server.Controllers
{
    [Route("api/cloudinary")]
    [ApiController]
    [AllowAnonymous]
    public class CloudinaryController(ICloudinaryRepo cloudinaryRepo) : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> UploadImage(IFormFile file)
        {
            var filePath = Path.GetTempFileName();

            using (var stream = System.IO.File.Create(filePath))
            {
                await file.CopyToAsync(stream);
            }

            var result = await cloudinaryRepo.UploadImageAsync(filePath);

            return Ok(result);
        }
    }
}
